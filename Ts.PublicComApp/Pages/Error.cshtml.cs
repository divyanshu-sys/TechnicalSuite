using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
namespace Ts.PublicComApp.Pages
{
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            this.logger = logger;
        }

        [FromRoute]
        public int Code { get; set; }

        public IActionResult OnGet()
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            logger.LogWarning("StatusCode: {StatusCode}, {StatusCodeName}. Path: {OriginalPath} | QueryStrings: {OriginalQueryString}",
                Code, (HttpStatusCode)Code, statusCodeResult.OriginalPath, statusCodeResult.OriginalQueryString);
            ViewData["StatusCode"] = Code;
            switch (Code)
            {
                case 404:
                    ViewData["TagLine"] = "Page Not Found !";
                    ViewData["HeadLine"] = "The page you were looking for could not be found.";
                    break;
                default:
                    ViewData["TagLine"] = "This page isn't available";
                    ViewData["HeadLine"] = "The link you followed may be broken, or the page may have been removed.";
                    break;
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            logger.LogWarning(
                "StatusCode: {StatusCode}, {StatusCodeName}. Path: {OriginalPath} | QueryStrings: {OriginalQueryString}",
                Code,
                (HttpStatusCode)Code,
                statusCodeResult.OriginalPath,
                statusCodeResult.OriginalQueryString
            );
            ViewData["StatusCode"] = Code;
            switch (Code)
            {
                case 404:
                    ViewData["TagLine"] = "Page Not Found !";
                    ViewData["HeadLine"] = "The page you were looking for could not be found.";
                    break;
                default:
                    ViewData["TagLine"] = "This page isn't available";
                    ViewData["HeadLine"] = "The link you followed may be broken, or the page may have been removed.";
                    break;
            }
            return Page();
        }
    }
}
