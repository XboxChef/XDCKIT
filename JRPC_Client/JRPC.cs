using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using XDevkit;

namespace JRPC_Client
{
    public static class JRPC
    {

        private readonly static uint Byte;

        private readonly static uint ByteArray;

        private static uint connectionId { get; set; }

        private readonly static uint Float;

        private readonly static uint FloatArray;

        private readonly static uint IntArray;

        private static Dictionary<Type, int> StructPrimitiveSizeMap;

        private readonly static uint Uint64;

        private readonly static uint Uint64Array;

        private static HashSet<Type> ValidReturnTypes;

        private static Dictionary<Type, int> ValueTypeSizeMap;
        private readonly static uint Void;

        public readonly static uint Int;

        public readonly static uint JRPCVersion;

        public readonly static uint String;

        static JRPC()
        {
            Void = 0;
            Int = 1;
            String = 2;
            Float = 3;
            Byte = 4;
            IntArray = 5;
            FloatArray = 6;
            ByteArray = 7;
            Uint64 = 8;
            Uint64Array = 9;
            JRPCVersion = 2;
            Dictionary<Type, int> types = new Dictionary<Type, int>()
            {
                { typeof(bool), 4 },
                { typeof(byte), 1 },
                { typeof(short), 2 },
                { typeof(int), 4 },
                { typeof(long), 8 },
                { typeof(ushort), 2 },
                { typeof(uint), 4 },
                { typeof(ulong), 8 },
                { typeof(float), 4 },
                { typeof(double), 8 }
            };
            ValueTypeSizeMap = types;
            StructPrimitiveSizeMap = new Dictionary<Type, int>();
            HashSet<Type> types1 = new HashSet<Type>();
            types1.Add(typeof(void));
            types1.Add(typeof(bool));
            types1.Add(typeof(byte));
            types1.Add(typeof(short));
            types1.Add(typeof(int));
            types1.Add(typeof(long));
            types1.Add(typeof(ushort));
            types1.Add(typeof(uint));
            types1.Add(typeof(ulong));
            types1.Add(typeof(float));
            types1.Add(typeof(double));
            types1.Add(typeof(string));
            types1.Add(typeof(bool[]));
            types1.Add(typeof(byte[]));
            types1.Add(typeof(short[]));
            types1.Add(typeof(int[]));
            types1.Add(typeof(long[]));
            types1.Add(typeof(ushort[]));
            types1.Add(typeof(uint[]));
            types1.Add(typeof(ulong[]));
            types1.Add(typeof(float[]));
            types1.Add(typeof(double[]));
            types1.Add(typeof(string[]));
            ValidReturnTypes = types1;
        }

        private static T[] ArrayReturn<T>(this IXboxConsole console, uint Address, uint Size)
        {
            if (Size == 0)
            {
                return new T[1];
            }
            Type type = typeof(T);
            object obj = new object();
            if (type == typeof(short))
            {
                obj = console.ReadInt16(Address, Size);
            }
            if (type == typeof(ushort))
            {
                obj = console.ReadUInt16(Address, Size);
            }
            if (type == typeof(int))
            {
                obj = console.ReadInt32(Address, Size);
            }
            if (type == typeof(uint))
            {
                obj = console.ReadUInt32(Address, Size);
            }
            if (type == typeof(long))
            {
                obj = console.ReadInt64(Address, Size);
            }
            if (type == typeof(ulong))
            {
                obj = console.ReadUInt64(Address, Size);
            }
            if (type == typeof(float))
            {
                obj = console.ReadFloat(Address, Size);
            }
            if (type == typeof(byte))
            {
                obj = console.GetMemory(Address, Size);
            }
            return (T[])obj;
        }

        /// <summary>
        /// EDITED:  Do Not Use
        /// </summary>
        private static object CallArgs(IXboxConsole console, bool SystemThread, uint Type, System.Type t, string module, int ordinal, uint Address, uint ArraySize, params object[] Arguments)
        {

            {
                object[] name;
                int i;
                object obj;
                string str;
                string[] strArrays;
                string str1;
                object obj1;
                if (!IsValidReturnType(t))
                {
                    name = new object[] { "Invalid type ", t.Name, Environment.NewLine, "JRPC only supports: bool, byte, short, int, long, ushort, uint, ulong, float, double" };
                    throw new Exception(string.Concat(name));
                }

                console.ConversationTimeout = 4000000;
                console.ConnectTimeout = 4000000;
                object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=", Type, null, null, null, null, null, null, null, null, null };
                jRPCVersion[4] = (SystemThread ? " system" : string.Empty);
                object[] objArray = jRPCVersion;
                if (module != null)
                {
                    object[] objArray1 = new object[] { " module=\"", module, "\" ord=", ordinal };
                    obj1 = string.Concat(objArray1);
                }
                else
                {
                    obj1 = string.Empty;
                }
                objArray[5] = obj1;
                jRPCVersion[6] = " as=";
                jRPCVersion[7] = ArraySize;
                jRPCVersion[8] = " params=\"A\\";
                jRPCVersion[9] = Address.ToString("X");
                jRPCVersion[10] = "\\A\\";
                jRPCVersion[11] = Arguments.Length;
                jRPCVersion[12] = "\\";
                string str2 = string.Concat(jRPCVersion);
                if (Arguments.Length > 37)
                {
                    throw new Exception("Can not use more than 37 paramaters in a call");
                }
                object[] arguments = Arguments;
                for (i = 0; i < arguments.Length; i++)
                {
                    object obj2 = arguments[i];
                    bool flag = false;
                    if (obj2 is uint)
                    {
                        obj = str2;
                        object[] num2 = new object[] { obj, Int, "\\", UIntToInt((uint)obj2), "\\" };
                        str2 = string.Concat(num2);
                        flag = true;
                    }
                    if (obj2 is int || obj2 is bool || obj2 is byte)
                    {
                        if (!(obj2 is bool))
                        {
                            object obj3 = str2;
                            object[] objArray2 = new object[] { obj3, Int, "\\", null, null };
                            objArray2[3] = (obj2 is byte ? Convert.ToByte(obj2).ToString() : Convert.ToInt32(obj2).ToString());
                            objArray2[4] = "\\";
                            str2 = string.Concat(objArray2);
                        }
                        else
                        {
                            object obj4 = str2;
                            object[] num3 = new object[] { obj4, Int, "/", Convert.ToInt32((bool)obj2), "\\" };
                            str2 = string.Concat(num3);
                        }
                        flag = true;
                    }
                    else if (obj2 is int[] || obj2 is uint[])
                    {
                        byte[] numArray = IntArrayToByte((int[])obj2);
                        object obj5 = str2;
                        object[] str3 = new object[] { obj5, ByteArray.ToString(), "/", numArray.Length, "\\" };
                        str2 = string.Concat(str3);
                        for (int j = 0; j < numArray.Length; j++)
                        {
                            str2 = string.Concat(str2, numArray[j].ToString("X2"));
                        }
                        str2 = string.Concat(str2, "\\");
                        flag = true;
                    }
                    else if (obj2 is string)
                    {
                        string str4 = (string)obj2;
                        object obj6 = str2;
                        object[] objArray3 = new object[] { obj6, ByteArray.ToString(), "/", str4.Length, "\\", ((string)obj2).ToHexString(), "\\" };
                        str2 = string.Concat(objArray3);
                        flag = true;
                    }
                    else if (obj2 is double)
                    {
                        double num4 = (double)obj2;
                        str = str2;
                        strArrays = new string[] { str, Float.ToString(), "\\", num4.ToString(), "\\" };
                        str2 = string.Concat(strArrays);
                        flag = true;
                    }
                    else if (obj2 is float)
                    {
                        float single = (float)obj2;
                        str = str2;
                        strArrays = new string[] { str, Float.ToString(), "\\", single.ToString(), "\\" };
                        str2 = string.Concat(strArrays);
                        flag = true;
                    }
                    else if (obj2 is float[])
                    {
                        float[] singleArray = (float[])obj2;
                        str = str2;
                        strArrays = new string[] { str, ByteArray.ToString(), "/", null, null };
                        int length = singleArray.Length * 4;
                        strArrays[3] = length.ToString();
                        strArrays[4] = "\\";
                        str2 = string.Concat(strArrays);
                        for (int k = 0; k < singleArray.Length; k++)
                        {
                            byte[] bytes = BitConverter.GetBytes(singleArray[k]);
                            Array.Reverse(bytes);
                            for (int l = 0; l < 4; l++)
                            {
                                str2 = string.Concat(str2, bytes[l].ToString("X2"));
                            }
                        }
                        str2 = string.Concat(str2, "\\");
                        flag = true;
                    }
                    else if (obj2 is byte[])
                    {
                        byte[] numArray1 = (byte[])obj2;
                        obj = str2;
                        name = new object[] { obj, ByteArray.ToString(), "/", numArray1.Length, "\\" };
                        str2 = string.Concat(name);
                        for (int m = 0; m < numArray1.Length; m++)
                        {
                            str2 = string.Concat(str2, numArray1[m].ToString("X2"));
                        }
                        str2 = string.Concat(str2, "\\");
                        flag = true;
                    }
                    if (!flag)
                    {
                        str = str2;
                        strArrays = new string[] { str, Uint64.ToString(), "\\", null, null };
                        ulong num5 = ConvertToUInt64(obj2);
                        strArrays[3] = num5.ToString();
                        strArrays[4] = "\\";
                        str2 = string.Concat(strArrays);
                    }
                }
                str2 = string.Concat(str2, "\"");
                string str5 = SendCommand(console, str2);
                string str6 = "buf_addr=";
                while (str5.Contains(str6))
                {
                    Thread.Sleep(250);
                    uint num6 = uint.Parse(str5.Substring(str5.find(str6) + str6.Length), NumberStyles.HexNumber);
                    str5 = SendCommand(console, string.Concat("consolefeatures ", str6, "0x", num6.ToString("X")));
                }
                console.ConversationTimeout = 2000;
                console.ConnectTimeout = 5000;
                switch (Type)
                {
                    case 1:
                        {
                            uint num7 = uint.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                            if (t == typeof(uint))
                            {
                                return num7;
                            }
                            if (t == typeof(int))
                            {
                                return UIntToInt(num7);
                            }
                            if (t == typeof(short))
                            {
                                return short.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                            }
                            if (t != typeof(ushort))
                            {
                                goto case 7;
                            }
                            return ushort.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                        }
                    case 2:
                        {
                            string str7 = str5.Substring(str5.find(" ") + 1);
                            if (t == typeof(string))
                            {
                                return str7;
                            }
                            if (t != typeof(char[]))
                            {
                                goto case 7;
                            }
                            return str7.ToCharArray();
                        }
                    case 3:
                        {
                            if (t == typeof(double))
                            {
                                return double.Parse(str5.Substring(str5.find(" ") + 1));
                            }
                            if (t != typeof(float))
                            {
                                goto case 7;
                            }
                            return float.Parse(str5.Substring(str5.find(" ") + 1));
                        }
                    case 4:
                        {
                            byte num8 = byte.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                            if (t == typeof(byte))
                            {
                                return num8;
                            }
                            if (t != typeof(char))
                            {
                                goto case 7;
                            }
                            return (char)num8;
                        }
                    case 5:
                    case 6:
                    case 7:
                        {
                            if (Type == 5)
                            {
                                string str8 = str5.Substring(str5.find(" ") + 1);
                                int num9 = 0;
                                string str9 = string.Empty;
                                uint[] numArray2 = new uint[8];
                                str1 = str8;
                                for (i = 0; i < str1.Length; i++)
                                {
                                    char chr = str1[i];
                                    if (chr == ',' || chr == ';')
                                    {
                                        numArray2[num9] = uint.Parse(str9, NumberStyles.HexNumber);
                                        num9++;
                                        str9 = string.Empty;
                                    }
                                    else
                                    {
                                        str9 = string.Concat(str9, chr.ToString());
                                    }
                                    if (chr == ';')
                                    {
                                        break;
                                    }
                                }
                                return numArray2;
                            }
                            if (Type == 6)
                            {
                                string str10 = str5.Substring(str5.find(" ") + 1);
                                int num10 = 0;
                                string str11 = string.Empty;
                                float[] singleArray1 = new float[ArraySize];
                                str1 = str10;
                                for (i = 0; i < str1.Length; i++)
                                {
                                    char chr1 = str1[i];
                                    if (chr1 == ',' || chr1 == ';')
                                    {
                                        singleArray1[num10] = float.Parse(str11);
                                        num10++;
                                        str11 = string.Empty;
                                    }
                                    else
                                    {
                                        str11 = string.Concat(str11, chr1.ToString());
                                    }
                                    if (chr1 == ';')
                                    {
                                        break;
                                    }
                                }
                                return singleArray1;
                            }
                            if (Type == 7)
                            {
                                string str12 = str5.Substring(str5.find(" ") + 1);
                                int num11 = 0;
                                string str13 = string.Empty;
                                byte[] numArray3 = new byte[ArraySize];
                                str1 = str12;
                                for (i = 0; i < str1.Length; i++)
                                {
                                    char chr2 = str1[i];
                                    if (chr2 == ',' || chr2 == ';')
                                    {
                                        numArray3[num11] = byte.Parse(str13);
                                        num11++;
                                        str13 = string.Empty;
                                    }
                                    else
                                    {
                                        str13 = string.Concat(str13, chr2.ToString());
                                    }
                                    if (chr2 == ';')
                                    {
                                        break;
                                    }
                                }
                                return numArray3;
                            }
                            if (Type == Uint64Array)
                            {
                                string str14 = str5.Substring(str5.find(" ") + 1);
                                int num12 = 0;
                                string str15 = string.Empty;
                                ulong[] numArray4 = new ulong[ArraySize];
                                str1 = str14;
                                for (i = 0; i < str1.Length; i++)
                                {
                                    char chr3 = str1[i];
                                    if (chr3 == ',' || chr3 == ';')
                                    {
                                        numArray4[num12] = ulong.Parse(str15);
                                        num12++;
                                        str15 = string.Empty;
                                    }
                                    else
                                    {
                                        str15 = string.Concat(str15, chr3.ToString());
                                    }
                                    if (chr3 == ';')
                                    {
                                        break;
                                    }
                                }
                                if (t == typeof(ulong))
                                {
                                    return numArray4;
                                }
                                if (t == typeof(long))
                                {
                                    long[] numArray5 = new long[ArraySize];
                                    for (int n = 0; n < ArraySize; n++)
                                    {
                                        numArray5[n] = BitConverter.ToInt64(BitConverter.GetBytes(numArray4[n]), 0);
                                    }
                                    return numArray5;
                                }
                            }
                            if (Type == Void)
                            {
                                return 0;
                            }
                            return ulong.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                        }
                    case 8:
                        {
                            if (t == typeof(long))
                            {
                                return long.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                            }
                            if (t != typeof(ulong))
                            {
                                goto case 7;
                            }
                            return ulong.Parse(str5.Substring(str5.find(" ") + 1), NumberStyles.HexNumber);
                        }
                    default:
                        {
                            goto case 7;
                        }
                }
            }
        }

        private static byte[] IntArrayToByte(int[] iArray)
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

        private static void ReverseBytes(byte[] buffer, int groupSize)
        {
            if (buffer.Length % groupSize != 0)
            {
                throw new ArgumentException("Group size must be a multiple of the buffer length", "groupSize");
            }
            for (int i = 0; i < buffer.Length; i += groupSize)
            {
                int num = i;
                for (int j = i + groupSize - 1; num < j; j--)
                {
                    byte num1 = buffer[num];
                    buffer[num] = buffer[j];
                    buffer[j] = num1;
                    num++;
                }
            }
        }

        private static string SendCommand(IXboxConsole console, string Command)
        {
            string str = null;
            try
            {
                console.SendTextCommand(Command, out str);
                if (str.Contains("error="))
                {
                    throw new Exception(str.Substring(11));
                }
                if (str.Contains("DEBUG"))
                {
                    throw new Exception("JRPC is not installed on the current console");
                }
            }
            catch (COMException cOMException1)
            {
                COMException cOMException = cOMException1;
                if (cOMException.ErrorCode != UIntToInt(2099642361))
                {
                    throw cOMException;
                }
                throw new Exception("JRPC is not installed on the current console");
            }
            return str;
        }

        private static uint TypeToType<T>(bool Array)
        where T : struct
        {
            Type type = typeof(T);
            if (type == typeof(int) || type == typeof(uint) || type == typeof(short) || type == typeof(ushort))
            {
                if (Array)
                {
                    return IntArray;
                }
                return Int;
            }
            if (type == typeof(string) || type == typeof(char[]))
            {
                return String;
            }
            if (type == typeof(float) || type == typeof(double))
            {
                if (Array)
                {
                    return FloatArray;
                }
                return Float;
            }
            if (type == typeof(byte) || type == typeof(char))
            {
                if (Array)
                {
                    return ByteArray;
                }
                return Byte;
            }
            if (type != typeof(ulong) && type != typeof(long))
            {
                return Uint64;
            }
            if (Array)
            {
                return Uint64Array;
            }
            return Uint64;
        }

        private static int UIntToInt(uint Value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(Value), 0);
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

        internal static bool IsValidReturnType(Type t)
        {
            return ValidReturnTypes.Contains(t);
        }

        internal static bool IsValidStructType(Type t)
        {
            if (t.IsPrimitive)
            {
                return false;
            }
            return t.IsValueType;
        }

        public static T Call<T>(this IXboxConsole console, uint Address, params object[] Arguments)
        where T : struct
        {
            return (T)CallArgs(console, true, TypeToType<T>(false), typeof(T), null, 0, Address, 0, Arguments);
        }

        public static T Call<T>(this IXboxConsole console, string module, int ordinal, params object[] Arguments)
        where T : struct
        {
            return (T)CallArgs(console, true, TypeToType<T>(false), typeof(T), module, ordinal, 0, 0, Arguments);
        }

        public static T Call<T>(this IXboxConsole console, ThreadType Type, uint Address, params object[] Arguments)
        where T : struct
        {
            return (T)CallArgs(console, Type == ThreadType.System, TypeToType<T>(false), typeof(T), null, 0, Address, 0, Arguments);
        }

        public static T Call<T>(this IXboxConsole console, ThreadType Type, string module, int ordinal, params object[] Arguments)
        where T : struct
        {
            return (T)CallArgs(console, Type == ThreadType.System, TypeToType<T>(false), typeof(T), module, ordinal, 0, 0, Arguments);
        }

        public static T[] CallArray<T>(this IXboxConsole console, uint Address, uint ArraySize, params object[] Arguments)
        where T : struct
        {
            if (ArraySize == 0)
            {
                return new T[1];
            }
            return (T[])CallArgs(console, true, TypeToType<T>(true), typeof(T), null, 0, Address, ArraySize, Arguments);
        }

        public static T[] CallArray<T>(this IXboxConsole console, string module, int ordinal, uint ArraySize, params object[] Arguments)
        where T : struct
        {
            if (ArraySize == 0)
            {
                return new T[1];
            }
            return (T[])CallArgs(console, true, TypeToType<T>(true), typeof(T), module, ordinal, 0, ArraySize, Arguments);
        }

        public static T[] CallArray<T>(this IXboxConsole console, ThreadType Type, uint Address, uint ArraySize, params object[] Arguments)
        where T : struct
        {
            if (ArraySize == 0)
            {
                return new T[1];
            }
            return (T[])CallArgs(console, Type == ThreadType.System, TypeToType<T>(true), typeof(T), null, 0, Address, ArraySize, Arguments);
        }

        public static T[] CallArray<T>(this IXboxConsole console, ThreadType Type, string module, int ordinal, uint ArraySize, params object[] Arguments)
        where T : struct
        {
            if (ArraySize == 0)
            {
                return new T[1];
            }
            return (T[])CallArgs(console, Type == ThreadType.System, TypeToType<T>(true), typeof(T), module, ordinal, 0, ArraySize, Arguments);
        }

        public static string CallString(this IXboxConsole console, uint Address, params object[] Arguments)
        {
            return (string)CallArgs(console, true, String, typeof(string), null, 0, Address, 0, Arguments);
        }

        public static string CallString(this IXboxConsole console, string module, int ordinal, params object[] Arguments)
        {
            return (string)CallArgs(console, true, String, typeof(string), module, ordinal, 0, 0, Arguments);
        }

        public static string CallString(this IXboxConsole console, ThreadType Type, uint Address, params object[] Arguments)
        {
            return (string)CallArgs(console, Type == ThreadType.System, String, typeof(string), null, 0, Address, 0, Arguments);
        }

        public static string CallString(this IXboxConsole console, ThreadType Type, string module, int ordinal, params object[] Arguments)
        {
            return (string)CallArgs(console, Type == ThreadType.System, String, typeof(string), module, ordinal, 0, 0, Arguments);
        }

        public static void CallVoid(this IXboxConsole console, uint Address, params object[] Arguments)
        {
            CallArgs(console, true, Void, typeof(void), null, 0, Address, 0, Arguments);
        }

        public static void CallVoid(this IXboxConsole console, string module, int ordinal, params object[] Arguments)
        {
            CallArgs(console, true, Void, typeof(void), module, ordinal, 0, 0, Arguments);
        }

        public static void CallVoid(this IXboxConsole console, ThreadType Type, uint Address, params object[] Arguments)
        {
            CallArgs(console, Type == ThreadType.System, Void, typeof(void), null, 0, Address, 0, Arguments);
        }

        public static void CallVoid(this IXboxConsole console, ThreadType Type, string module, int ordinal, params object[] Arguments)
        {
            CallArgs(console, Type == ThreadType.System, Void, typeof(void), module, ordinal, 0, 0, Arguments);
        }

        public static bool Connect(this Xbox console, out Xbox Console, string XboxNameOrIP = "default")
        {
            bool flag;
            if (XboxNameOrIP == "default")
            {
                XboxNameOrIP = new XboxManager().DefaultConsole;
            }
            IXboxConsole xboxConsole = new XboxManager().OpenConsole(XboxNameOrIP);
            int num = 0;
            bool flag1 = false;
            while (!flag1)
            {
                try
                {
                    connectionId = 0;
                    flag1 = true;
                    continue;
                }
                catch (COMException cOMException)
                {
                    if (cOMException.ErrorCode != UIntToInt(2099642112))
                    {
                        flag = false;
                    }
                    else if (num < 3)
                    {
                        num++;
                        Thread.Sleep(100);
                        continue;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                Console = console;
                return flag;
            }
            Console = console;
            return true;
        }

        public static string ConsoleType(this IXboxConsole console)
        {
            string str = string.Concat("consolefeatures ver=", JRPCVersion, " type=17 params=\"A\\0\\A\\0\\\"");
            string str1 = SendCommand(console, str);
            return str1.Substring(str1.find(" ") + 1);
        }

        public static void constantMemorySet(this IXboxConsole console, uint Address, uint Value)
        {
            constantMemorySetting(console, Address, Value, false, 0, false, 0);
        }

        public static void constantMemorySet(this IXboxConsole console, uint Address, uint Value, uint TitleID)
        {
            constantMemorySetting(console, Address, Value, false, 0, true, TitleID);
        }

        public static void constantMemorySet(this IXboxConsole console, uint Address, uint Value, uint IfValue, uint TitleID)
        {
            constantMemorySetting(console, Address, Value, true, IfValue, true, TitleID);
        }

        public static void constantMemorySetting(IXboxConsole console, uint Address, uint Value, bool useIfValue, uint IfValue, bool usetitleID, uint TitleID)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=18 params=\"A\\", Address.ToString("X"), "\\A\\5\\", Int, "\\", UIntToInt(Value), "\\", Int, "\\", (useIfValue ? 1 : 0), "\\", Int, "\\", IfValue, "\\", Int, "\\", (usetitleID ? 1 : 0), "\\", Int, "\\", UIntToInt(TitleID), "\\\"" };
            SendCommand(console, string.Concat(jRPCVersion));
        }

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

        public static string GetCPUKey(this IXboxConsole console)
        {
            string str = string.Concat("consolefeatures ver=", JRPCVersion, " type=10 params=\"A\\0\\A\\0\\\"");
            string str1 = SendCommand(console, str);
            return str1.Substring(str1.find(" ") + 1);
        }

        public static uint GetKernalVersion(this IXboxConsole console)
        {
            string str = string.Concat("consolefeatures ver=", JRPCVersion, " type=13 params=\"A\\0\\A\\0\\\"");
            string str1 = SendCommand(console, str);
            return uint.Parse(str1.Substring(str1.find(" ") + 1));
        }

        public static byte[] GetMemory(this IXboxConsole console, uint Address, uint Length)
        {

            {
                uint num = 0;
                byte[] numArray = new byte[Length];
                console.DebugTarget.GetMemory(Address, Length, numArray, out num);
                console.DebugTarget.InvalidateMemoryCache(true, Address, Length);
                return numArray;
            }
        }

        public static uint GetTemperature(this IXboxConsole console, TemperatureFlag TemperatureType)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=15 params=\"A\\0\\A\\1\\", Int, "\\", (int)TemperatureType, "\\\"" };
            string str = SendCommand(console, string.Concat(jRPCVersion));
            return uint.Parse(str.Substring(str.find(" ") + 1), NumberStyles.HexNumber);
        }

        public static void Push(this byte[] InArray, out byte[] OutArray, byte Value)
        {
            OutArray = new byte[InArray.Length + 1];
            InArray.CopyTo(OutArray, 0);
            OutArray[InArray.Length] = Value;
        }

        public static bool ReadBool(this IXboxConsole console, uint Address)
        {
            return console.GetMemory(Address, 1)[0] != 0;
        }

        public static byte ReadByte(this IXboxConsole console, uint Address)
        {
            return console.GetMemory(Address, 1)[0];
        }

        public static float ReadFloat(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 4);
            ReverseBytes(memory, 4);
            return BitConverter.ToSingle(memory, 0);
        }

        public static float[] ReadFloat(this IXboxConsole console, uint Address, uint ArraySize)
        {
            {
                float[] single = new float[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 4);
                ReverseBytes(memory, 4);
                for (int i = 0; i < ArraySize; i++)
                {
                    single[i] = BitConverter.ToSingle(memory, i * 4);
                }
                return single;
            }
        }

        public static short ReadInt16(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 2);
            ReverseBytes(memory, 2);
            return BitConverter.ToInt16(memory, 0);
        }

        public static short[] ReadInt16(this IXboxConsole console, uint Address, uint ArraySize)
        {

            {
                short[] num = new short[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 2);
                ReverseBytes(memory, 2);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToInt16(memory, i * 2);
                }
                return num;
            }
        }

        public static int ReadInt32(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 4);
            ReverseBytes(memory, 4);
            return BitConverter.ToInt32(memory, 0);
        }

        public static int[] ReadInt32(this IXboxConsole console, uint Address, uint ArraySize)
        {
            {
                int[] num = new int[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 4);
                ReverseBytes(memory, 4);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToInt32(memory, i * 4);
                }
                return num;
            }
        }

        public static long ReadInt64(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 8);
            ReverseBytes(memory, 8);
            return BitConverter.ToInt64(memory, 0);
        }

        public static long[] ReadInt64(this IXboxConsole console, uint Address, uint ArraySize)
        {

            {
                long[] num = new long[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 8);
                ReverseBytes(memory, 8);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = (long)BitConverter.ToUInt32(memory, i * 8);
                }
                return num;
            }
        }

        public static sbyte ReadSByte(this IXboxConsole console, uint Address)
        {
            return (sbyte)console.GetMemory(Address, 1)[0];
        }

        public static string ReadString(this IXboxConsole console, uint Address, uint size)
        {
            return Encoding.UTF8.GetString(console.GetMemory(Address, size));
        }

        public static ushort ReadUInt16(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 2);
            ReverseBytes(memory, 2);
            return BitConverter.ToUInt16(memory, 0);
        }

        public static ushort[] ReadUInt16(this IXboxConsole console, uint Address, uint ArraySize)
        {

            {
                ushort[] num = new ushort[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 2);
                ReverseBytes(memory, 2);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt16(memory, i * 2);
                }
                return num;
            }
        }

        public static uint ReadUInt32(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 4);
            ReverseBytes(memory, 4);
            return BitConverter.ToUInt32(memory, 0);
        }

        public static uint[] ReadUInt32(this IXboxConsole console, uint Address, uint ArraySize)
        {

            {
                uint[] num = new uint[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 4);
                ReverseBytes(memory, 4);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt32(memory, i * 4);
                }
                return num;
            }
        }

        public static ulong ReadUInt64(this IXboxConsole console, uint Address)
        {
            byte[] memory = console.GetMemory(Address, 8);
            ReverseBytes(memory, 8);
            return BitConverter.ToUInt64(memory, 0);
        }

        public static ulong[] ReadUInt64(this IXboxConsole console, uint Address, uint ArraySize)
        {

            {
                ulong[] num = new ulong[ArraySize];
                byte[] memory = console.GetMemory(Address, ArraySize * 8);
                ReverseBytes(memory, 8);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt32(memory, i * 8);
                }
                return num;
            }
        }

        public static uint ResolveFunction(this IXboxConsole console, string ModuleName, uint Ordinal)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=9 params=\"A\\0\\A\\2\\", String, "/", ModuleName.Length, "\\", ModuleName.ToHexString(), "\\", Int, "\\", Ordinal, "\\\"" };
            string str = SendCommand(console, string.Concat(jRPCVersion));
            return uint.Parse(str.Substring(str.find(" ") + 1), NumberStyles.HexNumber);
        }

        public static void SetLeds(this IXboxConsole console, LEDState Top_Left, LEDState Top_Right, LEDState Bottom_Left, LEDState Bottom_Right)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=14 params=\"A\\0\\A\\4\\", Int, "\\", (uint)Top_Left, "\\", Int, "\\", (uint)Top_Right, "\\", Int, "\\", (uint)Bottom_Left, "\\", Int, "\\", (uint)Bottom_Right, "\\\"" };
            SendCommand(console, string.Concat(jRPCVersion));
        }

        public static void SetMemory(this IXboxConsole console, uint Address, byte[] Data)
        {

            uint num = 0;
            console.DebugTarget.SetMemory(Address, (uint)Data.Length, Data, out num);
        }

        public static void ShutDownConsole(this IXboxConsole console)
        {
            try
            {
                string str = string.Concat("consolefeatures ver=", JRPCVersion, " type=11 params=\"A\\0\\A\\0\\\"");
                SendCommand(console, str);
            }
            catch
            {
            }
        }

        public static byte[] ToByteArray(this string String)
        {
            byte[] str = new byte[String.Length + 1];
            for (int i = 0; i < String.Length; i++)
            {
                str[i] = (byte)String[i];
            }
            return str;
        }

        public static string ToHexString(this string String)
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

        public static void WriteBool(this IXboxConsole console, uint Address, bool Value)
        {
            object obj;
            IXboxConsole xboxConsole = console;
            uint address = Address;
            byte[] numArray = new byte[1];
            byte[] numArray1 = numArray;
            if (Value)
            {
                obj = 1;
            }
            else
            {
                obj = null;
            }
            numArray1[0] = (byte)obj;
            xboxConsole.SetMemory(address, numArray);
        }

        public static void WriteBool(this IXboxConsole console, uint Address, bool[] Value)
        {
            object obj;
            byte[] numArray = new byte[0];
            for (int i = 0; i < Value.Length; i++)
            {
                byte[] numArray1 = numArray;
                if (Value[i])
                {
                    obj = 1;
                }
                else
                {
                    obj = null;
                }
                numArray1.Push(out numArray, (byte)obj);
            }
            console.SetMemory(Address, numArray);
        }

        public static void WriteByte(this IXboxConsole console, uint Address, byte Value)
        {
            console.SetMemory(Address, new byte[] { Value });
        }

        public static void WriteByte(this IXboxConsole console, uint Address, byte[] Value)
        {
            console.SetMemory(Address, Value);
        }

        public static void WriteFloat(this IXboxConsole console, uint Address, float Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            Array.Reverse(bytes);
            console.SetMemory(Address, bytes);
        }

        public static void WriteFloat(this IXboxConsole console, uint Address, float[] Value)
        {
            byte[] numArray = new byte[Value.Length * 4];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 4);
            }
            ReverseBytes(numArray, 4);
            console.SetMemory(Address, numArray);
        }

        public static void WriteInt16(this IXboxConsole console, uint Address, short Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 2);
            console.SetMemory(Address, bytes);
        }

        public static void WriteInt16(this IXboxConsole console, uint Address, short[] Value)
        {
            byte[] numArray = new byte[Value.Length * 2];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 2);
            }
            ReverseBytes(numArray, 2);
            console.SetMemory(Address, numArray);
        }

        public static void WriteInt32(this IXboxConsole console, uint Address, int Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 4);
            console.SetMemory(Address, bytes);
        }

        public static void WriteInt32(this IXboxConsole console, uint Address, int[] Value)
        {
            byte[] numArray = new byte[Value.Length * 4];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 4);
            }
            ReverseBytes(numArray, 4);
            console.SetMemory(Address, numArray);
        }

        public static void WriteInt64(this IXboxConsole console, uint Address, long Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 8);
            console.SetMemory(Address, bytes);
        }

        public static void WriteInt64(this IXboxConsole console, uint Address, long[] Value)
        {
            byte[] numArray = new byte[Value.Length * 8];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 8);
            }
            ReverseBytes(numArray, 8);
            console.SetMemory(Address, numArray);
        }

        public static void WriteSByte(this IXboxConsole console, uint Address, sbyte Value)
        {
            byte[] bytes = new byte[] { BitConverter.GetBytes(Value)[0] };
            console.SetMemory(Address, bytes);
        }

        public static void WriteSByte(this IXboxConsole console, uint Address, sbyte[] Value)
        {
            byte[] numArray = new byte[0];
            sbyte[] value = Value;
            for (int i = 0; i < value.Length; i++)
            {
                numArray.Push(out numArray, (byte)value[i]);
            }
            console.SetMemory(Address, numArray);
        }

        public static void WriteString(this IXboxConsole console, uint Address, string String)
        {
            byte[] numArray = new byte[0];
            string str = String;
            for (int i = 0; i < str.Length; i++)
            {
                byte num = (byte)str[i];
                numArray.Push(out numArray, num);
            }
            numArray.Push(out numArray, 0);
            console.SetMemory(Address, numArray);
        }

        public static void WriteUInt16(this IXboxConsole console, uint Address, ushort Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 2);
            console.SetMemory(Address, bytes);
        }

        public static void WriteUInt16(this IXboxConsole console, uint Address, ushort[] Value)
        {
            byte[] numArray = new byte[Value.Length * 2];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 2);
            }
            ReverseBytes(numArray, 2);
            console.SetMemory(Address, numArray);
        }

        public static void WriteUInt32(this IXboxConsole console, uint Address, uint Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 4);
            console.SetMemory(Address, bytes);
        }

        public static void WriteUInt32(this IXboxConsole console, uint Address, uint[] Value)
        {
            byte[] numArray = new byte[Value.Length * 4];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 4);
            }
            ReverseBytes(numArray, 4);
            console.SetMemory(Address, numArray);
        }

        public static void WriteUInt64(this IXboxConsole console, uint Address, ulong Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 8);
            console.SetMemory(Address, bytes);
        }

        public static void WriteUInt64(this IXboxConsole console, uint Address, ulong[] Value)
        {
            byte[] numArray = new byte[Value.Length * 8];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 8);
            }
            ReverseBytes(numArray, 8);
            console.SetMemory(Address, numArray);
        }

        public static uint XamGetCurrentTitleId(this IXboxConsole console)
        {
            string str = string.Concat("consolefeatures ver=", JRPCVersion, " type=16 params=\"A\\0\\A\\0\\\"");
            string str1 = SendCommand(console, str);
            return uint.Parse(str1.Substring(str1.find(" ") + 1), NumberStyles.HexNumber);
        }

        public static string XboxIP(this IXboxConsole console)
        {
            byte[] bytes = BitConverter.GetBytes(console.IPAddress);
            Array.Reverse(bytes);
            return (new IPAddress(bytes)).ToString();
        }

        public static void XNotify(this Xbox console, string Text)
        {
            console = new Xbox();
            console.XNotify(Text, 34);
        }

        public static void XNotify(this IXboxConsole console, string Text, uint Type)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", JRPCVersion, " type=12 params=\"A\\0\\A\\2\\", String, "/", Text.Length, "\\", Text.ToHexString(), "\\", Int, "\\", Type, "\\\"" };
            SendCommand(console, string.Concat(jRPCVersion));
        }


    }
}