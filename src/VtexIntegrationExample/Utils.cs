using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        //Cada item tem suas formas de entrega (PAC, SEDEX, ...) com valor e prazo de entrega, esse método consolida para cada tipo de entrega o preço e data para entregar todos os itens da lista
        public static List<Models.ShippingInformation> ConsolidateShippingInformation(List<Models.ItemPrice> itemPrices)
        {
            var listRet = new List<Models.ShippingInformation>();

            for (int i = 0; i < itemPrices.Count; i++)
            {
                var itemPrice = itemPrices[i];

                foreach (var shipping in itemPrice.ShippingInformations)
                {
                    var shippingTime = new Models.ShippingTime(shipping.ShippingTime.Days, shipping.ShippingTime.IsBusinessDays);

                    if (itemPrices.Where(a => a.ShippingInformations.Any(x => x.ShippingTypeID == shipping.ShippingTypeID)).Count() < itemPrices.Count)
                    {
                        //Esse tipo de frete não aparece em todos os itens
                        continue;
                    }

                    if (i == 0)
                    {
                        listRet.Add(new Models.ShippingInformation(shipping.ShippingTypeID, shipping.Name, shipping.Price, shippingTime));
                    }
                    else
                    {
                        var itemRet = listRet.FirstOrDefault(a => a.ShippingTypeID == shipping.ShippingTypeID);

                        //soma o preço e obtém a maior data para a entrega
                        itemRet.IncrementShippingTime(shipping.Price, shippingTime);
                    }
                }
            }

            return listRet;
        }

        public static Models.PaymentTypesEnum GetPaymentConditionTypeID(string implementation)
        {
            if (string.IsNullOrEmpty(implementation))
                throw new ArgumentNullException(nameof(implementation));

            if (implementation.Equals("Vtex.PaymentGateway.CreditCard.Elo", StringComparison.CurrentCultureIgnoreCase))
                return Models.PaymentTypesEnum.Elo;

            if (implementation.Equals("Vtex.PaymentGateway.CreditCard.Amex", StringComparison.CurrentCultureIgnoreCase))
                return Models.PaymentTypesEnum.Amex;

            if (implementation.Equals("Vtex.PaymentGateway.CreditCard.Mastercard", StringComparison.CurrentCultureIgnoreCase))
                return Models.PaymentTypesEnum.Mastercard;

            if (implementation.Equals("Vtex.PaymentGateway.CreditCard.Visa", StringComparison.CurrentCultureIgnoreCase))
                return Models.PaymentTypesEnum.Visa;

            if (implementation.Equals("Vtex.PaymentGateway.CreditCard.Diners", StringComparison.CurrentCultureIgnoreCase))
                return Models.PaymentTypesEnum.Diners;

            if (implementation.Equals("Vtex.PaymentGateway.CreditCard.Hipercard", StringComparison.CurrentCultureIgnoreCase))
                return Models.PaymentTypesEnum.Hipercard;

            return Models.PaymentTypesEnum.None;
        }

        public static string HideCreditCardNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            const int QUANTITY_START = 4;
            const int QUANTITY_END = 3;

            string ret = new string('*', text.Length);
            if (text.Length > (QUANTITY_START + QUANTITY_END))
            {
                ret = string.Concat(
                    text.Substring(0, QUANTITY_START),
                    new string('*', text.Length - (QUANTITY_START + QUANTITY_END)),
                    text.Substring(text.Length - (QUANTITY_END + 1), QUANTITY_END)
                );
            }

            return ret;
        }

        public static string HideCreditCardValidationCode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string ret = new string('*', text.Length);
            return ret;
        }
        
        public static string GetConfigDirectory()
        {
            string path = Utils.GetExecutableFolderPath(System.Reflection.Assembly.GetExecutingAssembly());

            if (path.IndexOf("debug", StringComparison.CurrentCultureIgnoreCase) != -1 || path.IndexOf("release", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                var directoryInfo = new System.IO.DirectoryInfo(path);
                path = directoryInfo.Parent.Parent.FullName.ToString();
            }

            path = System.IO.Path.Combine(path, "config");

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            return path;
        }
    }
}
