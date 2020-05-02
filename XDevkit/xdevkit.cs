
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace XDevkit
{
	#region XboxThread Class
	class XboxThread : IXboxThread
	{
		Xbox CurrentConsole = new Xbox();
		public uint CurrentProcessor
		{
			get;
		}

		public uint LastError
		{
			get;
		}

		public XBOX_EVENT_INFO StopEventInfo
		{
			get;
		}

		public XBOX_THREAD_INFO ThreadInfo
		{
			get;
		}

		public IXboxStackFrame TopOfStack
		{
			get;
		}

		public void Continue(bool Exception)
		{
			if (Exception == true)
			{
				CurrentConsole.SendTextCommand("Continue" );
			}

		}

		public void Halt()
		{
			CurrentConsole.SendTextCommand("Halt" );
		}

		public void Resume()
		{
			CurrentConsole.SendTextCommand("Resume" );
		}

		public void Suspend()
		{
			CurrentConsole.SendTextCommand("Suspend" );
		}
		public void Break()
		{
			CurrentConsole.SendTextCommand("Break" );
		}
	}
	#endregion
	
	#region XboxFile Class
	class XboxFile : IXboxFile
	{
		Xbox CurrentConsole = new Xbox();
		public object ChangeTime
		{
			get => changeTime(string.Empty);
			set => changeTime(value.ToString());
		}

		private string changeTime(string value)//Todo: Add the text Command....
		{

			if (value == null)
			{
				string r;
				CurrentConsole.SendTextCommand("", out r);
				return r;
			}
			else
			{
				string r;
				CurrentConsole.SendTextCommand("", out r);
				return null;
			}
		}
		private string creationTime(string value)//Todo: Add the text Command....
		{
			if (value == null)
			{
				string r;
				CurrentConsole.SendTextCommand("", out r);
				return r;
			}
			else
			{
				string r;
				CurrentConsole.SendTextCommand("", out r);
				return null;
			}
		}


		public object CreationTime
		{
			get => creationTime(string.Empty);
			set => creationTime(value.ToString());
		}

		public bool IsDirectory
		{
			get;
		}

		public bool IsReadOnly
		{
			get;
			set;
		}
		public ulong Size
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
	}
	#endregion
	
	#region Xbox Manager Class
	public class XboxManager : IXboxManager
	{
		int ConnectionAttempt;


		public XboxManager()
		{
		}
		XboxConsole IXboxManager.OpenConsole(string XboxName)
		{
			return null;
		}

		public IXboxConsole OpenConsole(string xboxNameOrIP)
		{
			if (xboxNameOrIP == "default")//Todo: Add A Way To Get Defualt IP 
			{


			}
			else//Connects threw Ip Adress If It Detects Numbers
			{
				if (ConnectionAttempt == 0)//checks if it already attempted a connection to Prevent it From Failing..
				{
					Xbox CurrentConsole = new Xbox();
					Console.WriteLine($"Debug: Connection returned {CurrentConsole.TCPConnect(xboxNameOrIP)}");
					ConnectionAttempt = 1;
				}
			}
			return null;
		}

		public int TranslateError(int errorCode)
		{
			return errorCode;
		}

		public string DefaultConsole
		{
			get
			{
				using (XmlReader reader = XmlReader.Create("Settings.xml"))
				{
					while (reader.Read())
					{
						if (reader.IsStartElement())
						{
							//return only when you have START tag  
							switch (reader.Name.ToString())
							{
								case "IP":

									//IPAddress = reader.ReadString();//todo:
									break;
							}
						}
					}
				}
				return "Default";
			}
			set
			{
				using (XmlWriter writer = XmlWriter.Create("Settings.xml"))
				{
					writer.WriteStartElement("Default");
					writer.WriteElementString("Description", "Saves Console's IP For Easy Access");
					writer.WriteElementString("IP", value);
					writer.WriteEndElement();
					writer.Flush();
				}
			}
		}


	}
	#endregion
	
	#region DebugTarget Class
	class XboxDebugTarget : IXboxDebugTarget
	{
		public TcpClient xboxName = Xbox.XboxName;
		public XboxConsole Console
		{
			get;
		}

		public bool IsDump
		{
			get;
		}

		public bool MemoryCacheEnabled
		{
			get; set;
		}

		public IXboxMemoryRegions MemoryRegions
		{
			get;
		}

		public IXboxModules Modules
		{
			get;
		}

		public XBOX_PROCESS_INFO RunningProcessInfo
		{
			get;
		}

		public IXboxThreads Threads
		{
			get;
		}

		public XboxManager XboxManager
		{
			get;
		}

		public void ConnectAsDebugger(string DebuggerName, XboxDebugConnectFlags Flags)
		{
			try
			{
				Xbox.TCPConnect(DebuggerName, XboxDebugConnectFlags.Force);

			}

			catch
			{

			}
		}

		public void CopyEventInfo(out XBOX_EVENT_INFO EventInfoDest, ref XBOX_EVENT_INFO EventInfoSource)
		{
			EventInfoDest = EventInfoSource;//compiler error added so it would stop yelling
		}

		public void DisconnectAsDebugger()
		{

		}

		public void FreeEventInfo(ref XBOX_EVENT_INFO EventInfo)
		{

		}

		public void GetMemory(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead)
		{
			BytesRead = 0;
			List<byte> ReturnData = new List<byte>();
			byte[] Packet = new byte[1026];
			Data = new byte[1024];

			// Send getmemex command.

			xboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), BytesToRead.ToString("X2"))));

			// Receieve the 203 response to verify we are going to recieve raw data in packets
			xboxName.Client.Receive(Packet);

			if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "203")
				throw new Exception("GETMEMEX 203 response not recieved. Cannot read memory.");

			// It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
			// Length / 1024 will get how many packets there are to recieve
			for (uint i = 0; i < BytesToRead / 1024; i++)
			{
				xboxName.Client.Receive(Packet);

				// Store the data minus the first two bytes
				// This was a cheap way of removing the 2 byte header
				Array.Copy(Packet, 2, Data, 0, 1024);
				ReturnData.AddRange(Data);
			}
		}

		public void GetMemory_cpp(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead)
		{

			BytesRead = 0;
			List<byte> ReturnData = new List<byte>();
			byte[] Packet = new byte[1026];
			Data = new byte[1024];

			// Send getmemex command.
			xboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), BytesToRead.ToString("X2"))));

			// Receieve the 203 response to verify we are going to recieve raw data in packets
			xboxName.Client.Receive(Packet);

			if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "203")
				throw new Exception("GETMEMEX 203 response not recieved. Cannot read memory.");

			// It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
			// Length / 1024 will get how many packets there are to recieve
			for (uint i = 0; i < BytesToRead / 1024; i++)
			{
				xboxName.Client.Receive(Packet);

				// Store the data minus the first two bytes
				// This was a cheap way of removing the 2 byte header
				Array.Copy(Packet, 2, Data, 0, 1024);
				ReturnData.AddRange(Data);
			}
		}

		public void Go(bool NotStopped)
		{

		}

		public void InvalidateMemoryCache(bool ExecutablePages, uint Address, uint Size)
		{

		}

		public void IsBreakpoint(uint Address, out XboxBreakpointType Type)
		{

			Type = XboxBreakpointType.NoBreakpoint;//just added a random one so compiler won't yell error
		}

		public bool IsDebuggerConnected(out string DebuggerName, out string UserName)
		{
			DebuggerName = "";
			UserName = "";
			return true;
		}

		public void PgoSaveSnapshot(string Phase, bool Reset, uint PgoModule)
		{

		}

		public void PgoSetAllocScale(uint PgoModule, uint BufferAllocScale)
		{

		}

		public void PgoStartDataCollection(uint PgoModule)
		{

		}

		public void PgoStopDataCollection(uint PgoModule)
		{

		}

		public void RemoveAllBreakpoints()
		{

		}

		public void RemoveBreakpoint(uint Address)
		{

		}

		public void SetBreakpoint(uint Address)
		{

		}

		public void SetDataBreakpoint(uint Address, XboxBreakpointType Type, uint Size)
		{

		}

		public void SetInitialBreakpoint()
		{

		}

		public void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten)//aka response
		{

			// Send the setmem command
			xboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n", Address.ToString("X2"), BitConverter.ToString(Data).Replace("-", ""))));

			// Check to see our response
			byte[] Packet = new byte[1026];
			xboxName.Client.Receive(Packet);
			BytesWritten = Convert.ToUInt32(Encoding.ASCII.GetString(Packet));
			if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 11) == "0 bytes set")
				throw new Exception("A problem occurred while writing bytes. 0 bytes set");
		}

		public void SetMemory_cpp(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten)
		{

			// Send the setmem command
			xboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n", Address.ToString("X2"), BitConverter.ToString(Data).Replace("-", ""))));

			// Check to see our response
			byte[] Packet = new byte[1026];
			xboxName.Client.Receive(Packet);
			BytesWritten = Convert.ToUInt32(Encoding.ASCII.GetString(Packet));
			if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 11) == "0 bytes set")
				throw new Exception("A problem occurred while writing bytes. 0 bytes set");
		}

		public void Stop(bool AlreadyStopped)
		{


		}

		public void StopOn(XboxStopOnFlags StopOn, bool Stop)
		{

		}

		public void WriteDump(string Filename, XboxDumpFlags Type)
		{
			byte[] Packet = new byte[1026];

			uint Address = 0;
			uint Length = 0;
			// Send getmemex command.
			xboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), Length.ToString("X2"))));

			// Receieve the 203 response to verify we are going to recieve raw data in packets
			xboxName.Client.Receive(Packet);

			if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "203")
				throw new Exception("GETMEMEX 203 response not recieved. Cannot read memory.");

			FileStream outfile = new FileStream(Filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);


			// It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
			// Length / 1024 will get how many packets there are to recieve
			for (uint i = 0; i < Length / 1024; i++)
			{
				xboxName.Client.Receive(Packet);

				// Write the data minus the first two bytes
				outfile.Write(Packet, 2, 1024);

			}

			// Get the remainder of Length / 1024 to see if we are recieving extra.
			uint Remainder = (Length % 1024);

			// If there is a remainder, read it
			if (Remainder > 0)
			{
				xboxName.Client.Receive(Packet);

				// Write the data minus the first two bytes
				outfile.Write(Packet, 2, 1024);
			}


			// Flush and close the file
			outfile.Flush();
			outfile.Close();
		}
	}
	#endregion
	
	#region XboxConsole Class
	partial class XboxConsoleClass : IXboxConsole
	{
		public static string IP;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public TcpClient XboxName = Xbox.XboxName;
		public static string DefaultXboxName = new XboxManager().DefaultConsole;


		public void CloseConnection(uint Connection)
		{
			SendTextCommand("bye");
			XboxName.Close();
			Xbox.XboxName.Close();
		}

		/// <summary>
		/// Deletes a file on the xbox.
		/// </summary>
		/// <param name="fileName">File to delete.</param>
		public void DeleteFile(string fileName)
		{
			string dre = string.Concat("delete name=\"{0}\"", fileName);
			SendTextCommand(dre);
		}

		public IXboxFiles DirectoryFiles(string Directory)
		{
			//"DIRLIST NAME="Directory"

			return null;
		}

		public void FindConsole(uint Retries, uint RetryDelay)// Todo: Add a Max And Minimal retry system.
		{

			Retries = 0;
			do
			{
				try
				{
					Retries++;
					CurrentConsole.ConsoleFinder();
					break; // Sucess! Lets exit the loop!
				}
				catch (Exception)
				{

					Task.Delay(4).Wait();
				}
			} while (true);

		}



		/// <summary>
		/// Dont use this, higher-level methods are available.  Use GetDriveFreeSpace or GetDriveSize instead.
		/// </summary>
		/// <param name="drive"></param>
		/// <param name="freeBytes"></param>
		/// <param name="driveSize"></param>
		/// <param name="totalFreeBytes"></param>

		public void GetDiskFreeSpace(ushort Drive, out ulong FreeBytesAvailableToCaller, out ulong TotalNumberOfBytes, out ulong TotalNumberOfFreeBytes)
		{
			FreeBytesAvailableToCaller = 0; TotalNumberOfBytes = 0; TotalNumberOfFreeBytes = 0;
			CurrentConsole.GetDriveInformation(Drive, out FreeBytesAvailableToCaller, out TotalNumberOfBytes, out TotalNumberOfFreeBytes);
		}

		public IXboxFile GetFileObject(string Filename)
		{
			return null;
		}

		public uint OpenConnection(string Handler)
		{
			Handler = "1";
			return uint.Parse(Handler);
		}

		public void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags)
		{
			string[] lines = Name.Split("\\".ToCharArray());
			for (int i = 0; i < lines.Length - 1; i++)
				MediaDirectory += lines[i] + "\\";
			object[] Reboot = new object[] { $"magicboot title=\"{Name}\" directory=\"{MediaDirectory}\"" };//todo
			SendTextCommand(string.Concat(Reboot));
		}

		public void ReceiveSocketLine(uint Connection, out string Line)
		{

			byte[] textBuffer = new byte[256];  // buffer large enough to contain a line of text

			Thread.Sleep(0);

			Stopwatch.StartNew();
			while (true)
			{

				int avail = XboxName.Available;   // only get once
				if (avail < textBuffer.Length)
				{
					XboxName.Client.Receive(textBuffer, avail, SocketFlags.Peek);
					Line = Encoding.ASCII.GetString(textBuffer, 0, avail);
				}
				else
				{
					XboxName.Client.Receive(textBuffer, textBuffer.Length, SocketFlags.Peek);
					Line = Encoding.ASCII.GetString(textBuffer);
				}

				int eolIndex = Line.IndexOf("\r\n");
				if (eolIndex != -1)
				{
					XboxName.Client.Receive(textBuffer, eolIndex + 2, SocketFlags.None);
					Encoding.ASCII.GetString(textBuffer, 0, eolIndex);
				}

				// end of line not found yet, lets wait some more...
				Thread.Sleep(0);
			}
		}

		public int ReceiveStatusResponse(uint Connection, out string Line)//get response codes here 
		{
			Line = null;
			return 0;
		}
		/// <summary>
		/// Renames or moves a file on the xbox.
		/// </summary>
		/// <param name="oldFileName">Old file name.</param>
		/// <param name="newFileName">New file name.</param>
		public void RenameFile(string OldFileName, string NewFileName)
		{

			string ren = string.Concat("rename name=\"{0}\" newname=\"{1}\"", OldFileName, NewFileName);
			SendTextCommand(ren);
		}

		public void ScreenShot(string Filename)
		{
			Xbox XConsole = new Xbox();
			XConsole.Screenshot(Filename);
		}


		public void SendTextCommand(string Command, out string Response)//Works flawlessly
		{
			CurrentConsole.SendTextCommand(Command, out Response);
		}

		public void XNotify(string Text)
		{
			XDevkit.XNotify.Show(Text,true);
		}


		public void SendBinary(uint connectionId, byte[] callData, uint length)
		{

		}

		public void ReceiveBinary(uint connectionId, byte[] numArray, uint length, out uint bytesReceived)
		{
			bytesReceived = 0;
		}

		public uint ConnectTimeout { get => Convert.ToUInt32(XboxName.ReceiveTimeout); set => XboxName.Client.SendTimeout = (int)value; }


		public uint ConversationTimeout { get => Convert.ToUInt32(XboxName.ReceiveTimeout); set => XboxName.SendTimeout = (int)value; }

		public IXboxDebugTarget DebugTarget
		{
			get;
		}
		public IXboxAutomation XboxAutomation { get; }
		public XboxDumpMode DumpMode { get; }



		public string Drives
		{
			get
			{
				string responses;
				SendTextCommand(string.Concat("drivelist"), out responses);
				return responses.Replace("200- drivelist=", string.Empty);
			}
		}

		public uint IPAddress
		{
			get
			{
				return Convert.ToUInt32(IP);
			}
		}

		public string SystemTime
		{
			get
			{
				string responses;
				SendTextCommand(string.Concat("systime"), out responses);
				return responses.Replace("200- systime=", string.Empty);
			}
			set
			{
				SendTextCommand(string.Concat("setsystime"));
			}
		}

		private void SendTextCommand(string Command)
		{
			if (Xbox.XboxName.Connected)
			{

				CurrentConsole.SendTextCommand(Command );
			}
		}

		public void SendTextCommand(uint connectionId, string Command, out string Response)
		{
			Response = null;
			if (Xbox.XboxName.Connected)
			{
				CurrentConsole.SendTextCommand(Command, out Response);
			}
		}

		public XBOX_PROCESS_INFO RunningProcessInfo { get; }


		public string Name { get; }
		public Xbox CurrentConsole { get; set; }
	}
	#endregion
	
	#region Automation Class
	class XboxAutomation : IXboxAutomation
	{
		public void BindController(uint UserIndex, uint QueueLength)
		{

		}

		public void ClearGamepadQueue(uint UserIndex)
		{

		}

		public void ConnectController(uint UserIndex)
		{

		}

		public void DisconnectController(uint UserIndex)
		{

		}

		public void GetInputProcess(uint UserIndex, out bool SystemProcess)
		{
			SystemProcess = true;
		}

		public void GetUserDefaultProfile(out long Xuid)
		{
			Xuid = 0;
		}

		public void QueryGamepadQueue(uint UserIndex, out uint QueueLength, out uint ItemsInQueue, out uint TimedDurationRemaining, out uint CountDurationRemaining)
		{
			QueueLength = 0; ItemsInQueue = 0; TimedDurationRemaining = 0; CountDurationRemaining = 0;
		}

		public bool QueueGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad, uint TimedDuration, uint CountDuration)
		{
			return true;
		}

		public void QueueGamepadState_cpp(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD GamepadArray, ref uint TimedDurationArray, ref uint CountDurationArray, uint ItemCount, out uint ItemsAddedToQueue)
		{
			ItemsAddedToQueue = 0;
		}

		public void SetGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad)
		{

		}

		public void SetUserDefaultProfile(long Xuid)
		{

		}

		public void UnbindController(uint UserIndex)
		{

		}
	}
	#endregion

	#region Misc
	public static class misc
	{
		private readonly static uint ByteArray = 0;
		private readonly static uint Float;
		private readonly static uint Uint64 = 0;

		private readonly static uint Uint64Array;

		private static HashSet<Type> ValidReturnTypes;

		private static Dictionary<Type, int> ValueTypeSizeMap;
		private readonly static uint Void;

		public readonly static uint Int;

		public readonly static uint THTVersion;

		public readonly static uint String;
		public static bool Connect(this IXboxConsole console, out IXboxConsole Console, string XboxNameOrIP = "default")
		{
			Console = console;
			Xbox cur = new Xbox();
			if (XboxNameOrIP == "default")
			{
				XboxNameOrIP = new XboxManager().DefaultConsole;
			}
			if(cur.TCPConnect(XboxNameOrIP))
			{
				return true;
			}
			else
			return false;
		}
		public static void CallVoid(this IXboxConsole console, uint Address, params object[] Arguments)
		{
			CallArgs(console, true, Void, typeof(void), null, 0, Address, 0, Arguments);
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

				}
			}
			catch
			{ 
			
			}
			
			return str;
		}
		public static uint ResolveFunction(this IXboxConsole console, string ModuleName, uint Ordinal)
		{
			object[] XBDMVersion = new object[] { "consolefeatures ver=", THTVersion, " type=9 params=\"A\\0\\A\\2\\", String, "/", ModuleName.Length, "\\", ModuleName.ToHexS(), "\\", Int, "\\", Ordinal, "\\\"" };
			string str = SendCommand(console, string.Concat(XBDMVersion));
			return uint.Parse(str.Substring(str.find(" ") + 1), NumberStyles.HexNumber);
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
					name = new object[] { "Invalid type ", t.Name, Environment.NewLine, "XBDM only supports: bool, byte, short, int, long, ushort, uint, ulong, float, double" };
					throw new Exception(string.Concat(name));
				}

				console.ConversationTimeout = 4000000;
				console.ConnectTimeout = 4000000;
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
						object[] objArray3 = new object[] { obj6, ByteArray.ToString(), "/", str4.Length, "\\", ((string)obj2).ToHexS(), "\\" };
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

	#region Xbox Main Class
	/// <summary>
	/// Todo:
	/// </summary>
	public class Xbox//will handle connection and commands and will use XDevkit as a gateway for commands to be passed threw
	{

		/// <summary>
		/// Gets the main connection used for pc to xbox communication.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static TcpClient XboxName;
		public string IPAddresss;
		private StreamReader sreader;
		public object[] arguments = new object[] { 0, 0, 0, 0 };

		public IXboxConsole Function = new XboxConsoleClass();
		public IXboxDebugTarget DebugTarget = new XboxDebugTarget();


		internal static void TCPConnect(string DebuggerName, XboxDebugConnectFlags Flags)
		{
			if (Flags == XboxDebugConnectFlags.Force)
			{

			}
			else if (Flags == XboxDebugConnectFlags.MonitorOnly)
			{

			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int timeout = 5000;
		private bool Connected;
		private string responses;
		private byte[] response_in_bytes;
		internal int connectionId;

		public bool TCPConnect(string XboxIP)
		{
			try
			{
				XboxName = new TcpClient(XboxIP, 730);
				sreader = new StreamReader(XboxName.GetStream());
				connectionId = 1;
				return Connected = (sreader.ReadLine().ToString().ToLower() == "201- connected");
			}
			catch
			{
				return false;
			}
		}


		/// Freezes/Stops Console.
		/// </summary>
		public void Freeze_Console(XboxSwitch Freeze)
		{
			if (Freeze == XboxSwitch.True)
			{
				SendTextCommand("stop");
			}
			else if (Freeze == XboxSwitch.False)
			{
				SendTextCommand("go");
			}
		}
		public void SendTextCommand(string Command, out byte[] response)//todo
		{
			response = null;
			if (XboxName == null)
			{
				Console.WriteLine("SendTextCommand ==> XboxName == null <==");
			}
			else
				try
				{
					// Max packet size is 1026
					byte[] Packet = new byte[1026];
					if (XboxName.Connected == false)
					{
						XBDM.Exception("Not Connected");
						Console.WriteLine("Failed to SendTextCommand ==> Not Connected <==");
					}
					else
						Console.WriteLine("SendTextCommand ==> Sending Command... <==");
					XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
					XboxName.Client.Receive(Packet);
					response = Encoding.Default.GetBytes(ReceiveMultilineResponse());
				}
				catch
				{

				}

		}
		public void SendTextCommand(string Command, out string response)//todo
		{
			response = "";
			if (XboxName == null)
			{
				Console.WriteLine("SendTextCommand ==> XboxName == null <==");
			}
			else
				try
				{
					// Max packet size is 1026
					byte[] Packet = new byte[1026];
					if (XboxName.Connected == false)
					{
						XBDM.Exception("Not Connected");
						Console.WriteLine("Failed to SendTextCommand ==> Not Connected <==");
					}
					else
						Console.WriteLine("SendTextCommand ==> Sending Command... <==");
					XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
					XboxName.Client.Receive(Packet);
					response = Encoding.ASCII.GetString(Packet);
				}
				catch
				{

				}

		}

		public string SendTextCommand(string Command)
		{
			try
			{
				SendTextCommand(Command, out responses);
			}
			catch
			{
			}
			return string.Empty;
		}

		/// <summary>
		/// Shortcuts To Guide The Xbox Very Fast
		/// </summary>
		/// <param name="Color"></param>
		public void XboxShortcut(XboxShortcuts UI)
		{

			if (XboxName.Connected)
				switch ((int)UI)//works by getting the int of the UI and matches the numbers to execute things
				{
					case (int)XboxShortcuts.XboxHome:
						XboxHome();
						break;
					case (int)XboxShortcuts.Turn_Off_Console:
						SendTextCommand("shutdown");
						break;
					case (int)XboxShortcuts.Account_Management:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Account_Management), arguments);
						break;
					case (int)XboxShortcuts.Achievements:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Achievements), arguments);//achievements
						break;
					case (int)XboxShortcuts.Active_Downloads:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Active_Downloads), arguments);//XamShowMarketplaceDownloadItemsUI
						break;
					case (int)XboxShortcuts.Awards:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Awards), arguments);
						break;
					case (int)XboxShortcuts.Beacons_And_Activiy:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Beacons_And_Activiy), arguments);
						break;
					case (int)XboxShortcuts.Family_Settings:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Family_Settings), arguments);
						break;
					case (int)XboxShortcuts.Friends:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Friends), arguments);//friends
						break;
					case (int)XboxShortcuts.Guide_Button:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Guide_Button), arguments);
						break;
					case (int)XboxShortcuts.Messages:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Messages), 0);//messages tab
						break;
					case (int)XboxShortcuts.My_Games:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.My_Games), arguments);
						break;
					case (int)XboxShortcuts.Open_Tray:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Open_Tray), arguments);
						break;
					case (int)XboxShortcuts.Party:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Party), arguments);
						break;
					case (int)XboxShortcuts.Preferences:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Preferences), arguments);
						break;
					case (int)XboxShortcuts.Private_Chat:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Private_Chat), arguments);
						break;
					case (int)XboxShortcuts.Profile:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Profile), arguments);
						break;
					case (int)XboxShortcuts.Recent:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Recent), arguments);
						break;
					case (int)XboxShortcuts.Redeem_Code:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Redeem_Code), arguments);
						break;
					case (int)XboxShortcuts.Select_Music:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Select_Music), arguments);
						break;
					case (int)XboxShortcuts.System_Music_Player:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Music_Player), arguments);
						break;
					case (int)XboxShortcuts.System_Settings:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Settings), arguments);
						break;
					case (int)XboxShortcuts.System_Video_Player:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Video_Player), arguments);
						break;
					case (int)XboxShortcuts.Windows_Media_Center:
						Function.CallVoid(Function.ResolveFunction("xam.xex", (int)XboxShortcuts.Windows_Media_Center), arguments);
						break;

				}
		}

		/// <summary>
		/// Returns To Console's Home.
		/// </summary>
		public void XboxHome()
		{
			Reboot(@"\Device\Harddisk0\SystemExtPartition\20445100\dash.xex", @"\Device\Harddisk0\SystemExtPartition\20445100\", @"\Device\Harddisk0\SystemExtPartition\20445100\", XboxRebootFlags.Title);
		}

		/// <summary>
		///Reboots you
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="MediaDirectory"></param>
		/// <param name="CmdLine"></param>
		/// <param name="Flags"></param>
		private void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags)
		{
			Function.Reboot(Name, MediaDirectory, CmdLine, Flags);
		}

		/// <summary>
		/// Turns The Console's Default Neighborhood Icon to any of the following...(black , blue , bluegray , nosidecar , white)
		/// Also Changes The Type Of Console It Is.
		/// </summary>
		/// <param name="Color"></param>
		public void ConsoleColor(XboxColor Color)
		{
			SendTextCommand("setcolor name=" + Enum.GetName(typeof(int), Color).ToLower());
		}

		/// <summary>
		/// Get's The Consoles ID.
		/// </summary>
		/// <returns></returns>
		public string GetConsoleID()
		{
			string responses;
			SendTextCommand(string.Concat("getconsoleid"), out responses);
			return responses.Replace("200- consoleid=", "");
		}

		/// <summary>
		/// Get's Consoles System Information.
		/// </summary>
		/// <param name="Type"></param>
		/// <returns>Type Is The System Type Of Information you Want To Retrieve</returns>
		public string GetSystemInfo(Info Type)//finish missing parts
		{
			Console.WriteLine("System Info Came Threw.. (Command Executed == " + Type + " )");
			switch ((int)Type)
			{
				case (int)Info.HDD:
					#region HDD
					try
					{
						SendTextCommand(string.Concat("systeminfo"));
						string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
						foreach (string s in Info)
						{
							int Start = s.IndexOf("hdd=");
							int End = s.IndexOf("type=");
							responses = s.Substring(Start + 4, End - 4);
							return responses;
						}
					}
					catch
					{

					}
					#endregion
					break;
				case (int)Info.Type:
					#region Console Type
					try
					{
						SendTextCommand(string.Concat("consoletype"),out responses);
							return responses.Replace("200- ", "");
					}
					catch
					{

					}
					#endregion
					break;
		        case (int)Info.Platform:
					#region Platform
					try
					{
						SendTextCommand(string.Concat("systeminfo"));
						string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
						foreach (string s in Info)
						{
							int Start = s.IndexOf("type=");
							int End = s.IndexOf(" p");
							responses = s.Substring(Start + End, End).Substring(Start);
							return responses;
						}
					}
					catch
					{

					}
					#endregion
					break;
				case (int)Info.System:
					#region System
					try
					{
						SendTextCommand(string.Concat("systeminfo"));
						string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
						foreach (string s in Info)
						{
							int Start = s.IndexOf("type=");
							int End = Int32.Parse(" basekrnl =");
							responses = s.Substring(Start , End);
							return responses;
						}
					}
					catch
					{

					}
					#endregion
					break;
				case (int)Info.BaseKrnlVersion:
					#region BaseKrnlVersion
					try
					{
						SendTextCommand(string.Concat("systeminfo"));
						string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
						foreach (string s in Info)
						{
							int Start = s.IndexOf(" krnl=");
							int End = s.IndexOf(" ");
							responses = s.Substring(Start - 10, End);
							return responses;
						}
					}
					catch
					{

					}
					#endregion
					break;
				case (int)Info.KrnlVersion:
					#region Kernal Version
					try
					{
						SendTextCommand(string.Concat("systeminfo"));
						string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
						foreach (string s in Info)
						{
							int Start = s.IndexOf(" krnl=");
							int End = s.IndexOf(" ");
							responses = s.Substring(Start + 6, End);
							return responses;
						}
					}
					catch
					{

					}
					#endregion
					break;
				case (int)Info.XDKVersion:
					#region XDK Version
					try
					{
						SendTextCommand(string.Concat("systeminfo"), out responses);
						string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
						foreach (string s in Info)
						{
							int Start = s.IndexOf(" xdk=");
							responses = s.Substring(Start + 5, 12);
							return responses;
						}
					}
					catch
					{

					}
					#endregion
					break;

			}
			return string.Empty;

		}

		/// <summary>
		/// Creates a directory on the xbox.
		/// </summary>
		/// <param name="name">Directory name.</param>
		public void CreateDirectory(string name)
		{
			string sdr = string.Concat("mkdir name=\"{0}\"", name);
			SendTextCommand(sdr, out responses);
		}
		/// <summary>
		/// Get's Box Id.
		/// </summary>
		/// <param name="fileName">File to delete.</param>
		public string GetBoxID()
		{
			string dre = string.Concat("BOXID");
			SendTextCommand(dre, out responses);
			return responses.Replace("200- ", "");
		}
		/// <summary>
		/// Gets the debugger version
		/// </summary>
		public string GetDMVersion()
		{
			string dre = string.Concat("dmversion");
			SendTextCommand(dre, out responses);
			return responses.Replace("200- ", "");
		}

		public void SetDebugName(string DebugName)
		{
	
			SendTextCommand("dbgname name=" + DebugName);
		}

		public void ConsoleFinder()
		{

		}
		/// <summary>
		/// Dont use this, higher-level methods are available.  Use GetDriveFreeSpace or GetDriveSize instead.
		/// </summary>
		/// <param name="drive"></param>
		/// <param name="FreeBytesAvailableToCaller"></param>
		/// <param name="TotalNumberOfBytes"></param>
		/// <param name="totalFreeBytes"></param>
		public void GetDriveInformation(ushort drive, out ulong FreeBytesAvailableToCaller, out ulong TotalNumberOfBytes, out ulong totalFreeBytes)
		{
			FreeBytesAvailableToCaller = 0; TotalNumberOfBytes = 0; totalFreeBytes = 0;
			SendTextCommand("drivefreespace name=\"{0}\"" + drive.ToString() + ":\\" );

			string msg = ReceiveMultilineResponse();
			FreeBytesAvailableToCaller = Convert.ToUInt64(msg.Substring(msg.IndexOf("freetocallerlo") + 17, 8), 16);
			FreeBytesAvailableToCaller |= (Convert.ToUInt64(msg.Substring(msg.IndexOf("freetocallerhi") + 17, 8), 16) << 32);

			TotalNumberOfBytes = Convert.ToUInt64(msg.Substring(msg.IndexOf("totalbyteslo") + 15, 8), 16);
			TotalNumberOfBytes |= (Convert.ToUInt64(msg.Substring(msg.IndexOf("totalbyteshi") + 15, 8), 16) << 32);

			totalFreeBytes = Convert.ToUInt64(msg.Substring(msg.IndexOf("totalfreebyteslo") + 19, 8), 16);
			totalFreeBytes |= (Convert.ToUInt64(msg.Substring(msg.IndexOf("totalfreebyteshi") + 19, 8), 16) << 32);
		}
		public XboxExecutionState ExecutionState()
		{
			string str = SendText("getexecstate")[0].Replace("200- ", "");
			if (str == "pending")
				return XboxExecutionState.Pending;
			else if (str == "reboot")
				return XboxExecutionState.Rebooting;
			else if (str == "start")
				return XboxExecutionState.Running;
			else if (str == "stop")
				return XboxExecutionState.Stopped;
			else if (str == "pending_title")
				return XboxExecutionState.TitlePending;
			else if (str == "reboot_title")
				return XboxExecutionState.TitleRebooting;
			return XboxExecutionState.Unknown;
		}
		public string[] SendText(string Text)
		{

			new BinaryWriter(XboxName.GetStream()).Write(Encoding.ASCII.GetBytes(Text + "\r\n"));
			return sreader.ReadToEnd().Split("\n".ToCharArray());
		}
		public XBOX_Hardware_Info HardwareInfo()
		{
			string[] lines = SendText("hwinfo");
			XBOX_Hardware_Info info;
			info.Flags = uint.Parse(lines[1].Split(" : ".ToCharArray())[1].Replace("0x", ""), NumberStyles.HexNumber);
			info.NumberOfProcessors = byte.Parse(lines[2].Split(" : ".ToCharArray())[1].Replace("0x", ""), NumberStyles.HexNumber);
			info.PCIBridgeRevisionID = byte.Parse(lines[3].Split(" : ".ToCharArray())[1].Replace("0x", ""), NumberStyles.HexNumber);
			info.ReservedBytes = new byte[6];
			info.ReservedBytes[0] = byte.Parse(lines[4].Split(" : 0x ".ToCharArray())[1].Substring(0, 2), NumberStyles.HexNumber);
			info.ReservedBytes[1] = byte.Parse(lines[4].Split(" : 0x ".ToCharArray())[1].Substring(3, 2), NumberStyles.HexNumber);
			info.ReservedBytes[2] = byte.Parse(lines[4].Split(" : 0x ".ToCharArray())[1].Substring(6, 2), NumberStyles.HexNumber);
			info.ReservedBytes[3] = byte.Parse(lines[4].Split(" : 0x ".ToCharArray())[1].Substring(9, 2), NumberStyles.HexNumber);
			info.ReservedBytes[4] = byte.Parse(lines[4].Split(" : 0x ".ToCharArray())[1].Substring(12, 2), NumberStyles.HexNumber);
			info.ReservedBytes[5] = byte.Parse(lines[4].Split(" : 0x ".ToCharArray())[1].Substring(15, 2), NumberStyles.HexNumber);
			info.BldrMagic = ushort.Parse(lines[5].Split(" : ".ToCharArray())[1].Replace("0x", ""), NumberStyles.HexNumber);
			info.BldrFlags = ushort.Parse(lines[6].Split(" : ".ToCharArray())[1].Replace("0x", ""), NumberStyles.HexNumber);
			return info;
		}

		/// <summary>
		/// Receives multiple lines of text from the xbox.
		/// </summary>
		/// <returns></returns>
		public string ReceiveMultilineResponse()
		{
			StringBuilder response = new StringBuilder();
			while (true)
			{
				string line = ReceiveSocketLine() + " ";//change here if any issue accurs
				if (line[0] == '.') break;
				else response.Append(line);
			}
			return response.ToString();
		}
		public string ReceiveSocketLine()
		{

			string Line;
			byte[] textBuffer = new byte[256];  // buffer large enough to contain a line of text

			Thread.Sleep(0);

			Stopwatch sw = Stopwatch.StartNew();
			while (true)
			{
				int avail = XboxName.Available;   // only get once
				if (avail < textBuffer.Length)
				{
					XboxName.Client.Receive(textBuffer, avail, SocketFlags.Peek);
					Line = Encoding.ASCII.GetString(textBuffer, 0, avail);
				}
				else
				{
					XboxName.Client.Receive(textBuffer, textBuffer.Length, SocketFlags.Peek);
					Line = Encoding.ASCII.GetString(textBuffer);
				}

				int eolIndex = Line.IndexOf("\r\n");
				if (eolIndex != -1)
				{
					XboxName.Client.Receive(textBuffer, eolIndex + 2, SocketFlags.None);
					return Encoding.ASCII.GetString(textBuffer, 0, eolIndex);
				}

				// end of line not found yet, lets wait some more...
				Thread.Sleep(0);
			}
		}

		/// <summary>
		/// Reboot Method flag types cold or warm reboot.
		/// </summary>
		public void Reboot(XboxReboot Warm_or_Cold)
		{
			if (Warm_or_Cold == XboxReboot.Cold)
			{
				SendTextCommand("magicboot cold", out responses);
			}
			if (Warm_or_Cold == XboxReboot.Warm)
			{
				SendTextCommand("magicboot warm", out responses);
			}
		}


		public Image Screenshot(string filename)
		{
			SendTextCommand("screenshot", out response_in_bytes);
			return (Bitmap)((new ImageConverter()).ConvertFrom(response_in_bytes));
		}
		public Image Screenshot()
		{
			SendTextCommand("screenshot", out response_in_bytes);
			return (Bitmap)((new ImageConverter()).ConvertFrom(response_in_bytes));
		}
	}

	#endregion

	#region Converter
	public static class Converters
	{
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

	#region Xbox Enums
	public enum XboxFunctionType
	{
		NoPData = -1,
		SaveMillicode = 0,
		NoHandler = 1,
		RestoreMillicode = 2,
		Handler = 3
	}
	public enum XboxSwitch
	{
		True,
		False
	}
	public enum XboxExecutionState
	{
		Pending,
		Rebooting,
		Running,
		Stopped,
		TitlePending,
		TitleRebooting,
		Unknown,

	}
	/// <summary>
	/// Used to Get Version Information And Console Type
	/// </summary>
	public enum Info
	{
		HDD,
		Type,
		Platform,
		System,
		BaseKrnlVersion,
		KrnlVersion,
		XDKVersion,
	}
	public enum TRAY_STATE
	{
		OPEN,
		UNKNOWN,
		CLOSED,
		OPENING,
		CLOSING
	}
	public enum XboxDrive
	{
		HDD,
		INTUSB,
		USB0,
		CdRom0,
		DVD,
		GAME,
		D,
		DASHUSER,
		media,
		SysCache0,
		SysCache1,
	}
	public enum XboxChars
	{
		a = 4,
		aa = 0x2d,
		b = 5,
		bb = 0x2e,
		c = 6,
		Caps = 0x39,
		cc = 0x2f,
		d = 7,
		dd = 0x30,
		Delete = 0x4c,
		e = 8,
		ee = 0x31,
		eight = 0x25,
		f = 9,
		ff = 0x33,
		five = 0x22,
		four = 0x21,
		g = 10,
		gg = 0x34,
		h = 11,
		hh = 0x35,
		i = 12,
		ii = 0x36,
		j = 13,
		jj = 0x37,
		k = 14,
		kk = 0x38,
		l = 15,
		m = 0x10,
		n = 0x11,
		nine = 0x26,
		o = 0x12,
		one = 30,
		p = 0x13,
		q = 20,
		r = 0x15,
		s = 0x16,
		seven = 0x24,
		six = 0x23,
		Space = 0x2c,
		t = 0x17,
		three = 0x20,
		two = 0x1f,
		u = 0x18,
		v = 0x19,
		w = 0x1a,
		x = 0x1b,
		y = 0x1c,
		z = 0x1d,
		zero = 0x27
	}
	public enum TRAY
	{
		CLOSE = 0x62,
		OPEN = 0x60
	}
	public enum XboxColor
	{
		Black,
		Blue,
		BlueGray,
		White,
	};
	public enum XboxReboot
	{
		Cold = 2,
		Warm = 4,
	}
	/// <summary>
	/// Party System
	/// </summary>
	public enum Xbox_Party_Options
	{
		CreateParty = 0xafc,
		PartySettings = 0xb08,
		InviteOnly = 1,
		Kick = 0xb02,
		OpenParty = 0,
		JoinParty = 0xb01,
		AltJoinParty = 0xb1b,
		LeaveParty = 0xafd,
		InvitePlayer = 0xb15,

	}
	/// <summary>
	/// Guide Shortcuts
	/// </summary>
	public enum XboxShortcuts
	{
		//Main Shortcut
		Recent = 0x2C8,
		Guide_Button = 0x506,
		//End Of Main Shortcut

		//Games And Apps Tab
		Achievements = 0x2D0,
		Awards = 0x03C6,
		My_Games,
		Active_Downloads = 0x02E7,
		Redeem_Code,
		//End Of Games And Apps Tab

		//Main Guide
		XboxHome,
		Friends = 0x2BF,
		Party = 0x0305,
		Messages = 0x2C0,
		Beacons_And_Activiy = 0xB39,
		Private_Chat = 0x2C2,
		Open_Tray = 0x60,
		//End Of Main Guide

		//Media
		System_Video_Player = 2,
		System_Music_Player = 1,
		Picture_Viewer,
		Windows_Media_Center,
		Select_Music = 0,
		//End Of Media

		//settings
		Profile = 0x2c4,
		Preferences,
		Family_Settings,
		System_Settings,
		Account_Management = 4,
		Turn_Off_Console = 0x0295
		//End Of settings

	};

	public enum XboxExceptionFlags
	{
		Noncontinuable = 1,
		FirstChance = 2
	}
	public enum XboxEventDeferFlags
	{
		CanDeferExecutionBreak = 1,
		CanDeferDebugString = 2,
		CanDeferSingleStep = 4,
		CanDeferAssertionFailed = 8,
		CanDeferAssertionFailedEx = 16,
		CanDeferDataBreak = 32,
		CanDeferRIP = 64
	}
	public enum XboxDumpReportFlags
	{
		FormatFullHeap = 0,
		LocalDestination = 0,
		PromptToReport = 0,
		AlwaysReport = 1,
		NeverReport = 2,
		DestinationGroup = 15,
		ReportGroup = 15,
		RemoteDestination = 16,
		FormatPartialHeap = 256,
		FormatNoHeap = 512,
		FormatRetail = 1024,
		FormatGroup = 3840
	}
	public enum XboxCreateDisposition
	{
		CreateNew = 1,
		CreateAlways = 2,
		OpenExisting = 3,
		OpenAlways = 4
	}
	public enum XboxConsoleType
	{
		DevelopmentKit,
		TestKit,
		ReviewerKit
	}
	public enum XboxBreakpointType
	{
		NoBreakpoint,
		OnWrite,
		OnRead,
		OnExecuteHW,
		OnExecute
	}
	public enum XboxDumpMode
	{
		Smart,
		Enabled,
		Disabled
	}
	public enum XboxDumpFlags
	{
		Normal = 0,
		WithDataSegs = 1,
		WithFullMemory = 2,
		WithHandleData = 4,
		FilterMemory = 8,
		ScanMemory = 16,
		WithUnloadedModules = 32,
		WithIndirectlyReferencedMemory = 64,
		FilterModulePaths = 128,
		WithProcessThreadData = 256,
		WithPrivateReadWriteMemory = 512
	}
	public enum XboxDebugEventType
	{
		NoEvent,
		ExecutionBreak,
		DebugString,
		ExecStateChange,
		SingleStep,
		ModuleLoad,
		ModuleUnload,
		ThreadCreate,
		ThreadDestroy,
		Exception,
		AssertionFailed,
		AssertionFailedEx,
		DataBreak,
		RIP,
		SectionLoad,
		SectionUnload,
		StackTrace,
		FiberCreate,
		FiberDestroy,
		BugCheck,
		PgoModuleStartup
	}
	public enum XboxStopOnFlags
	{
		OnThreadCreate = 1,
		OnFirstChanceException = 2,
		OnDebugString = 4,
		OnStackTrace = 8,
		OnModuleLoad = 16,
		OnTitleLaunch = 32,
		OnPgoModuleStartup = 64
	}
	public enum XboxAutomationButtonFlags
	{
		DPadUp = 1,
		DPadDown = 2,
		DPadLeft = 4,
		DPadRight = 8,
		StartButton = 16,
		BackButton = 32,
		LeftThumbButton = 64,
		RightThumbButton = 128,
		LeftShoulderButton = 256,
		RightShoulderButton = 512,
		Xbox360_Button = 1024,
		Bind_Button = 2048,
		A_Button = 4096,
		B_Button = 8192,
		X_Button = 16384,
		Y_Button = 32768
	}
	public enum XboxAccessFlags
	{
		Read = 1,
		Write = 2,
		Control = 4,
		Configure = 8,
		Manage = 16
	}
	public enum XboxShareMode
	{
		ShareNone = 0,
		ShareRead = 1,
		ShareWrite = 2,
		ShareDelete = 4
	}
	public enum XboxSelectConsoleFlags
	{
		NoPromptIfDefaultExists,
		NoPromptIfOnlyOne,
		FilterByAccess
	}
	public enum XboxSectionInfoFlags
	{
		Loaded = 1,
		Readable = 2,
		Writeable = 4,
		Executable = 8,
		Uninitialized = 16
	}
	public enum XboxRegistersVector
	{
		v0,
		v1,
		v2,
		v3,
		v4,
		v5,
		v6,
		v7,
		v8,
		v9,
		v10,
		v11,
		v12,
		v13,
		v14,
		v15,
		v16,
		v17,
		v18,
		v19,
		v20,
		v21,
		v22,
		v23,
		v24,
		v25,
		v26,
		v27,
		v28,
		v29,
		v30,
		v31,
		v32,
		v33,
		v34,
		v35,
		v36,
		v37,
		v38,
		v39,
		v40,
		v41,
		v42,
		v43,
		v44,
		v45,
		v46,
		v47,
		v48,
		v49,
		v50,
		v51,
		v52,
		v53,
		v54,
		v55,
		v56,
		v57,
		v58,
		v59,
		v60,
		v61,
		v62,
		v63,
		v64,
		v65,
		v66,
		v67,
		v68,
		v69,
		v70,
		v71,
		v72,
		v73,
		v74,
		v75,
		v76,
		v77,
		v78,
		v79,
		v80,
		v81,
		v82,
		v83,
		v84,
		v85,
		v86,
		v87,
		v88,
		v89,
		v90,
		v91,
		v92,
		v93,
		v94,
		v95,
		v96,
		v97,
		v98,
		v99,
		v100,
		v101,
		v102,
		v103,
		v104,
		v105,
		v106,
		v107,
		v108,
		v109,
		v110,
		v111,
		v112,
		v113,
		v114,
		v115,
		v116,
		v117,
		v118,
		v119,
		v120,
		v121,
		v122,
		v123,
		v124,
		v125,
		v126,
		v127,
		vscr
	}
	public enum XboxRegisters64
	{
		ctr,
		r0,
		r1,
		r2,
		r3,
		r4,
		r5,
		r6,
		r7,
		r8,
		r9,
		r10,
		r11,
		r12,
		r13,
		r14,
		r15,
		r16,
		r17,
		r18,
		r19,
		r20,
		r21,
		r22,
		r23,
		r24,
		r25,
		r26,
		r27,
		r28,
		r29,
		r30,
		r31
	}
	public enum XboxDebugConnectFlags
	{
		Force = 1,
		MonitorOnly = 2
	}
	public enum XboxMemoryRegionFlags
	{
		NoAccess = 1,
		ReadOnly = 2,
		ReadWrite = 4,
		WriteCopy = 8,
		Execute = 16,
		ExecuteRead = 32,
		ExecuteReadWrite = 64,
		ExecuteWriteCopy = 128,
		Guard = 256,
		NoCache = 512,
		WriteCombine = 1024,
		UserReadOnly = 4096,
		UserReadWrite = 8192
	}
	public enum XboxRebootFlags
	{
		Title = 0,
		Wait = 1,
		Cold = 2,
		Warm = 4,
		Stop = 8
	}
	public enum XboxModuleInfoFlags
	{
		Main = 1,
		Tls = 2,
		Dll = 4
	}
	public enum XboxRegisters32
	{
		msr,
		iar,
		lr,
		cr,
		xer
	}
	public enum XboxRegistersDouble
	{
		fp0,
		fp1,
		fp2,
		fp3,
		fp4,
		fp5,
		fp6,
		fp7,
		fp8,
		fp9,
		fp10,
		fp11,
		fp12,
		fp13,
		fp14,
		fp15,
		fp16,
		fp17,
		fp18,
		fp19,
		fp20,
		fp21,
		fp22,
		fp23,
		fp24,
		fp25,
		fp26,
		fp27,
		fp28,
		fp29,
		fp30,
		fp31,
		fpscr
	}
	public enum LEDState
	{
		OFF = 0,
		RED = 8,
		GREEN = 128,
		ORANGE = 136
	}

	public enum TemperatureFlag
	{
		CPU,
		GPU,
		EDRAM,
		MotherBoard
	}

	public enum ThreadType
	{
		System,
		Title
	}
	public enum XNotiyLogo
	{
		XBOX_LOGO = 0,
		NEW_MESSAGE_LOGO = 1,
		FRIEND_REQUEST_LOGO = 2,
		NEW_MESSAGE = 3,
		FLASHING_XBOX_LOGO = 4,
		GAMERTAG_SENT_YOU_A_MESSAGE = 5,
		GAMERTAG_SINGED_OUT = 6,
		GAMERTAG_SIGNEDIN = 7,
		GAMERTAG_SIGNED_INTO_XBOX_LIVE = 8,
		GAMERTAG_SIGNED_IN_OFFLINE = 9,
		GAMERTAG_WANTS_TO_CHAT = 10,
		DISCONNECTED_FROM_XBOX_LIVE = 11,
		DOWNLOAD = 12,
		FLASHING_MUSIC_SYMBOL = 13,
		FLASHING_HAPPY_FACE = 14,
		FLASHING_FROWNING_FACE = 15,
		FLASHING_DOUBLE_SIDED_HAMMER = 16,
		GAMERTAG_WANTS_TO_CHAT_2 = 17,
		PLEASE_REINSERT_MEMORY_UNIT = 18,
		PLEASE_RECONNECT_CONTROLLERM = 19,
		GAMERTAG_HAS_JOINED_CHAT = 20,
		GAMERTAG_HAS_LEFT_CHAT = 21,
		GAME_INVITE_SENT = 22,
		FLASH_LOGO = 23,
		PAGE_SENT_TO = 24,
		FOUR_2 = 25,
		FOUR_3 = 26,
		ACHIEVEMENT_UNLOCKED = 27,
		FOUR_9 = 28,
		GAMERTAG_WANTS_TO_TALK_IN_VIDEO_KINECT = 29,
		VIDEO_CHAT_INVITE_SENT = 30,
		READY_TO_PLAY = 31,
		CANT_DOWNLOAD_X = 32,
		DOWNLOAD_STOPPED_FOR_X = 33,
		FLASHING_XBOX_CONSOLE = 34,
		X_SENT_YOU_A_GAME_MESSAGE = 35,
		DEVICE_FULL = 36,
		FOUR_7 = 37,
		FLASHING_CHAT_ICON = 38,
		ACHIEVEMENTS_UNLOCKED = 39,
		X_HAS_SENT_YOU_A_NUDGE = 40,
		MESSENGER_DISCONNECTED = 41,
		BLANK = 42,
		CANT_SIGN_IN_MESSENGER = 43,
		MISSED_MESSENGER_CONVERSATION = 44,
		FAMILY_TIMER_X_TIME_REMAINING = 45,
		DISCONNECTED_XBOX_LIVE_11_MINUTES_REMAINING = 46,
		KINECT_HEALTH_EFFECTS = 47,
		FOUR_5 = 48,
		GAMERTAG_WANTS_YOU_TO_JOIN_AN_XBOX_LIVE_PARTY = 49,
		PARTY_INVITE_SENT = 50,
		GAME_INVITE_SENT_TO_XBOX_LIVE_PARTY = 51,
		KICKED_FROM_XBOX_LIVE_PARTY = 52,
		NULLED = 53,
		DISCONNECTED_XBOX_LIVE_PARTY = 54,
		DOWNLOADED = 55,
		CANT_CONNECT_XBL_PARTY = 56,
		GAMERTAG_HAS_JOINED_XBL_PARTY = 57,
		GAMERTAG_HAS_LEFT_XBL_PARTY = 58,
		GAMER_PICTURE_UNLOCKED = 59,
		AVATAR_AWARD_UNLOCKED = 60,
		JOINED_XBL_PARTY = 61,
		PLEASE_REINSERT_USB_STORAGE_DEVICE = 62,
		PLAYER_MUTED = 63,
		PLAYER_UNMUTED = 64,
		FLASHING_CHAT_SYMBOL = 65,
		UPDATING = 76,
	}
	#endregion

	#region Xbox Structs
	public struct XBOX_AUTOMATION_GAMEPAD
	{
		public XboxAutomationButtonFlags Buttons;

		public uint LeftTrigger;

		public uint RightTrigger;

		public int LeftThumbX;

		public int LeftThumbY;

		public int RightThumbX;

		public int RightThumbY;
	}

	public struct XBOX_SECTION_INFO
	{
		public string Name;

		public uint BaseAddress;

		public uint Size;

		public uint Index;

		public XboxSectionInfoFlags Flags;
	}

	public struct XBOX_THREAD_INFO
	{
		public uint ThreadId;

		public uint SuspendCount;

		public uint Priority;

		public uint TlsBase;

		public uint StartAddress;

		public uint StackBase;

		public uint StackLimit;

		public uint StackSlackSpace;

		public object CreateTime;

		public string Name;
	}

	public struct XBOX_USER
	{
		public string UserName;

		public XboxAccessFlags Access;
	}

	public struct XBOX_DUMP_SETTINGS
	{
		public XboxDumpReportFlags Flags;

		public string NetworkPath;
	}

	public struct XBOX_EVENT_INFO
	{
		public XboxDebugEventType Event;

		public short IsThreadStopped;

		public IXboxThread Thread;

		public IXboxModule Module;

		public IXboxSection Section;

		public XboxExecutionState ExecState;

		public string Message;

		public uint Code;

		public uint Address;

		public XboxExceptionFlags Flags;

		public uint ParameterCount;

		public uint[] Parameters;
	}

	public struct XBOX_FUNCTION_INFO
	{
		public XboxFunctionType FunctionType;

		public uint BeginAddress;

		public uint PrologEndAddress;

		public uint FunctionEndAddress;
	}

	public struct XBOX_MODULE_INFO
	{
		public string Name;

		public string FullName;

		public uint BaseAddress;

		public uint Size;

		public uint TimeStamp;

		public uint CheckSum;

		public XboxModuleInfoFlags Flags;
	}

	public struct XBOX_PROCESS_INFO
	{
		public uint ProcessId;

		public string ProgramName;
	}
	public struct XBOX_Hardware_Info
	{
		public uint Flags;
		public byte NumberOfProcessors, PCIBridgeRevisionID;
		public byte[] ReservedBytes;
		public ushort BldrMagic, BldrFlags;
	}
	public struct XBOX_Vector2
	{
		public float x, y;
	}
	public struct XBOX_Vector3
	{
		public float x, y, z;
	}
	#endregion
	
	#region Interfaces
	public interface IXboxConsole
	{
		Xbox CurrentConsole { get; set; }
		IXboxFile GetFileObject(string Filename);
		IXboxFiles DirectoryFiles(string Directory);
		void DeleteFile(string Filename);
		void RenameFile(string OldName, string NewName);
		void ScreenShot(string Filename);
		void CloseConnection(uint Connection);
		void FindConsole(uint Retries, uint RetryDelay);
		void GetDiskFreeSpace(ushort Drive, out ulong FreeBytesAvailableToCaller, out ulong TotalNumberOfBytes, out ulong TotalNumberOfFreeBytes);
		uint OpenConnection(string Handler);
		void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags);
		void ReceiveSocketLine(uint Connection, out string Line);
		int ReceiveStatusResponse(uint Connection, out string Line);
		void SendTextCommand(string text, out string response);

		void XNotify(string message);

		uint ConnectTimeout { get; set; }

		uint ConversationTimeout { get; set; }
		IXboxDebugTarget DebugTarget { get; }
		string Drives { get; }

		uint IPAddress { get; }
		string SystemTime { get; set; }
		XBOX_PROCESS_INFO RunningProcessInfo { get; }
		IXboxAutomation XboxAutomation { get; }
		string Name { get; }

		void SendBinary(uint connectionId, byte[] callData, uint length);
		void ReceiveBinary(uint connectionId, byte[] numArray, uint length, out uint bytesReceived);
		void SendTextCommand(uint connectionId, string command, out string str);
	}

	public interface IXboxAutomation
	{
		void BindController(uint UserIndex, uint QueueLength);

		void ClearGamepadQueue(uint UserIndex);

		void ConnectController(uint UserIndex);

		void DisconnectController(uint UserIndex);

		void GetInputProcess(uint UserIndex, out bool SystemProcess);
		void GetUserDefaultProfile(out long Xuid);

		void QueryGamepadQueue(uint UserIndex, out uint QueueLength, out uint ItemsInQueue, out uint TimedDurationRemaining, out uint CountDurationRemaining);

		bool QueueGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad, uint TimedDuration, uint CountDuration);

		void QueueGamepadState_cpp(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD GamepadArray, ref uint TimedDurationArray, ref uint CountDurationArray, uint ItemCount, out uint ItemsAddedToQueue);

		void SetGamepadState(uint UserIndex, ref XBOX_AUTOMATION_GAMEPAD Gamepad);
		void SetUserDefaultProfile(long Xuid);
		void UnbindController(uint UserIndex);
	}

	public interface IXboxDebugTarget
	{


		void ConnectAsDebugger(string DebuggerName, XboxDebugConnectFlags Flags);
		void CopyEventInfo(out XBOX_EVENT_INFO EventInfoDest, ref XBOX_EVENT_INFO EventInfoSource);
		void DisconnectAsDebugger();
		void FreeEventInfo(ref XBOX_EVENT_INFO EventInfo);
		void GetMemory(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead);
		void GetMemory_cpp(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead);
		void Go(bool NotStopped);
		void InvalidateMemoryCache(bool ExecutablePages, uint Address, uint Size);
		void IsBreakpoint(uint Address, out XboxBreakpointType Type);
		bool IsDebuggerConnected(out string DebuggerName, out string UserName);
		void PgoSaveSnapshot(string Phase, bool Reset, uint PgoModule);
		void PgoSetAllocScale(uint PgoModule, uint BufferAllocScale);
		void PgoStartDataCollection(uint PgoModule);
		void PgoStopDataCollection(uint PgoModule);
		void RemoveAllBreakpoints();
		void RemoveBreakpoint(uint Address);
		void SetBreakpoint(uint Address);
		void SetDataBreakpoint(uint Address, XboxBreakpointType Type, uint Size);
		void SetInitialBreakpoint();
		void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten);
		void SetMemory_cpp(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten);
		void Stop(bool AlreadyStopped);
		void StopOn(XboxStopOnFlags StopOn, bool Stop);
		void WriteDump(string Filename, XboxDumpFlags Type);

		XboxConsole Console { get; }
		bool IsDump { get; }
		bool MemoryCacheEnabled { get; set; }

		IXboxMemoryRegions MemoryRegions { get; }

		IXboxModules Modules { get; }
		XBOX_PROCESS_INFO RunningProcessInfo { get; }
		IXboxThreads Threads { get; }

		XboxManager XboxManager { get; }
	}

	public interface XboxConsole : IXboxConsole
	{

	}

	public interface IXboxStackFrame
	{
		void FlushRegisterChanges();
		bool GetRegister32(XboxRegisters32 Register, out int Value);
		bool GetRegister64(XboxRegisters64 Register, out long Value);
		bool GetRegisterDouble(XboxRegistersDouble Register, out double Value);
		bool GetRegisterVector(XboxRegistersVector Register, float[] Value);
		bool GetRegisterVector_cpp(XboxRegistersVector Register, out float Value);
		void SetRegister32(XboxRegisters32 Register, int Value);
		void SetRegister64(XboxRegisters64 Register, long Value);
		void SetRegisterDouble(XboxRegistersDouble Register, double Value);
		void SetRegisterVector(XboxRegistersVector Register, float[] Value);
		void SetRegisterVector_cpp(XboxRegistersVector Register, ref float Value);

		bool Dirty { get; }
		XBOX_FUNCTION_INFO FunctionInfo { get; }
		IXboxStackFrame NextStackFrame { get; }
		uint ReturnAddress { get; }
		uint StackPointer { get; }
		bool TopOfStack { get; }
	}

	public interface IXboxThread
	{

		void Continue(bool Exception);
		void Halt();
		void Resume();
		void Suspend();

		uint CurrentProcessor { get; }
		uint LastError { get; }
		XBOX_EVENT_INFO StopEventInfo { get; }
		XBOX_THREAD_INFO ThreadInfo { get; }
		IXboxStackFrame TopOfStack { get; }
	}

	public interface IXboxThreads : IEnumerable
	{

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface XboxEvents
	{
		void OnStdNotify(XboxDebugEventType EventCode, IXboxEventInfo EventInfo);

		void OnTextNotify(string Source, string Notification);
	}

	public interface IXboxManager
	{
		XboxConsole OpenConsole(string XboxName);
		string DefaultConsole { get; set; }

		int TranslateError(int errorCode);
	}

	public interface IXboxEventInfo
	{
		XBOX_EVENT_INFO xbox_event_info { get; }
	}

	public interface IXboxFile
	{

		object ChangeTime { get; set; }
		object CreationTime { get; set; }
		bool IsDirectory { get; }
		bool IsReadOnly { get; set; }
		ulong Size { get; set; }
		string Name { get; set; }
	}

	public interface IXboxModule
	{
		XBOX_MODULE_INFO xbox_module_info { get; }

		uint GetEntryPointAddress();
		void GetFunctionInfo(uint Address, out XBOX_FUNCTION_INFO FunctionInfo);

		IXboxExecutable Executable { get; }
		uint OriginalSize { get; }
		IXboxSections Sections { get; }
	}

	public interface IXboxMemoryRegions : IEnumerable
	{
		IXboxMemoryRegion xboxMemoryRegion { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface IXboxFiles : IEnumerable
	{
		IXboxFile xboxFile { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface IXboxEvents
	{
		void OnStdNotify(XboxDebugEventType EventCode, IXboxEventInfo EventInfo);
		void OnTextNotify(string Source, string Notification);
	}

	public interface IXboxMemoryRegion
	{
		int BaseAddress { get; }
		XboxMemoryRegionFlags Flags { get; }
		int RegionSize { get; }
	}

	public interface IXboxExecutable
	{
		string GetPEModuleName();
	}

	public interface IXboxExecutableInfo
	{
		string BasePath { get; set; }
		string ModuleName { get; }
		string PortableExecutablePath { get; }
		bool PropGetRelativePath { get; set; }
		string PublicSymbolPath { get; }
		uint SizeOfImage { get; }
		bool StoreRelativePath { get; }
		string SymbolGuid { get; }
		string SymbolPath { get; }
		uint TimeDateStamp { get; }
		string XboxExecutablePath { get; }
	}

	public interface IXboxModules : IEnumerable
	{
		IXboxModule XboxModule { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}

	public interface IXboxSection
	{
		XBOX_SECTION_INFO SectionInfo { get; }
	}

	public interface IXboxSections : IEnumerable
	{
		IXboxSection XboxSection { get; }

		new IEnumerator GetEnumerator();

		int Count { get; }
	}
	#endregion

	#region XNotify
	public class XNotify
	{
		public static IXboxConsole console;
		public static bool XNotifyEnabled
		{
			get
			{
				return XNotifyEnabled;
			}
			set
			{
				XNotifyEnabled = value;
			}
		}
		public static void Show(string Message, bool isON)
		{
			if (isON == true)
			{
				XNotifyEnabled = true;
			}
			if (XNotifyEnabled == true)
			{
				Show(XNotiyLogo.FLASHING_XBOX_LOGO, Message, isON);
			}
		}
		public static void Show(XNotiyLogo Type, string Message, bool isON)
		{
			if (isON == true)
			{
				XNotifyEnabled = true;
			}
			if (XNotifyEnabled == true)
			{
				object[] arguments = new object[] { 0x18, 0xff, 2, (Message).ToWCHAR(), (int)Type };
				console.CallVoid((int)ThreadType.Title, "xam.xex", 0x290, arguments);
			}
		}

	}
	#endregion
}
