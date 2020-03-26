using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit.Utils
{
    public static class Converters
    {
        public static string ConvertHexToString(string hexInput, Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }
        public static string ToHexS(this string String)//help it depend on it's own
        {
            string str = string.Empty;
            string str1 = String;
            for (int i = 0; i < str1.Length; i++)
            {
                byte num = (byte)str1[i];
                str = string.Concat(str, num.ToString("X2"));
            }
            return str;
        }

        public static string ConvertStringToHex(string input, Encoding encoding)
        {
            byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.Append($"{b:X2}");
            }
            return sbBytes.ToString();
        }
        private static string DateToHex(DateTime theDate)
        {
            string isoDate = theDate.ToString("yyyyMMddHHmmss");

            string resultString = string.Empty;

            for (int i = 0; i < isoDate.Length; i++)    // Amended
            {
                int n = char.ConvertToUtf32(isoDate, i);
                string hs = n.ToString("x");
                resultString += hs;

            }
            return resultString;
        }
        public static string CovertHexTime(string hexDate)
        {
            hexDate = DateToHex(DateTime.Now);

            string sDate = string.Empty;
            for (int i = 0; i < hexDate.Length - 1; i += 2)       // Amended
            {
                string ss = hexDate.Substring(i, 2);
                int nn = int.Parse(ss, NumberStyles.AllowHexSpecifier);

                string c = Char.ConvertFromUtf32(nn);
                sDate += c;
            }

            CultureInfo provider = CultureInfo.InvariantCulture;
            CultureInfo[] cultures = { new CultureInfo("fr-FR") };


            return DateTime.ParseExact(sDate, "yyyyMMddHHmmss", provider).ToString();
        }
    }
}
