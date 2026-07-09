using System.ComponentModel.DataAnnotations;
namespace Ts.Application.Enums
{
    public enum GenderEnum
    {
        [Display(Name = "Not to say")]
        NotToSay = 1,

        [Display(Name = "Male")]
        Male,

        [Display(Name = "Female")]
        Female
    }
}
