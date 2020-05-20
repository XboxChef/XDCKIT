using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XDevkit
{
	#region Misc
	public static class misc
	{
		private readonly static uint ByteArray = 0;
		private readonly static uint Float = 0;
		private readonly static uint Uint64 = 0;

		private readonly static uint Uint64Array = 0;

		private static HashSet<Type> ValidReturnTypes;
		private static int ConversationTimeout;
		private static int ConnectTimeout;
		public readonly static uint Int;

		public readonly static uint THTVersion;

		public readonly static uint String;

		private static string DefaultConsole()
		{
			throw new NotImplementedException();
		}

		public static void CallVoid(uint Address, params object[] Arguments)
		{
			CallArgs(true, 0, typeof(void), null, 0, Address, 0, Arguments);
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
		internal static bool IsValidReturnType(Type t)
		{
			return ValidReturnTypes.Contains(t);
		}
		public static XboxConsole Jtag = new XboxConsole();

		private static string SendCommand(string Command)
		{
			string str = null;
			try
			{
				XboxConsole x = new XboxConsole();
				x.SendTextCommand(Command, out str);
				if (str.Contains("error="))
				{
					throw new Exception(str.Substring(11));
				}
				if (str.Contains("DEBUG"))
				{

				}
			}
			catch
			{

			}

			return str;
		}
		public static uint ResolveFunction(string ModuleName, uint Ordinal)
		{
			object[] XBDMVersion = new object[] { "consolefeatures ver=", THTVersion, " type=9 params=\"A\\0\\A\\2\\", String, "/", ModuleName.Length, "\\", ModuleName.ToHexString(), "\\", Int, "\\", Ordinal, "\\\"" };
			string str = SendCommand(string.Concat(XBDMVersion));
			return uint.Parse(str.Substring(str.find(" ") + 1), NumberStyles.HexNumber);
		}
		/// <summary>
		/// EDITED:  Do Not Use
		/// </summary>
		private static object CallArgs(bool SystemThread, uint Type, System.Type t, string module, int ordinal, uint Address, uint ArraySize, params object[] Arguments)
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
					name = new object[] { "Invalid type ", t.Name, Environment.NewLine, "XBDM only supports: bool, byte, short, int, long, ushort, uint, ulong, float, double" };
					throw new Exception(string.Concat(name));
				}

				ConversationTimeout = 4000000;
				ConnectTimeout = 4000000;
				object[] XBDMVersion = new object[] { "consolefeatures ver=", THTVersion, " type=", Type, null, null, null, null, null, null, null, null, null };
				XBDMVersion[4] = (SystemThread ? " system" : string.Empty);
				object[] objArray = XBDMVersion;
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
				XBDMVersion[6] = " as=";
				XBDMVersion[7] = ArraySize;
				XBDMVersion[8] = " params=\"A\\";
				XBDMVersion[9] = Address.ToString("X");
				XBDMVersion[10] = "\\A\\";
				XBDMVersion[11] = Arguments.Length;
				XBDMVersion[12] = "\\";
				string str2 = string.Concat(XBDMVersion);
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
						object[] num2 = new object[] { obj, Int, "\\", Converters.UIntToInt((uint)obj2), "\\" };
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
						byte[] numArray = Converters.IntArrayToByte((int[])obj2);
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
						ulong num5 = Converters.ConvertToUInt64(obj2);
						strArrays[3] = num5.ToString();
						strArrays[4] = "\\";
						str2 = string.Concat(strArrays);
					}
				}
				str2 = string.Concat(str2, "\"");
				string str5 = SendCommand(str2);
				string str6 = "buf_addr=";
				while (str5.Contains(str6))
				{
					Thread.Sleep(250);
					uint num6 = uint.Parse(str5.Substring(str5.find(str6) + str6.Length), NumberStyles.HexNumber);
					str5 = SendCommand(string.Concat("consolefeatures ", str6, "0x", num6.ToString("X")));
				}
				ConversationTimeout = 2000;
				ConnectTimeout = 5000;
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
								return Converters.UIntToInt(num7);
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
							if (Type == 0)
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
	}
	#endregion
}
