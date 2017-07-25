using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enginesoft.VtexIntegrationSample
{
    public static class Utils
    {
        public static string GetExecutableFolderPath(System.Reflection.Assembly executingAssembly)
        {
            var codeBase = executingAssembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
            return path;
        }

        public static string RemoveSpecialCharacters(string text, bool removeSpaces = false, bool removeDiacritics = true, bool removePageBreak = false, bool trim = false, string exceptions = "")
        {
            string ret = text;

            if (string.IsNullOrEmpty(ret))
                return ret;

            if (removeDiacritics)
                ret = RemoveDiacritics(ret);

            ret = System.Text.RegularExpressions.Regex.Replace(ret, @"\t", " ");

            if (!removeSpaces)
                ret = System.Text.RegularExpressions.Regex.Replace(ret, string.Format(@"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s{0}]+?", exceptions), string.Empty);
            else
                ret = System.Text.RegularExpressions.Regex.Replace(ret, string.Format(@"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ{0}]+?", exceptions), string.Empty);

            if (removePageBreak)
                RemovePageBreak(ret);

            if (trim)
                ret = ret.Trim();

            // "a - b" = "a  b", isso remove os espaços seguidos
            ret = ret.Replace("  ", " ");

            return ret;
        }

        public static string RemovePageBreak(string text, string separator = " ")
        {
            if (text == null)
                return null;

            text = text.Replace("\r\n", separator);
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\n", separator);
            return text;
        }

        public static string RemoveSpaces(string text)
        {
            if (text == null)
                return null;

            text = text.Replace(" ", string.Empty);
            return text;
        }

        public static string RemoveHtmlTags(string text)
        {
            if (text == null)
                return null;

            text = System.Text.RegularExpressions.Regex.Replace(text, @"<[^>]*>", String.Empty);
            return text;
        }

        public static string GetNumericCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string ret = System.Text.RegularExpressions.Regex.Replace(text, @"[^0-9]+?", string.Empty);

            return ret != null ? ret.Trim() : ret;
        }

        public static string RemoveNonVisibleCharacters(string text)
        {
            if (text == null)
                return null;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] >= 32 && text[i] <= 255 && text[i] != 127 && text[i] != 129 && text[i] != 141 && text[i] != 143 && text[i] != 144 && text[i] != 157)
                    sb.Append(text[i]);
            }

            return sb.ToString();
        }

        public static string MaxLength(string text, int maxLength, string sufixToIncludeWhenTruncated = null)
        {
            if (text == null)
                return null;

            text = text.Trim();

            if (string.IsNullOrEmpty(sufixToIncludeWhenTruncated))
            {
                if (text.Length > maxLength)
                    text = text.Substring(0, maxLength);
            }
            else
            {
                if (text.Length > maxLength)
                    text = text.Substring(0, maxLength - sufixToIncludeWhenTruncated.Length) + sufixToIncludeWhenTruncated;
            }

            return text;
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                    sb.Append(text[i]);
                else
                    sb.Append(s_Diacritics[text[i]]);
            }

            return sb.ToString();
        }
        private static readonly char[] s_Diacritics = GetDiacritics();
        private static char[] GetDiacritics()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++)
                accents[i] = (char)i;

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }
        
        public static string JsonSerialize(object obj, bool ignoreNull = false, bool indented = false, bool camelCase = false)
        {
            if (obj == null)
                return null;

            var settings = new Newtonsoft.Json.JsonSerializerSettings();

            if (indented)
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            if (ignoreNull)
                settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            if (camelCase)
                settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
        }

        public static T JsonDeserialize<T>(string json)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json,
                new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            return obj;
        }

        public static Newtonsoft.Json.Linq.JObject JsonDeserializeToJObject(string json)
        {
            Newtonsoft.Json.Linq.JObject jObject;
            
            try
            {
                jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
            }
            catch (System.Exception ex)
            {
                throw new System.InvalidOperationException(string.Format("Erro na conversão do JSON para JObject -- Exception: {0} -- JSON: {1}", ex.Message, json));
            }

            return jObject;
        }
    }
}
