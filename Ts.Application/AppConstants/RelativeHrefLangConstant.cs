namespace Ts.Application.AppConstants
{
    public static class RelativeHrefLangConstant
    {
        public static Dictionary<int, string> GetHrefLangs()
        {
            var hrefLangs = new Dictionary<int, string>
            {
                [1] = "en", // English
                [2] = "en-IN", // English (India)
                [3] = "hi-IN" // Hindi (India)
            };
            return hrefLangs;
        }
    }
}
