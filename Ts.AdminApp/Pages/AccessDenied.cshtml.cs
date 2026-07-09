using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Ts.AdminApp.Pages
{
    [IgnoreAntiforgeryToken]
    public class AccessDeniedModel : PageModel
    {
        [FromQuery]
        public string ReturnUrl { get; set; }

        public IActionResult OnGet()
        {
            ViewData["StatusCode"] = 403;
            ViewData["TagLine"] = "Access Denied";
            ViewData["HeadLine"] = "Access to this resource is denied. Please try relogin.";
            return Page();
        }

        public IActionResult OnPost()
        {
            ViewData["StatusCode"] = 403;
            ViewData["TagLine"] = "Access Denied";
            ViewData["HeadLine"] = "Access to this resource is denied. Please try relogin.";
            return Page();
        }
    }
}
