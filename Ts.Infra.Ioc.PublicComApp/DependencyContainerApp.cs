using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ts.Client.HttpClientServices;
using Ts.DotCom.Client.AutoMapper;
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
            services.AddHttpClient<IHttpPublicService, HttpPublicService>();
            #endregion
        }
    }
}