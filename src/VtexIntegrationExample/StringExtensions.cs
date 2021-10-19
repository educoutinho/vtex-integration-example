namespace Enginesoft.VtexIntegrationSample.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string param)
        {
            return Utils.RemoveDiacritics(param);
        }

        public static string RemoveSpecialCharacters(this string param, bool removeSpaces = false, bool removeDiacritics = true, bool removePageBreak = false, bool trim = false, string exceptions = "")
        {
            return Utils.RemoveSpecialCharacters(param, removeSpaces, removeDiacritics, removePageBreak, trim, exceptions);
        }

        public static string GetNumericCharacters(this string param)
        {
            return Utils.GetNumericCharacters(param);
        }

        public static string ReplaceRegex(this string param, string regexPattern, string replacement)
        {
            if (string.IsNullOrEmpty(param))
                return param;

            return new System.Text.RegularExpressions.Regex(regexPattern).Replace(param, replacement);
        }

        public static string RemovePageBreak(this string param, string separator = " ")
        {
            return Utils.RemovePageBreak(param, separator);
        }

        public static string RemoveNonVisibleCharacters(this string param)
        {
            return Utils.RemoveNonVisibleCharacters(param);
        }

        public static string RemoveSpaces(this string param)
        {
            return Utils.RemoveSpaces(param);
        }

        public static string RemoveHtmlTags(this string param)
        {
            return Utils.RemoveHtmlTags(param);
        }
    }
}
