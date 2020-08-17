using System;

namespace XDevkit
{
    static class Functions
    {
        /// <summary>Verifies if the given string is hex</summary>
        /// <param name="value">The string value to check</param>
        /// <returns>True if its hex and false if it isn't.</returns>
        public static bool IsHex(string value)
        {
            if (value.Length % 2 != 0) return false;
            //^ - Begin the match at the beginning of the line.
            //$ - End the match at the end of the line.
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        /// <summary>Convert Byte Array to String Hex</summary>
        /// <param name="value">The byte array</param>
        /// <returns>Returns an hex string value</returns>
        public static string ToHexString(byte[] value)
        {
            try
            {
                return BitConverter.ToString(value).Replace("-", "");
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public static byte[] StringToByteArray(string text)
        {
            var bytes = new byte[text.Length / 2];

            for (int i = 0; i < text.Length; i += 2)
            {
                bytes[i / 2] = byte.Parse(text[i].ToString() + text[i + 1].ToString(),
                    System.Globalization.NumberStyles.HexNumber);
            }

            return bytes;
        }

        public static uint Convert(string value)
        {
            //using Ternary operator
            return value.Contains("0x") ?
                System.Convert.ToUInt32(value.Substring(2), 16) : System.Convert.ToUInt32(value,16);
        }
        
        public static string ByteArrayToString(byte[] bytes)
        {
            var text = "";

            foreach (byte t in bytes)
            {
                text += String.Format("{0,0:X2}", t);
            }

            return text;
        }

        /// <summary>Converts Unsigned Integer 32 to 4 Byte array</summary>
        /// <param name="value">The value to be converted</param>
        public static Byte[] UInt32ToBytes(UInt32 value)
        {
            var buffer = new Byte[4];
            buffer[3] = (Byte)(value & 0xFF);
            buffer[2] = (Byte)((value >> 8) & 0xFF);
            buffer[1] = (Byte)((value >> 16) & 0xFF);
            buffer[0] = (Byte)((value >> 24) & 0xFF);
            return buffer;
        }

        /// <summary>Converts a Hex string to bytes</summary>
        /// <param name="input">Is the String input</param>
        public static byte[] HexToBytes(String input)
        {
            input = input.Replace(" ", "");
            input = input.Replace("-", "");
            input = input.Replace("0x", "");
            input = input.Replace("0X", "");
            if ((input.Length % 2) != 0)
                input = "0" + input;
            var output = new byte[(input.Length / 2)];

            try
            {
                int index;
                for (index = 0; index < output.Length; index++)
                {
                    output[index] = System.Convert.ToByte(input.Substring((index * 2), 2), 16);
                }
                return output;
            }
            catch
            {
                throw new Exception("Invalid byte Input");
            }
        }

        #region byte to sbyte to byte
        /// <summary>
        /// Converts a unsigned byte to a signed one
        /// </summary>
        /// <param name="b"> byte </param>
        /// <returns>sbyte</returns>
        public static sbyte ByteToSByte(byte b)
        {
            int signed = b - ((b & 0x80) << 1);
            return (sbyte)signed;
        }
        #endregion
        #region (u)int to byte array to (u)int
        /// <summary>
        /// Converts a Byte array to Int16
        /// </summary>
        /// <param name="bytes"> bytes array</param>
        /// <param name="isBigEndian"> if bigendian = true else false; default = false</param>
        /// <returns> returns Int16 </returns>
        public static Int16 BytesToInt16(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }
        /// <summary>
        /// Converts Int16 to a bytes array
        /// </summary>
        /// <param name="value"> Int16</param>
        /// <param name="isBigEndian">if bigendian = true else false; default = false</param>
        /// <returns> returns a bytes arraay</returns>
        public static Byte[] Int16ToBytes(Int16 value, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(value);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        /// <summary>
        /// Converts a Byte array to UInt16
        /// </summary>
        /// <param name="bytes"> bytes array</param>
        /// <param name="isBigEndian"> if bigendian = true else false; default = false</param>
        /// <returns> returns UInt16 </returns>
        public static UInt16 BytesToUInt16(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt16(bytes, 0);
        }
        /// <summary>
        /// Converts UInt16 to a bytes array
        /// </summary>
        /// <param name="value"> UInt16</param>
        /// <param name="isBigEndian">if bigendian = true else false; default = false</param>
        /// <returns> returns a bytes arraay</returns>
        public static Byte[] UInt16ToBytes(UInt16 value, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(value);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }

        /// <summary>
        /// Converts a Byte array to Int32
        /// </summary>
        /// <param name="bytes"> bytes array</param>
        /// <param name="isBigEndian"> if bigendian = true else false; default = false</param>
        /// <returns> returns Int32 </returns>
        public static Int32 BytesToInt32(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// Converts Int32 to a bytes array
        /// </summary>
        /// <param name="value"> Int32</param>
        /// <param name="isBigEndian">if bigendian = true else false; default = false</param>
        /// <returns> returns a bytes arraay</returns>
        public static Byte[] Int32ToBytes(Int32 value, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(value);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        /// <summary>
        /// Converts a Byte array to UInt32
        /// </summary>
        /// <param name="bytes"> bytes array</param>
        /// <param name="isBigEndian"> if bigendian = true else false; default = false</param>
        /// <returns> returns UInt32 </returns>
        public static UInt32 BytesToUInt32(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }
        /// <summary>
        /// Converts UInt32 to a bytes array
        /// </summary>
        /// <param name="value"> UInt32</param>
        /// <param name="isBigEndian">if bigendian = true else false; default = false</param>
        /// <returns> returns a bytes arraay</returns>
        public static Byte[] UInt32ToBytes(UInt32 value, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(value);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }

        /// <summary>
        /// Converts a Byte array to Int64
        /// </summary>
        /// <param name="bytes"> bytes array</param>
        /// <param name="isBigEndian"> if bigendian = true else false; default = false</param>
        /// <returns> returns Int64 </returns>
        public static Int64 BytesToInt64(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
        /// <summary>
        /// Converts Int64 to a bytes array
        /// </summary>
        /// <param name="value"> Int64</param>
        /// <param name="isBigEndian">if bigendian = true else false; default = false</param>
        /// <returns> returns a bytes arraay</returns>
        public static Byte[] Int64ToBytes(Int64 value, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(value);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        /// <summary>
        /// Converts a Byte array to UInt64
        /// </summary>
        /// <param name="bytes"> bytes array</param>
        /// <param name="isBigEndian"> if bigendian = true else false; default = false</param>
        /// <returns> returns UInt64 </returns>
        public static UInt64 BytesToUInt64(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }
        /// <summary>
        /// Converts UInt64 to a bytes array
        /// </summary>
        /// <param name="value"> UInt64</param>
        /// <param name="isBigEndian">if bigendian = true else false; default = false</param>
        /// <returns> returns a bytes arraay</returns>
        public static Byte[] UInt64ToBytes(UInt64 value, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(value);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        #endregion
        #region float/double to byte array to float/double
        public static Byte[] FloatToByteArray(Single f, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(f);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        public static Single BytesToSingle(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }
        public static Byte[] DoubleToByteArray(Double d, bool isBigEndian = true)
        {
            Byte[] buffer = BitConverter.GetBytes(d);
            if (isBigEndian)
                Array.Reverse(buffer);

            return buffer;
        }
        public static Double BytesToDouble(Byte[] bytes, bool isBigEndian = true)
        {
            if (isBigEndian)
                Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }
        #endregion
    }
}
