using Microsoft.AspNetCore.Identity;
namespace Ts.Domain.Models
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsActive { get; set; }
    }
}
