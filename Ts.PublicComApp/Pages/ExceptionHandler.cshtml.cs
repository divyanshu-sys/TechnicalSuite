using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Ts.PublicComApp.Pages
{
    public class ExceptionHandlerModel : PageModel
    {
        private readonly ILogger<ExceptionHandlerModel> logger;

        public ExceptionHandlerModel(ILogger<ExceptionHandlerModel> logger)
        {
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string exception = HttpContext.BuildExceptionTextMessage(exceptionDetails.Error);
            logger.LogError("Unhandled Exception: {Exception}", exception);
            ViewData["StatusCode"] = 500;
            ViewData["TagLine"] = "Internal Server Error";
            HttpContext.Response.ContentType = "text/html";
            return Page();
        }

        public IActionResult OnPost()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string exception = HttpContext.BuildExceptionTextMessage(exceptionDetails.Error);
            logger.LogError("Unhandled Exception: {Exception}", exception);
            ViewData["StatusCode"] = 500;
            ViewData["TagLine"] = "Internal Server Error";
            HttpContext.Response.ContentType = "text/html";
            return Page();
        }
    }
}
