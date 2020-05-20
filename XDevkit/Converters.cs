using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDevkit
{
	#region Converter
	public static class Converters
	{
		public static byte[] ToWCHAR(this string String)
		{
			return WCHAR(String);
		}
		public static byte[] WCHAR(string String)
		{
			byte[] numArray = new byte[String.Length * 2 + 2];
			int num = 1;
			string str = String;
			for (int i = 0; i < str.Length; i++)
			{
				numArray[num] = (byte)str[i];
				num += 2;
			}
			return numArray;
		}
		internal static ulong ConvertToUInt64(object o)
		{
			if (o is bool)
			{
				if ((bool)o)
				{
					return (ulong)1;
				}
				return (ulong)0;
			}
			if (o is byte)
			{
				return (ulong)((byte)o);
			}
			if (o is short)
			{
				return (ulong)((short)o);
			}
			if (o is int)
			{
				return (ulong)(int)o;
			}
			if (o is long)
			{
				return (ulong)o;
			}
			if (o is ushort)
			{
				return (ushort)o;
			}
			if (o is uint)
			{
				return (uint)o;
			}
			if (o is ulong)
			{
				return (ulong)o;
			}
			if (o is float)
			{
				return (ulong)BitConverter.DoubleToInt64Bits((double)((float)o));
			}
			if (!(o is double))
			{
				return 0;
			}
			return (ulong)BitConverter.DoubleToInt64Bits((double)o);
		}

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
		public static byte[] IntArrayToByte(int[] iArray)
		{
			byte[] bytes = new byte[iArray.Length * 4];
			int num = 0;
			int num1 = 0;
			while (num < iArray.Length)
			{
				for (int i = 0; i < 4; i++)
				{
					bytes[num1 + i] = BitConverter.GetBytes(iArray[num])[i];
				}
				num++;
				num1 += 4;
			}
			return bytes;
		}
		public static string ToHexString(this string String)//help it depend on it's own
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
		public static int UIntToInt(uint Value) =>
	BitConverter.ToInt32(BitConverter.GetBytes(Value), 0);
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
	#endregion

}
