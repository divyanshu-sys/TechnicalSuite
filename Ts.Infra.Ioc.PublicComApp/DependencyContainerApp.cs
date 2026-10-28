using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Timeout;
using System.Net;
using Ts.Client.HttpClientServices;
using Ts.DotCom.Client.AutoMapper;
using Ts.DotCom.Client.Resilience;
using WebMarkupMin.AspNetCore8;
namespace Ts.Infra.Ioc.PublicComApp
{
    public static class DependencyContainerApp
    {
        public static void AddServicesForRazorPagesApp(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            #region Set MaxRequestBodySize
            var AllowedFormSize = Convert.ToInt32(configuration["AllowedFormSize"]);
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = AllowedFormSize;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = AllowedFormSize; // if not set then default value is: 30 MB
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = AllowedFormSize;
                x.MultipartBodyLengthLimit = AllowedFormSize;
                x.MultipartHeadersLengthLimit = AllowedFormSize;
            });
            #endregion

            #region Lowercasing URL
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            #endregion

            #region Register RazorPages
            services.AddRazorPages().AddRazorPagesOptions(config =>
            {

            }).AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();
            #endregion

            #region Registering for Compressing Html, Css, Js
            services.AddWebMarkupMin(options =>
            {
                options.AllowMinificationInDevelopmentEnvironment = true;
                options.AllowCompressionInDevelopmentEnvironment = true;
                options.DisablePoweredByHttpHeaders = true;
            }).AddHtmlMinification(options =>
            {
                options.MinificationSettings.RemoveOptionalEndTags = false;
                options.MinificationSettings.AttributeQuotesRemovalMode = WebMarkupMin.Core.HtmlAttributeQuotesRemovalMode.KeepQuotes;
                options.MinificationSettings.EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.SpaceAndSlash;
                options.MinificationSettings.WhitespaceMinificationMode = WebMarkupMin.Core.WhitespaceMinificationMode.Aggressive;
                options.MinificationSettings.MinifyEmbeddedJsCode = true;
                options.MinificationSettings.MinifyEmbeddedCssCode = true;
            }).AddHttpCompression();
            #endregion

            #region Configure Antiforgery
            services.AddAntiforgery(o =>
            {
                o.HeaderName = "XSRF-TOKEN";
                o.Cookie.Domain = configuration["Domain"];
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
                o.Cookie.Name = configuration["AntiforgeryName"];
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.Path = "/";
                o.Cookie.SameSite = SameSiteMode.None;
            });
            #endregion

            #region Register AutoMapper
            services.AddSingleton(AutoMapperConfigurationApp.RegisterAutoMapperProfilesApp().CreateMapper());
            #endregion

            #region Register HttpClient
            services.AddHttpContextAccessor();
            services.Configure<DotComResilienceSetting>(configuration.GetSection(nameof(DotComResilienceSetting)));
            services.AddHttpClient<IHttpPublicService, HttpPublicService>().AddResilience();
            #endregion
        }

        private static IHttpClientBuilder AddResilience(this IHttpClientBuilder httpClientBuilder)
        {
            httpClientBuilder.AddResilienceHandler("webapi-request", (builder, context) =>
            {
                var options = context.ServiceProvider
                                     .GetRequiredService<IOptions<DotComResilienceSetting>>()
                                     .Value;

                var policy = options.Rest.WebApi;

                builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                {
                    FailureRatio = policy.FailureRatio,
                    MinimumThroughput = policy.MinimumThroughput,
                    SamplingDuration = TimeSpan.FromSeconds(policy.SamplingDurationSeconds),
                    BreakDuration = TimeSpan.FromSeconds(policy.BreakDurationSeconds),
                    ShouldHandle = static args =>
                    {
                        if (args.Outcome.Exception is HttpRequestException or TimeoutRejectedException)
                            return ValueTask.FromResult(true);

                        return ValueTask.FromResult(
                            args.Outcome.Result is HttpResponseMessage r &&
                            (
                                r.StatusCode is HttpStatusCode.RequestTimeout or
                                HttpStatusCode.TooManyRequests ||
                                (int)r.StatusCode >= 500
                            )
                        );
                    },
                    OnOpened = args =>
                    {
                        var logger = context.ServiceProvider
                                            .GetRequiredService<ILoggerFactory>()
                                            .CreateLogger("Resilience.WebApi");
                        logger.LogWarning("WebApi circuit OPENED for {BreakDuration}. Reason: {Reason}",
                            args.BreakDuration,
                            args.Outcome.Exception?.Message ?? args.Outcome.Result?.StatusCode.ToString());
                        return ValueTask.CompletedTask;
                    },
                    OnHalfOpened = args =>
                    {
                        var logger = context.ServiceProvider
                                            .GetRequiredService<ILoggerFactory>()
                                            .CreateLogger("Resilience.WebApi");
                        logger.LogWarning("WebApi circuit is HALF-OPEN (testing).");
                        return ValueTask.CompletedTask;
                    },
                    OnClosed = args =>
                    {
                        var logger = context.ServiceProvider
                                            .GetRequiredService<ILoggerFactory>()
                                            .CreateLogger("Resilience.WebApi");
                        logger.LogInformation("WebApi circuit CLOSED (service recovered).");
                        return ValueTask.CompletedTask;
                    }
                });
            });
            return httpClientBuilder;
        }
    }
}