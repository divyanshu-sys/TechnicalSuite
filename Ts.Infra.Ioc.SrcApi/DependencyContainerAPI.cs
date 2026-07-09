using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ts.Application.AppInterfaces;
using Ts.Application.AppServices;
using Ts.Application.AppSettings;
using Ts.Application.SecurityHandlers;
using Ts.Common.AppInterfaces;
using Ts.Common.AppServices;
namespace Ts.Infra.Ioc.SrcApi
{
    public static class DependencyContainerApi
    {
        public static void AddServicesForApi(this IServiceCollection services, IConfiguration configuration)
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

            #region Register Authentication
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            #endregion

            #region Register Controllers functionality
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                config.Filters.Add(new ProducesAttribute("application/json", "text/json", "application/xml", "text/xml", "text/plain"));

                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();
            #endregion

            #region Configure Settings from AppSetting
            // Configure Email Setting
            services.Configure<EmailSetting>(configuration.GetSection(nameof(EmailSetting)));
            #endregion

            #region Register HelperServices
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IRandomService, RandomService>();
            services.AddSingleton<IFileValidationService, FileValidationService>();
            services.AddSingleton<IFileService, FileService>();
            #endregion

            #region Add CORS
            services.AddCors();
            #endregion
        }
    }
}
