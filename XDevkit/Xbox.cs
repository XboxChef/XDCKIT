using System;
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

namespace XDevkit
{
	#region Xbox Main Class
	/// <summary>
	/// Todo:
	/// </summary>
	public class Xbox //will handle connection and commands and will use XDevkit as a gateway for commands to be passed threw
	{

		/// <summary>
		/// Gets the main connection used for pc to xbox communication.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static TcpClient XboxName;
		public string IPAddresss;
		public DebugTarget DebugTarget = new DebugTarget();
		private StreamReader sreader;
		public static string IP;
		public Xbox()
		{

		}


		internal static void Connect(string DebuggerName, XboxDebugConnectFlags Flags)
		{
			if (Flags == XboxDebugConnectFlags.Force)
			{

			}
			else if (Flags == XboxDebugConnectFlags.MonitorOnly)
			{

			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool Connected;
		private string responses;
		private byte[] response_in_bytes;
		internal int connectionId;
		/// <summary>
		/// Retrieves actual xbox connection status. Average execution time of 3600 executions per second.
		/// </summary>
		/// <returns>Connection status</returns>
		public bool Ping()
		{
			return Ping(Timeout);
		}
		/// <summary>
		/// Disconnects from the xbox.
		/// </summary>
		public void Disconnect()
		{
			try
			{
				// attempt to clean up if still connected
				if (Ping())
				{
					SendCommand("bye"); // we cant leave without saying goodbye ;)
				}
			}
			catch { }
		}
		// todo: dont timeout if still receiving, currently it could timeout if receiving large information with small timeout...

		/// <summary>
		/// Waits for a specified amount of data to be received.  Use with file IO.
		/// </summary>
		/// <param name="targetLength">Amount of data to wait for</param>
		public void Wait(int targetLength)
		{
			if (XboxName != null)
			{
				if (XboxName.Available < targetLength) // avoid waiting if we already have data in our buffer...
				{
					Stopwatch sw = Stopwatch.StartNew();
					while (XboxName.Available < targetLength)
					{
						Thread.Sleep(0);
						if (sw.ElapsedMilliseconds > timeout)
						{
							if (!Ping(250)) Disconnect();  // only disconnect if actually disconnected
							throw new TimeoutException();
						}
					}
				}
			}
			else throw new NoConnectionException();
		}
		/// <summary>
		/// Waits for data to be received.  During execution this method will enter a spin-wait loop and appear to use 100% cpu when in fact it is just a suspended thread.  
		/// This is much more efficient than waiting a millisecond since most commands take fractions of a millisecond.
		/// It will either resume after the condition is met or throw a timeout exception.
		/// </summary>
		/// <param name="type">Wait type</param>
		public void Wait(WaitType type)
		{
			if (XboxName != null)
			{
				Stopwatch sw = Stopwatch.StartNew();
				switch (type)
				{
					// waits for data to start being received
					case WaitType.Partial:
						while (XboxName.Available == 0)
						{
							Thread.Sleep(0);
							if (sw.ElapsedMilliseconds > 5000)
							{
								if (!Ping(250)) Disconnect();  // only disconnect if actually disconnected
								throw new TimeoutException();
							}
						}
						break;

					// waits for data to start and then stop being received
					case WaitType.Full:

						// do a partial wait first
						while (XboxName.Available == 0)
						{
							Thread.Sleep(0);
							if (sw.ElapsedMilliseconds > 5000)
							{
								if (!Ping(250)) Disconnect();  // only disconnect if actually disconnected
								throw new TimeoutException();
							}
						}

						// wait for rest of data to be received
						int avail = XboxName.Available;
						Thread.Sleep(0);
						while (XboxName.Available != avail)
						{
							avail = XboxName.Available;
							Thread.Sleep(0);
						}
						break;

					// waits for data to stop being received
					case WaitType.Idle:
						int before = XboxName.Available;
						Thread.Sleep(0);
						while (XboxName.Available != before)
						{
							before = XboxName.Available;
							Thread.Sleep(0);
							if (sw.ElapsedMilliseconds > 5000)
							{
								if (!Ping(250)) Disconnect();  // only disconnect if actually disconnected
								throw new TimeoutException();
							}
						}
						break;
				}
			}
			else throw new NoConnectionException();
		}
		/// <summary>
		/// Gets or sets the maximum waiting time given (in milliseconds) for a response.
		/// </summary>
		[Browsable(false)]
		public int Timeout { get { return timeout; } set { timeout = value; } }
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int timeout = 5000;
		/// <summary>
		/// Waits for a specified amount and then flushes it from the socket buffer.
		/// </summary>
		/// <param name="size">Size to flush</param>
		public void FlushSocketBuffer(int size)
		{
			if (size > 0)
			{
				Wait(size);
				try
				{
					XboxName.Client.Receive(new byte[size]);
				}
				catch { connected = false; }
			}
		}
		/// <summary>
		/// Retrieves actual xbox connection status. Average execution time of 3600 executions per second.
		/// </summary>
		/// <param name="waitTime">Time to wait for a response</param>
		/// <returns>Connection status</returns>
		public bool Ping(int waitTime)
		{
			int oldTimeOut = timeout;
			try
			{
				if (XboxName != null)
				{
					if (XboxName.Available > 0)
						XboxName.Client.Receive(new byte[XboxName.Available]);

					XboxName.Client.Send(ASCIIEncoding.ASCII.GetBytes(Environment.NewLine));
					timeout = waitTime;
					FlushSocketBuffer(16);    // throw out garbage response "400- Unknown Command\r\n"
					connected = true;
					return true;
				}
				return false;
			}
			catch
			{
				connected = false;
				XboxName.Close();
				return false;
			}
			finally
			{
				timeout = oldTimeOut;   // make sure to restore old timeout
			}
		}
		/// <summary>
		/// Gets the current connection status known to Yelo.Debug.  For an actual status update you need to Ping() the xbox.
		/// </summary>
		[Browsable(false)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public bool connected = false;
		/// <summary>
		/// Waits for the receive buffer to stop receiving, then clears it.
		/// Call this before you send anything to the xbox to help keep the channel in sync.
		/// </summary>
		public void FlushSocketBuffer()
		{
			Wait(WaitType.Idle);    // waits for the link to be idle...
			try
			{
				if (XboxName.Available > 0)
					XboxName.Client.Receive(new byte[XboxName.Available]);
			}
			catch { connected = false; }
		}
		/// <summary>
		/// Sends a command to the xbox.
		/// </summary>
		/// <param name="command">Command to be sent</param>
		/// <param name="args">Arguments</param>
		/// <returns>Status response</returns>
		public StatusResponse SendCommand(string command, params object[] args)
		{
			if (XboxName != null)
			{
				FlushSocketBuffer();

				try
				{
					XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format(command, args) + Environment.NewLine));
				}
				catch (Exception /*ex*/)
				{
					Disconnect();
					throw new NoConnectionException();
				}

				StatusResponse response = ReceiveStatusResponse();

				if (response.Success) return response;
				else throw new ApiException(response.Full);
			}
			else throw new NoConnectionException();
		}
		public bool Connect(string XboxIP,string XboxNameOrIP = "default")
		{
			try
			{
				if(XboxNameOrIP == "defualt")
				{

				}
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
		/// <summary>
		/// Waits for a status response to be received from the xbox.
		/// </summary>
		/// <returns>Status response</returns>
		public StatusResponse ReceiveStatusResponse()
		{
			if (XboxName != null)
			{
				string response = ReceiveSocketLine();
				return new StatusResponse(response, (ResponseType)Convert.ToInt32(response.ToString().Remove(3)), response.Remove(0, 5).ToString());
			}
			else throw new NoConnectionException();
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
		/// Gets or sets the xbox notification port.
		/// </summary>
		[Browsable(false)]
		public int NotificationPort { get { return notificationPort; } set { notificationPort = value; } }

		public uint ConnectTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public uint ConversationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public void CloseConnection(uint Connection)
		{
			SendTextCommand("bye");
			XboxName.Close();
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

		//public IXboxFiles DirectoryFiles(string Directory)
		//{
		//	//"DIRLIST NAME="Directory"

		//	return null;
		//}

		public void FindConsole(uint Retries, uint RetryDelay)// Todo: Add a Max And Minimal retry system.
		{

			Retries = 0;
			do
			{
				try
				{
					Retries++;
					ConsoleFinder();
					break; // Sucess! Lets exit the loop!
				}
				catch (Exception)
				{

					Task.Delay(4).Wait();
				}
			} while (true);

		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int notificationPort = 731;
		/// <summary>
		/// Shortcuts To Guide The Xbox Very Fast
		/// </summary>
		/// <param name="Color"></param>
		public void XboxShortcut(XboxShortcuts UI)
		{
			Xbox Xconsole = new Xbox();
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
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Account_Management), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Achievements:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Achievements), new object[] { 0, 0, 0, 0 });//achievements
						break;
					case (int)XboxShortcuts.Active_Downloads:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Active_Downloads), new object[] { 0, 0, 0, 0 });//XamShowMarketplaceDownloadItemsUI
						break;
					case (int)XboxShortcuts.Awards:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Awards), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Beacons_And_Activiy:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Beacons_And_Activiy), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Family_Settings:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Family_Settings), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Friends:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Friends), new object[] { 0, 0, 0, 0 });//friends
						break;
					case (int)XboxShortcuts.Guide_Button:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Guide_Button), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Messages:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Messages), 0);//messages tab
						break;
					case (int)XboxShortcuts.My_Games:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.My_Games), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Open_Tray:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Open_Tray), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Party:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Party), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Preferences:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Preferences), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Private_Chat:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Private_Chat), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Profile:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Profile), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Recent:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Recent), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Redeem_Code:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Redeem_Code), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Select_Music:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Select_Music), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.System_Music_Player:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Music_Player), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.System_Settings:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Settings), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.System_Video_Player:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Video_Player), new object[] { 0, 0, 0, 0 });
						break;
					case (int)XboxShortcuts.Windows_Media_Center:
						Xconsole.CallVoid(Xconsole.ResolveFunction("xam.xex", (int)XboxShortcuts.Windows_Media_Center), new object[] { 0, 0, 0, 0 });
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
			if (XboxName == null)
			{

				Console.WriteLine("Console Is Not Connnected...");
			}
			else
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
							SendTextCommand(string.Concat("consoletype"), out responses);
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
								responses = s.Substring(Start, End);
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
			SendTextCommand("drivefreespace name=\"{0}\"" + drive.ToString() + ":\\");

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
		//[DllImport("xbdm.dll")]
		//public extern DmScreenShot(string filename);



		public Image Screenshot(string filename)
		{
			SendTextCommand("screenshot", out response_in_bytes);
			return (Bitmap)((new ImageConverter()).ConvertFrom(response_in_bytes));
		}
		public Image Screenshot()
		{
			try
			{
				if (XboxName.Connected)
				{
					SendTextCommand("screenshot", out response_in_bytes);
					return (Bitmap)((new ImageConverter()).ConvertFrom(response_in_bytes));
				}

			}
			catch
			{

			}
			return Image.FromFile(@"error.jpg");
		}
		public string Drives
		{
			get
			{
				string responses;
				SendTextCommand(string.Concat("drivelist"), out responses);
				return responses.Replace("200- drivelist=", string.Empty);
			}
		}
		public int ReceiveStatusResponse(uint Connection, out string Line)//get response codes here 
		{
			Line = null;
			return 0;
		}
		public XboxFile GetFileObject(string Filename)
		{
			return null;
		}
		public void SendBinary(uint connectionId, byte[] callData, uint length)
		{

		}
		public uint IPAddress
		{
			get
			{
				return Convert.ToUInt32(IP);
			}
		}

		public string Name { get; }

		public void ReceiveBinary(uint connectionId, byte[] numArray, uint length, out uint bytesReceived)
		{
			bytesReceived = 0;
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
			GetDriveInformation(Drive, out FreeBytesAvailableToCaller, out TotalNumberOfBytes, out TotalNumberOfFreeBytes);
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

		
		/// <summary>
		/// Gets or sets the xbox system time.
		/// </summary>
		public /*unsafe*/ DateTime SystemTime
		{
			get
			{
				StatusResponse response = SendCommand("systime");
				if (response.Type == ResponseType.SingleResponse)
				{
					string ticks = string.Format("0x{0}{1}",
						response.Message.Substring(7, 7),
						response.Message.Substring(21).PadLeft(8, '0')
						);
					return DateTime.FromFileTime(Convert.ToInt64(ticks, 16));
				}
				else throw new ApiException("Failed to get xbox system time.");
			}
			set
			{

				long fileTime = value.ToFileTimeUtc();
				int lo = (int)(fileTime & 0xFFFFFFFF); // *(int*)&fileTime;
				int hi = (int)(((ulong)fileTime & 0xFFFFFFFF00000000UL) >> 32);// *((int*)&fileTime + 1);

				StatusResponse response = SendCommand(string.Format("setsystime clockhi=0x{0} clocklo=0x{1} tz=1", Convert.ToString(hi, 16), Convert.ToString(lo, 16)));
				if (response.Type != ResponseType.SingleResponse)
					throw new ApiException("Failed to set xbox system time.");
			}
		}

		public void SetMemory(uint Address, byte[] Data)
		{
			uint num;
			DebugTarget.SetMemory(Address, (uint)Data.Length, Data, out num);
			throw new NotImplementedException();
		}

		public byte[] GetMemory(uint Address, uint Length)
		{
			uint bytesRead = 0;
			byte[] data = new byte[Length];
			DebugTarget.GetMemory(Address, Length, data, out bytesRead);
			//DebugTarget.InvalidateMemoryCache(true, Address, Length);// ADD This Method
			return data;
		}
		public void WriteUInt32(uint Address, uint Value)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			ReverseBytes(bytes, 4);
			SetMemory(Address, bytes);
		}


		public void WriteUInt32(uint Address, uint[] Value)
		{
			byte[] array = new byte[Value.Length * 4];
			for (int i = 0; i < Value.Length; i++)
			{
				BitConverter.GetBytes(Value[i]).CopyTo(array, (int)(i * 4));
			}
			ReverseBytes(array, 4);
			SetMemory(Address, array);
		}

		public void WriteByte(uint Address, byte Value)
		{
			byte[] data = new byte[] { Value };
			SetMemory(Address, data);
		}


		public void WriteByte(uint Address, byte[] Value)
		{
			SetMemory(Address, Value);
		}
		public void WriteFloat(uint Address, float Value)
		{
			byte[] bytes = BitConverter.GetBytes(Value);
			Array.Reverse(bytes);
			SetMemory(Address, bytes);
		}


		public void WriteFloat(uint Address, float[] Value)
		{
			byte[] array = new byte[Value.Length * 4];
			for (int i = 0; i < Value.Length; i++)
			{
				BitConverter.GetBytes(Value[i]).CopyTo(array, (int)(i * 4));
			}
			ReverseBytes(array, 4);
			SetMemory(Address, array);
		}
		private static void ReverseBytes(byte[] buffer, int groupSize)
		{
			if ((buffer.Length % groupSize) != 0)
			{
				throw new ArgumentException("Group size must be a multiple of the buffer length", "groupSize");
			}
			int num = 0;
			while (num < buffer.Length)
			{
				int index = num;
				int num3 = (num + groupSize) - 1;
				while (true)
				{
					if (index >= num3)
					{
						num += groupSize;
						break;
					}
					byte num4 = buffer[index];
					buffer[index] = buffer[num3];
					buffer[num3] = num4;
					index++;
					num3--;
				}
			}
		}




	}
	/// <summary>
	/// Xbox command status response.
	/// </summary>
	public class StatusResponse
	{
		public string Full { get; private set; }
		public ResponseType Type { get; private set; }
		public string Message { get; private set; }
		public bool Success { get { return ((int)Type & 200) == 200; } }

		public StatusResponse(string full, ResponseType type, string message)
		{
			this.Full = full;
			this.Type = type;
			this.Message = message;
		}
	};
	#endregion
}
