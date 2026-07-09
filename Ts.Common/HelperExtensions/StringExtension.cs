namespace Ts.Common.HelperExtensions
{
    public static class StringExtension
    {
        public static string GetFirst(this string source, int length)
        {
            return length >= source.Length ? source.ToUpper() : source.Substring(0, length).ToUpper();
        }

        public static string GetLast(this string source, int length)
        {
            return length >= source.Length ? source.ToUpper() : source.Substring(source.Length - length).ToUpper();
        }

        public static string RemoveSpace(this string source)
        {
            return source.Contains(" ") ? source.Replace(" ", "") : source;
        }

        public static string GetUptoFirstSpace(this string source)
        {
            return source.Contains(" ") ? source.Split(" ")[0].ToUpper() : source.ToUpper();
        }

        public static string GetSubstringByString(this string source, string a, string b)
        {
            return source.Substring(source.IndexOf(a, StringComparison.Ordinal) + a.Length, source.IndexOf(b, StringComparison.Ordinal) - source.IndexOf(a, StringComparison.Ordinal) - a.Length);
        }

        public static string ToUpperFirstLetter(this string source)
        {
            return char.ToUpper(source[0]) + source.Substring(1);
        }

        public static string ToUpperFirstLetterAndAfterHyphen(this string source)
        {
            // Split the string by hyphens
            string[] parts = source.Split('-');

            // Capitalize the first letter of each part
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }

            // Join the parts back together with hyphens
            return string.Join("-", parts);
        }

        public static string ToSpaceAfterEveryCapitalLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            var result = new System.Text.StringBuilder();
            foreach (char c in source)
            {
                if (char.IsUpper(c) && result.Length > 0)
                {
                    result.Append(' ');
                }
                result.Append(c);
            }
            return result.ToString();
        }
    }
}
