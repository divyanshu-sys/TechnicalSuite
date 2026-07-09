using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ts.Application.Client.SecurityHandlers;
using Ts.Client.AutoMapper;
using Ts.Client.HttpClientServices;
using Ts.Common.AppInterfaces;
using Ts.Common.AppServices;
using Ts.Common.Constant.AppConstants;
using WebMarkupMin.AspNetCore8;
using DotComAutoMapper = Ts.DotCom.Client.AutoMapper;
using DotInAutoMapper = Ts.DotIn.Client.AutoMapper;
using ShopInAutoMapper = Ts.ShopIn.Client.AutoMapper;
namespace Ts.Infra.Ioc.AdminApp
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

            #region Register Authentication and Authorization
            // Add Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.ClaimsIssuer = configuration["ClaimsIssuer"];
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/accessdenied";
                    options.ReturnUrlParameter = "returnurl";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(configuration["CookieExpireTimeInMinute"]));
                    options.SlidingExpiration = false;

                    options.Cookie.Name = configuration["CookieName"];
                    options.Cookie.Domain = configuration["Domain"];
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.Path = "/";
                });

            // Add Authorization
            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;

                foreach (var privileges in RoleConstant.GetApplicationPrivileges().Values)
                {
                    foreach (var policies in privileges.Values)
                    {
                        foreach (var claim in policies)
                        {
                            options.AddPolicy(claim, p => p.RequireClaim(claim));
                        }
                    }
                }
            });
            #endregion

            #region Register RazorPages
            services.AddRazorPages().AddRazorPagesOptions(config =>
            {
                var policy = new AuthorizationPolicyBuilder().AddRequirements(new ValidateBrowserUiRequirement(), new ChangePasswordUiRequirement()).RequireAuthenticatedUser().Build();
                config.Conventions.ConfigureFilter(new AuthorizeFilter(policy));
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

            #region Register AuthorizationHandler
            // This handler is used to check user Browser detail
            services.AddScoped<IAuthorizationHandler, RazorValidateBrowserUiHandler>();

            // This handler is used to check if user needs to change password
            services.AddSingleton<IAuthorizationHandler, RazorChangePasswordUiHandler>();
            #endregion

            #region Register Helper services
            services.AddSingleton<IFileValidationService, FileValidationService>();
            #endregion

            #region This service is used to detect Browser information
            services.AddBrowserDetection();
            #endregion

            #region Saving Antiforgery in File because of Shared hosting
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(env.ContentRootPath, "DataProtectionKey")))
                .SetApplicationName(configuration["SiteTitle"])
                .SetDefaultKeyLifetime(TimeSpan.FromDays(Convert.ToDouble(configuration["DataProtectionKeyExpireTimeInDays"])));
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
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileApp());
                config.AddProfile(new DotInAutoMapper.AutoMapperProfileApp());
                config.AddProfile(new DotComAutoMapper.AutoMapperProfileApp());
                config.AddProfile(new ShopInAutoMapper.AutoMapperProfileApp());
            });
            services.AddSingleton(mapperConfig.CreateMapper());
            #endregion

            #region Register HttpClient
            services.AddHttpContextAccessor();
            services.AddHttpClient<IHttpClientService, HttpClientService>();
            #endregion
        }
    }
}