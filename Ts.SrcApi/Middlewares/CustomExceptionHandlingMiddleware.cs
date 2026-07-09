using Microsoft.Extensions.Options;
using System.Net;
using Ts.Application.AppInterfaces;
using Ts.Application.AppSettings;
using Ts.Application.Helpers.HelperClasses;
using Ts.Common.HelperExtensions;
namespace Ts.SrcApi.Middlewares
{
    public class CustomExceptionHandlingMiddleware(RequestDelegate next, IEmailService emailService,
        IConfiguration config, ILogger<CustomExceptionHandlingMiddleware> logger,
        IWebHostEnvironment webHostEnvironment, IOptions<EmailSetting> emailSetting)
    {
        private readonly RequestDelegate next = next;
        private readonly IEmailService emailService = emailService;
        private readonly IConfiguration config = config;
        private readonly ILogger<CustomExceptionHandlingMiddleware> logger = logger;
        private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
        private readonly IEmailSetting emailSetting = emailSetting.Value;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Send email
                if (config.GetValue<bool>("IsSendExceptionMail"))
                {
                    var emailData = new EmailData
                    {
                        ToAddresses = new() { config["ErrorEmail"] },
                        Subject = "Unhandled Error - Src Api",
                        IsBodyHtml = true,
                        Body = context.BuildExceptionHtmlMessage(ex)
                    };
                    _ = emailService.SendEmailAsync(emailData, emailSetting, logger).ConfigureAwait(false);
                }

                // Write to the response
                string textException = context.BuildExceptionTextMessage(ex);
                logger.LogError("{ExceptionText}", textException);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                if (webHostEnvironment.IsDevelopment())
                    await context.Response.WriteAsync(textException).ConfigureAwait(false);
                else
                    await context.Response.WriteAsync($"{(ex.InnerException == null ? ex.Message : ex.InnerException)}\nPlease contact developer.").ConfigureAwait(false);
            }
        }
    }
}
