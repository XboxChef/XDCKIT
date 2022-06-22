

//Do Not Delete This Comment... 
//Made By Serenity on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

#region Misc
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace XDCKIT
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XboxExtention
    {
        private static HashSet<Type> ValidReturnTypes { get; set; }
        public static void CallVoid(string module, int ordinal, params object[] Arguments)
        {
            uint Void = 0;
            CallArgs(true, Void, typeof(void), module, ordinal, 0U, 0U, Arguments);
        }
        public static void CallVoid(uint Address, params object[] Arguments)
        { CallArgs(true, 0, typeof(void), null, 0, Address, 0, Arguments); }

        public static void CallVoid(ThreadType Type, string module, int ordinal, params object[] Arguments)
        { CallArgs(Type == ThreadType.System, 0, typeof(void), module, ordinal, 0, 0, Arguments); }

        public static int find(this string String, string _Ptr)
        {
            if (_Ptr.Length == 0 || String.Length == 0)
            {
                return -1;
            }
            for (int i = 0; i < String.Length; i++)
            {
                if (String[i] == _Ptr[0])
                {
                    bool flag = true;
                    for (int j = 0; j < _Ptr.Length; j++)
                    {
                        if (String[i + j] != _Ptr[j])
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private static object CallArgs(bool SystemThread, uint Type, Type t, string module, int ordinal, uint Address, uint ArraySize, params object[] Arguments)
        {
            uint Void = 0;
            uint Int = 1;
            uint Float = 3;
            uint ByteArray = 7;
            uint Uint64 = 8;
            uint Uint64Array = 9;
            uint Version = 2;
            XboxConsole.ConnectTimeout = XboxConsole.ConversationTimeout = (int)4000000U; //ConversationTimeout
            object[] objArray1 = new object[13];
            objArray1[0] = "consolefeatures ver=";
            objArray1[1] = Version;
            objArray1[2] = " type=";
            objArray1[3] = Type;
            objArray1[4] = SystemThread ? " system" : "";
            object[] objArray2 = objArray1;
            string str1;
            if (module == null)
                str1 = "";
            else
                str1 = " module=\"" + module + "\" ord=" + ordinal;
            objArray2[5] = str1;
            objArray1[6] = " as=";
            objArray1[7] = ArraySize;
            objArray1[8] = " params=\"A\\";
            objArray1[9] = Address.ToString("X");
            objArray1[10] = "\\A\\";
            objArray1[11] = Arguments.Length;
            objArray1[12] = "\\";
            string str2 = string.Concat(objArray1);
            if (Arguments.Length > 37)
                throw new Exception("Can not use more than 37 paramaters in a call");
            foreach (object o in Arguments)
            {
                bool flag1 = false;
                if (o is uint num)
                {

                    str2 = str2 + Int + "\\" + UIntToInt(num) + "\\";
                    flag1 = true;
                }
                if (o is int || o is bool || o is byte)
                {
                    if (o is bool flag)
                        str2 = str2 + Int + "/" + System.Convert.ToInt32(flag) + "\\";
                    else
                        str2 = str2 + Int + "\\" + (o is byte ? System.Convert.ToByte(o).ToString() : System.Convert.ToInt32(o).ToString()) + "\\";
                    flag1 = true;
                }
                else if (o is int[] || o is uint[])
                {
                    byte[] numArray = IntArrayToByte((int[])o);
                    string str3 = str2 + ByteArray.ToString() + "/" + numArray.Length + "\\";
                    for (int index = 0; index < numArray.Length; ++index)
                        str3 += numArray[index].ToString("X2");
                    str2 = str3 + "\\";
                    flag1 = true;
                }
                else if (o is string)
                {
                    string str3 = (string)o;
                    str2 = str2 + ByteArray.ToString() + "/" + str3.Length + "\\" + ((string)o).ToHexString() + "\\";
                    flag1 = true;
                }
                else if (o is double)
                {
                    str2 = str2 + Float.ToString() + "\\" + o.ToString() + "\\";
                    flag1 = true;
                }
                else if (o is float)
                {
                    str2 = str2 + Float.ToString() + "\\" + o.ToString() + "\\";
                    flag1 = true;
                }
                else if (o is float[])
                {
                    float[] numArray = (float[])o;
                    string str3 = str2 + ByteArray.ToString() + "/" + (numArray.Length * 4).ToString() + "\\";
                    for (int index1 = 0; index1 < numArray.Length; ++index1)
                    {
                        byte[] bytes = BitConverter.GetBytes(numArray[index1]);
                        Array.Reverse(bytes);
                        for (int index2 = 0; index2 < 4; ++index2)
                            str3 += bytes[index2].ToString("X2");
                    }
                    str2 = str3 + "\\";
                    flag1 = true;
                }
                else if (o is byte[])
                {
                    byte[] numArray = (byte[])o;
                    string str3 = str2 + ByteArray.ToString() + "/" + numArray.Length + "\\";
                    for (int index = 0; index < numArray.Length; ++index)
                        str3 += numArray[index].ToString("X2");
                    str2 = str3 + "\\";
                    flag1 = true;
                }
                if (!flag1)
                    str2 = str2 + Uint64.ToString() + "\\" + ConvertToUInt64(o).ToString() + "\\";
            }
            string Command = str2 + "\"";
            string String = XboxConsole.SendTextCommand(Command);
            uint num1;
            for (string _Ptr = "buf_addr="; String.Contains(_Ptr); String = XboxConsole.SendTextCommand("consolefeatures " + _Ptr + "0x" + num1.ToString("X")))
            {
                Thread.Sleep(250);
                num1 = uint.Parse(String.Substring(String.find(_Ptr) + _Ptr.Length), NumberStyles.HexNumber);
            }
            XboxConsole.ConnectTimeout = (int)2000U;
            XboxConsole.ConversationTimeout = (int)5000U;
            switch (Type)
            {
                case 1:
                    uint num2 = uint.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
                    if (t == typeof(uint))
                        return num2;
                    if (t == typeof(int))
                        return UIntToInt(num2);
                    if (t == typeof(short))
                        return short.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
                    if (t == typeof(ushort))
                        return ushort.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
                    break;
                case 2:
                    string str4 = String.Substring(String.find(" ") + 1);
                    if (t == typeof(string))
                        return str4;
                    if (t == typeof(char[]))
                        return str4.ToCharArray();
                    break;
                case 3:
                    if (t == typeof(double))
                        return double.Parse(String.Substring(String.find(" ") + 1));
                    if (t == typeof(float))
                        return float.Parse(String.Substring(String.find(" ") + 1));
                    break;
                case 4:
                    byte num3 = byte.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
                    if (t == typeof(byte))
                        return num3;
                    if (t == typeof(char))
                        return (char)num3;
                    break;
                case 8:
                    if (t == typeof(long))
                        return long.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
                    if (t == typeof(ulong))
                        return ulong.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
                    break;
            }
            switch (Type)
            {
                case 5:
                    string str5 = String.Substring(String.find(" ") + 1);
                    int index3 = 0;
                    string s1 = "";
                    uint[] numArray1 = new uint[8];
                    foreach (char ch in str5)
                    {
                        switch (ch)
                        {
                            case ',':
                            case ';':
                                numArray1[index3] = uint.Parse(s1, NumberStyles.HexNumber);
                                ++index3;
                                s1 = "";
                                break;
                            default:
                                s1 += ch.ToString();
                                break;
                        }
                        if (ch == ';')
                            break;
                    }
                    return numArray1;
                case 6:
                    string str6 = String.Substring(String.find(" ") + 1);
                    int index4 = 0;
                    string s2 = "";
                    float[] numArray2 = new float[ArraySize];
                    foreach (char ch in str6)
                    {
                        switch (ch)
                        {
                            case ',':
                            case ';':
                                numArray2[index4] = float.Parse(s2);
                                ++index4;
                                s2 = "";
                                break;
                            default:
                                s2 += ch.ToString();
                                break;
                        }
                        if (ch == ';')
                            break;
                    }
                    return numArray2;
                case 7:
                    string str7 = String.Substring(String.find(" ") + 1);
                    int index5 = 0;
                    string s3 = "";
                    byte[] numArray3 = new byte[ArraySize];
                    foreach (char ch in str7)
                    {
                        switch (ch)
                        {
                            case ',':
                            case ';':
                                numArray3[index5] = byte.Parse(s3);
                                ++index5;
                                s3 = "";
                                break;
                            default:
                                s3 += ch.ToString();
                                break;
                        }
                        if (ch == ';')
                            break;
                    }
                    return numArray3;
                default:
                    if ((int)Type == (int)Uint64Array)
                    {
                        string str3 = String.Substring(String.find(" ") + 1);
                        int index1 = 0;
                        string s4 = "";
                        ulong[] numArray4 = new ulong[ArraySize];
                        foreach (char ch in str3)
                        {
                            switch (ch)
                            {
                                case ',':
                                case ';':
                                    numArray4[index1] = ulong.Parse(s4);
                                    ++index1;
                                    s4 = "";
                                    break;
                                default:
                                    s4 += ch.ToString();
                                    break;
                            }
                            if (ch == ';')
                                break;
                        }
                        if (t == typeof(ulong))
                            return numArray4;
                        if (t == typeof(long))
                        {
                            long[] numArray5 = new long[ArraySize];
                            for (int index2 = 0; index2 < ArraySize; ++index2)
                                numArray5[index2] = BitConverter.ToInt64(BitConverter.GetBytes(numArray4[index2]), 0);
                            return numArray5;
                        }
                    }
                    return (int)Type == (int)Void ? 0 : ulong.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
            }
        }
        public static T Call<T>(uint Address, params object[] Arguments) where T : struct => (T)CallArgs(true, TypeToType<T>(false), typeof(T), null, 0, Address, 0U, Arguments);
        public static T Call<T>(string module, int ordinal, params object[] Arguments) where T : struct
        {
            return (T)CallArgs(true, TypeToType<T>(false), typeof(T), module, ordinal, 0U, 0U, Arguments);
        }
        private static uint TypeToType<T>(bool Array) where T : struct
        {
            uint Int = 1;
            uint String = 2;
            uint Float = 3;
            uint Byte = 4;
            uint IntArray = 5;
            uint FloatArray = 6;
            uint ByteArray = 7;
            uint Uint64 = 8;
            uint Uint64Array = 9;
            Type type = typeof(T);
            if (type == typeof(int) || type == typeof(uint) || (type == typeof(short) || type == typeof(ushort)))
                return Array ? IntArray : Int;
            if (type == typeof(string) || type == typeof(char[]))
                return String;
            return type == typeof(float) || type == typeof(double) ? (Array ? FloatArray : Float) : (type == typeof(byte) || type == typeof(char) ? (Array ? ByteArray : Byte) : ((type == typeof(ulong) || type == typeof(long)) && Array ? Uint64Array : Uint64));
        }

        public static void Push(this byte[] InArray, out byte[] OutArray, byte Value)
        {
            OutArray = new byte[InArray.Length + 1];
            InArray.CopyTo(OutArray, 0);
            OutArray[InArray.Length] = Value;
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
        private static byte[] getData(long[] argument)
        {
            byte[] numArray = new byte[argument.Length * 8];
            int index = 0;
            foreach (long num in argument)
            {
                byte[] bytes = BitConverter.GetBytes(num);
                Array.Reverse(bytes);
                bytes.CopyTo(numArray, index);
                index += 8;
            }
            return numArray;
        }
        private static float[] toFloatArray(double[] arr)
        {
            if (arr == null)
                return null;
            int length = arr.Length;
            float[] numArray = new float[length];
            for (int index = 0; index < length; ++index)
                numArray[index] = (float)arr[index];
            return numArray;
        }

        internal static ulong ConvertToUInt64(object o)
        {
            if (o is bool)
            {
                if ((bool)o)
                {
                    return 1;
                }
                return 0;
            }
            if (o is byte)
            {
                return (byte)o;
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
                return (ulong)BitConverter.DoubleToInt64Bits((float)o);
            }
            if (!(o is double))
            {
                return 0;
            }
            return (ulong)BitConverter.DoubleToInt64Bits((double)o);
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            var text = string.Empty;

            foreach (byte t in bytes)
            {
                text += String.Format("{0,0:X2}", t);
            }

            return text;
        }
        public static string BytesToHexString(byte[] data)
        {
            string str = string.Empty;
            int index = 0;
            while (index < data.Length)
            {
                string str2 = data[index].ToString("X");
                while (true)
                {
                    if (str2.Length == 2)
                    {
                        str = str + str2;
                        index++;
                        break;
                    }
                    str2 = "0" + str2;
                }
            }
            return str;
        }

        public static string BytesToString(byte[] data) =>
            RemoveWhiteSpacingFromString(Encoding.ASCII.GetString(data));
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

        public static int CalculatePadding(int integer, int interval) =>
            (interval - (integer % interval));

        public static uint Convert(string value)
        {
            if (value.Contains("0x"))
                return System.Convert.ToUInt32(value.Substring(2), 16);
            return System.Convert.ToUInt32(value, 16);
        }

        public static string ConvertHexToString(string hexInput, Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = System.Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }

        public static int ConvertSigned(string value)
        {
            if (value.Contains("0x"))
                return System.Convert.ToInt32(value.Substring(2), 16);
            return System.Convert.ToInt32(value, 16);
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

        public static void DeleteBytes(string filePath, int startOffset, int size)
        {
            FileStream input = new FileStream(filePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(input);
            Stream baseStream = reader.BaseStream;
            baseStream.Position += size;
            reader.Close();
            input.Close();
            input = new FileStream(filePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(input);
            writer.Write(reader.ReadBytes(startOffset));
            writer.Write(reader.ReadBytes(((int)reader.BaseStream.Length) - ((int)reader.BaseStream.Position)));
            input.Close();
            writer.Close();
        }

        public static byte[] GetBytesFromStream(BinaryReader br) =>
            GetBytesFromStream(br, 0, (int)br.BaseStream.Length);

        public static byte[] GetBytesFromStream(BinaryReader br, int offset, int size)
        {
            br.BaseStream.Position = offset;
            return br.ReadBytes(size);
        }
        public static byte[] HexStringToBytes(string hexString)
        {
            List<byte> list = new List<byte>();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                if (hexString.Length > (i + 1))
                {
                    list.Add(byte.Parse(hexString[i] + hexString[i + 1].ToString(), NumberStyles.HexNumber));
                }
            }
            return list.ToArray();
        }

        /// <summary>Converts a Hex string to bytes</summary>
        /// <param name="input">Is the String input</param>
        public static byte[] HexToBytes(String input)
        {
            input = input.Replace(" ", string.Empty);
            input = input.Replace("-", string.Empty);
            input = input.Replace("0x", string.Empty);
            input = input.Replace("0X", string.Empty);
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


        public static void InsertBytes(string filePath, int startOffset, int size)
        {
            FileStream input = new FileStream(filePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(input)
            {
                BaseStream = { Position = startOffset }
            };
            reader.Close();
            input.Close();
            input = new FileStream(filePath, FileMode.Open);
            BinaryWriter writer = new BinaryWriter(input)
            {
                BaseStream = { Position = startOffset }
            };
            writer.Write(new byte[size]);
            writer.Write(reader.ReadBytes(((int)reader.BaseStream.Length) - ((int)reader.BaseStream.Position)));
            input.Close();
            writer.Close();
        }

        public static void InsertBytes(string filePath, int startOffset, byte[] data)
        {
            FileStream input = new FileStream(filePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(input)
            {
                BaseStream = { Position = startOffset }
            };
            reader.Close();
            input.Close();
            input = new FileStream(filePath, FileMode.Open);
            BinaryWriter writer = new BinaryWriter(input)
            {
                BaseStream = { Position = startOffset }
            };
            writer.Write(data);
            writer.Write(reader.ReadBytes(((int)reader.BaseStream.Length) - ((int)reader.BaseStream.Position)));
            input.Close();
            writer.Close();
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
        public static bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
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

        public static string RemoveWhiteSpacingFromString(string str) =>
            str.Replace("\0", string.Empty);
        /// <summary>
        /// Turn hex string to byte array
        /// </summary>
        /// <param name="text">The hex string</param>
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = System.Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static byte[] StringToBytes(string data) =>
            Encoding.ASCII.GetBytes(data);

        public static byte[] StringToUnicodeBytes(string text)
        {
            byte[] buffer = StringToBytes(text);
            byte[] buffer2 = new byte[buffer.Length * 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer2[i * 2] = buffer[i];
            }
            return buffer2;
        }

        public static byte[] StringToUnicodeBytes(string text, int Length)
        {
            byte[] buffer = StringToBytes(text);
            byte[] buffer2 = new byte[Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer2[i * 2] = buffer[i];
            }
            return buffer2;
        }

        public static void SwapBytes(ref byte[] data)
        {
            Array.Reverse(data);
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

        /// <summary>Convert Byte Array to String Hex</summary>
        /// <param name="value">The byte array</param>
        /// <returns>Returns an hex string value</returns>
        public static string ToHexString(byte[] value)
        {
            try
            {
                return BitConverter.ToString(value).Replace("-", string.Empty);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        public static byte[] ToWCHAR(this string String)
        {
            return WCHAR(String);
        }

        /// <summary>Converts Unsigned Integer 32 to 4 Byte array</summary>
        /// <param name="value">The value to be converted</param>
        public static Byte[] UInt32ToBytes(UInt32 value)
        {
            var buffer = new Byte[4];
            buffer[3] = (byte)(value & 0xFF);
            buffer[2] = (Byte)((value >> 8) & 0xFF);
            buffer[1] = (Byte)((value >> 16) & 0xFF);
            buffer[0] = (Byte)((value >> 24) & 0xFF);
            return buffer;
        }
        public static int UIntToInt(uint Value) =>
    BitConverter.ToInt32(BitConverter.GetBytes(Value), 0);

        public static byte[] ValueToBytes(byte data) =>
            new byte[] { data };

        //public static byte[] ValueToBytes(short data)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
        //    nio.Open();
        //    nio.Out.Write(data);
        //    nio.In.BaseStream.Position = 0L;
        //    byte[] buffer = nio.In.ReadBytes((int)nio.In.BaseStream.Length);
        //    nio.Close();
        //    stream.Close();
        //    return buffer;
        //}

        //public static byte[] ValueToBytes(int data)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
        //    nio.Open();
        //    nio.Out.Write(data);
        //    nio.In.BaseStream.Position = 0L;
        //    byte[] buffer = nio.In.ReadBytes((int)nio.In.BaseStream.Length);
        //    nio.Close();
        //    stream.Close();
        //    return buffer;
        //}

        //public static byte[] ValueToBytes(float data)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
        //    nio.Open();
        //    nio.Out.Write(data);
        //    nio.In.BaseStream.Position = 0L;
        //    byte[] buffer = nio.In.ReadBytes((int)nio.In.BaseStream.Length);
        //    nio.Close();
        //    stream.Close();
        //    return buffer;
        //}

        //public static byte[] ValueToBytes(ushort data)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
        //    nio.Open();
        //    nio.Out.Write(data);
        //    nio.In.BaseStream.Position = 0L;
        //    byte[] buffer = nio.In.ReadBytes((int)nio.In.BaseStream.Length);
        //    nio.Close();
        //    stream.Close();
        //    return buffer;
        //}

        //public static byte[] ValueToBytes(uint data)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    EndianIO nio = new EndianIO(stream, EndianType.BigEndian);
        //    nio.Open();
        //    nio.Out.Write(data);
        //    nio.In.BaseStream.Position = 0L;
        //    byte[] buffer = nio.In.ReadBytes((int)nio.In.BaseStream.Length);
        //    nio.Close();
        //    stream.Close();
        //    return buffer;
        //}

        public static byte[] ValueToBytes(string data, bool unicode) =>
            (!unicode ? StringToBytes(data) : StringToUnicodeBytes(data));
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
        public static byte[] WideChar(string text)
        {
            byte[] numArray = new byte[text.Length * 2 + 2];
            int index = 1;
            numArray[0] = 0;
            foreach (char ch in text)
            {
                numArray[index] = System.Convert.ToByte(ch);
                index += 2;
            }
            return numArray;
        }

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
#endregion
/// <summary>
/// Module information.
/// </summary>
public class ModuleInfo
{
    /// <summary>
    /// Name of the module that was loaded.
    /// </summary>
    public string Name;

    /// <summary>
    /// Address that the module was loaded to.
    /// </summary>
    public uint BaseAddress;

    /// <summary>
    /// Size of the module.
    /// </summary>
    public uint Size;

    /// <summary>
    /// Time stamp of the module.
    /// </summary>
    public DateTime TimeStamp;

    /// <summary>
    /// Checksum of the module.
    /// </summary>
    public uint Checksum;

    /// <summary>
    /// Sections contained within the module.
    /// </summary>
    public System.Collections.Generic.List<ModuleSection> Sections;

    public override string ToString()
    {
        return
            "{ Name: " + Name +
            " BaseAddress: " + BaseAddress +
            " Size: " + Size +
            " TimeStamp: " + TimeStamp.ToString() +
            " Checksum: " + Checksum +
            " }";
    }
};
/// <summary>
/// Module section information.
/// </summary>
public class ModuleSection
{
    public string Name;
    public uint Base;
    public uint Size;
    public uint Index;
    public uint Flags;

    public override string ToString()
    {
        return
            "{ Name: " + Name +
            " Base: " + Base +
            " Size: " + Size +
            " Index: " + Index +
            " Flags: " + Flags +
            " }";
    }
};