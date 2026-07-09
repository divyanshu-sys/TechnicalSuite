namespace Ts.Common.Constant.AppConstants
{
    public class RegxConstant
    {
        public const string Password = @"^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!#\$%&'\(\)\*\+,-\.\/:;<=>\?@[\]\^_`\{\|}~])[a-zA-Z0-9!#\$%&'\(\)\*\+,-\.\/:;<=>\?@[\]\^_`\{\|}~]{0,}$";
        public const string Email = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        public const string LinkText = @"^(?=.{0,}$)[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$";
        public const string UrlLink = @"^(?=.{0,}$)[a-z0-9]+(?:[_-][a-z0-9]+)*$";
        public const string Keyword = @"^[a-zA-Z0-9]+( [a-zA-Z0-9]+)*(,[ ]?[a-zA-Z0-9]+( [a-zA-Z0-9]+)*)?$";
    }
}
