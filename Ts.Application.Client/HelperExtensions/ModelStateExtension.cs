using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Ts.Application.Client.HelperExtensions
{
    public static class ModelStateExtension
    {
        public static IEnumerable<string> ToErrorMessageList(this ModelStateDictionary dictionary)
        {
            return dictionary.SelectMany(m => m.Value.Errors)
                .Select(m => m.ErrorMessage);
        }

        public static string ToErrorMessageHtmlString(this ModelStateDictionary dictionary)
        {
            var errors = dictionary.SelectMany(m => m.Value.Errors)
                .Select(m => m.ErrorMessage);
            var message = string.Join("<br/>", errors);
            return message;
        }

        public static string ToHtmlBreakString(this List<string> errorMessageList)
        {
            var message = string.Join("<br/>", errorMessageList);
            return message;
        }
    }
}
