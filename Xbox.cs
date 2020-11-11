using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace XDevkit
{
    /// <summary>
    /// Credits To JRPC Dev Class also Yelo debug  and PeekPoker. the Rest Is Created By Me TeddyHammer
    /// </summary>
    public class Xbox
    {

        public Xbox()
        {

        }

        #region Property's
        private bool ValidConnection;
        private RwStream _readWriter;
        private uint _startDumpOffset;
        private bool _stopSearch;
        /// <summary>
        /// Gets or sets the maximum waiting time given (in milliseconds) for a response.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Timeout { get => 5000; set => Timeout = value; }
        private const string Connection_Error = "Console Not Connected";
        public XBOX_PROCESS_INFO RunningProcessInfo;

        public string Name { get; set; }
        public string IPAddress { get; set; }
        public static TcpClient XboxName;
        public StreamReader sreader;
        public string responses;

        public bool Connected { get; set; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string xdkRegistryPath = @"HKEY_CURRENT_USER\Software\Microsoft\XboxSDK";
        public int ConnectTimeout { get => XboxName.ReceiveTimeout; set => XboxName.ReceiveTimeout = value; }
        public int ConversationTimeout { get => XboxName.SendTimeout; set => XboxName.SendTimeout = value; }
        /// <summary>Set or Get the start dump offset</summary>
        public uint DumpOffset
        {
            set { _startDumpOffset = value; }
        }

        /// <summary>Set or Get the dump length</summary>
        public uint DumpLength { set; get; }

        /// <summary>
        /// Stop any searching
        /// </summary>
        public bool StopSearch
        {
            get
            {
                if (!_readWriter.Accessed) return false;
                return _readWriter.StopSearch;
            }
            set
            {
                if (!_readWriter.Accessed) return;
                _readWriter.StopSearch = value;
                _stopSearch = value;
            }
        }

        /// <summary>
        /// Gets or sets the last xbox connection used.
        /// </summary>
        [Browsable(false)]
        public string LastConnectionUsed
        {
            get { return (string)Microsoft.Win32.Registry.GetValue(xdkRegistryPath, "XboxName", string.Empty); }
            set { Microsoft.Win32.Registry.SetValue(xdkRegistryPath, "XboxName", value); }
        }



        #endregion

        #region Networking
        /// <summary>Connect to the  using port 730 using the given ip address</summary>
        /// <returns>True if connection was successful and False if not</returns>
        public bool CheckConnection()
        {
            try
            {
                if (IPAddress.Length < 5)
                    throw new Exception("Invalid IP");
                if (Connected) return true; //If you are already connected then return
                var response = new byte[1024];
                XboxName.Client.Receive(response);
                string reponseString = Encoding.ASCII.GetString(response).Replace("\0", string.Empty);
                //validate connection
                Connected = reponseString.Substring(0, 3) == "201";

                return Connected;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// looks up local addresses on the network to find console ip
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsLocalIpAddress(string host)
        {
            try
            { // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (System.Net.IPAddress.IsLoopback(hostIP)) return true;
                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }
        public static bool PingHost(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new TcpClient(hostUri, portNumber))
                    return true;
            }
            catch (SocketException)
            {
                MessageBox.Show("Error pinging host:'" + hostUri + ":" + portNumber.ToString() + "'");
                return false;
            }
        }
        public void OpenConsole(string XboxNameOrIP)
        {
            Connect(XboxNameOrIP);
        }
        /// <summary>
        /// Connects to an xbox on the network. If console is detected this method 
        /// will attempt to connect to the console.
        /// </summary>

        /// <summary>
        /// Connects To The Console Via Tcp Connection. 
        /// </summary>
        public bool Connect(string XboxNameOrIP = "defualt")
        {
            if (XboxNameOrIP == string.Empty)
            {
                XboxNameOrIP = "defualt";
            }
            try
            {
                if (XboxNameOrIP == "defualt")
                {
                    bool sw = true;

                    while (sw)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            XboxName = new TcpClient();
                            if (!XboxName.ConnectAsync("192.168.0." + i, 730).Wait(10))
                            {
                                Connected = Ping(10);  // make sure it is successful


                            }
                            else if (Ping() == true)
                            {

                                sw = false;
                                string IPAddress = "192.168.0." + i;
                                XboxName = new TcpClient(IPAddress, 730);
                                sreader = new StreamReader(XboxName.GetStream());
                                Console.WriteLine("Connected == ( Method 1 )");
                                XNotify.Show("Connected!",XNotiyLogo.FLASHING_HAPPY_FACE);
                                return Connected = true;
                            }


                        }
                    }


                }
                else if (XboxNameOrIP.Contains("192"))
                {
                    string IPAddress = XboxNameOrIP;
                    XboxName = new TcpClient(XboxNameOrIP, 730);
                    sreader = new StreamReader(XboxName.GetStream());

                    Connected = Ping(100);  // make sure it is successful
                    if (Ping() == false)
                    {
                        return Connected = false;
                    }
                    else if (Ping() == true)
                    {
                        Console.WriteLine("Connected == ( Method 2 )");
                        StreamWriter sW = new StreamWriter(XboxName.GetStream());
                        sW.AutoFlush = true;

                        //get's the pc's connection and port
                        Console.WriteLine("Xbox Connected to " + XboxName.Client.LocalEndPoint.ToString());
                        return Connected = true;
                    }
                }

                return Connected = false;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return Connected = false;
            }
        }



        public void CloseConnection(uint Connection)
        {
            SendTextCommand("bye");
            sreader.Close();
            XboxName.Close();
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
                    CloseConnection(0);

                }
            }
            catch { }
        }
        #endregion

        #region SendCommands
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="response"></param>
        public void SendTextCommand(string Command, out string response)
        {
            response = string.Empty;
            if (XboxName == null)
            {
                Console.WriteLine("SendTextCommand ==> " + Assembly.GetEntryAssembly().GetName().Name + " Connection == null <==");
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
                        FlushSocketBuffer();
                    Console.WriteLine("SendTextCommand ==> Sending Command... <==");
                    XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
                    XboxName.Client.Receive(Packet);
                    response = Encoding.ASCII.GetString(Packet);
                }
                catch
                {

                }

        }

        /// <summary>
        /// Get's Box Id.
        /// </summary>
        /// <param name="fileName">File to delete.</param>
        public string GetBoxID()
        {

            SendTextCommand("BOXID", out responses);
            return responses.Replace("200- ", string.Empty);
        }
        /// <summary>
        /// Turns The Console's Default Neighborhood Icon to any of the following...(black , blue , bluegray , nosidecar , white)
        /// Also Changes The Type Of Console It Is.
        /// </summary>
        /// <param name="Color"></param>
        public void SetConsoleColor(XboxColor Color)
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
            return responses.Replace("200- consoleid=", string.Empty);
        }
        /// <summary>
        /// Gets the debug Monitor version Number.
        /// </summary>
        public string GetDMVersion()
        {
            SendTextCommand("dmversion", out responses);
            return responses.Replace("200- ", string.Empty);
        }
        /// <summary>
        /// Get's Consoles System Information.
        /// </summary>
        /// <param name="Type"></param>
        /// <returns>Type Is The System Type Of Information you Want To Retrieve</returns>
        public string GetSystemInfo(Info Type)
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
                            return responses.Replace("200- ", string.Empty);
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
                                responses = s.Substring(Start + 9, End - 1).Substring(Start);
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
                                int End = s.IndexOf(" p");
                                responses = s.Substring(Start + End + 4, End - 4).Substring(Start);
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

        #endregion

        #region Features
        /// <summary>
        /// Gets A Float From Address And Returns it as String.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReadFloat(string text)
        {
            try
            {
                Console.WriteLine("ReadFloat Was Passed Threw Returning float to String");
                return GetFloat(0x8 + uint.Parse(text.Substring(1))).ToString();

            }
            catch
            {
                Console.WriteLine("ReadFloat Failed Sending Empty String");
                return string.Empty;
            }

        }
        public byte[] NOPBytes()
        {
            return new byte[] { 0x60, 0x00, 0x00, 0x00 };
        }
        public static void X360Text(string a)
        {

            object[] arguments = new object[] { 0x14/*Type*/, 0xff, 2, (a).ToWCHAR(), 1 };
            JRPC.CallVoid(ThreadType.Title, "xam.xex", 0x290, arguments);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="MediaDirectory"></param>
        /// <param name="CmdLine"></param>
        /// <param name="Flags"></param>
        public void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags)
        {
            string[] lines = Name.Split("\\".ToCharArray());
            for (int i = 0; i < lines.Length - 1; i++)
                MediaDirectory += lines[i] + "\\";
            object[] Reboot = new object[] { $"magicboot title=\"{Name}\" directory=\"{MediaDirectory}\"" };//todo
            SendTextCommand(string.Concat(Reboot));
        }
        /// <summary>
        /// Shortcuts To Guide
        /// </summary>
        /// <param name="Color"></param>
        public void XboxShortcut(XboxShortcuts UI)
        {
            if (XboxName.Connected)
                switch ((int)UI)//works by getting the int of the UI and matches the numbers to execute things
                {
                    case (int)XboxShortcuts.XboxHome:
                        Reboot(@"\Device\Harddisk0\SystemExtPartition\20445100\dash.xex", @"\Device\Harddisk0\SystemExtPartition\20445100\", @"\Device\Harddisk0\SystemExtPartition\20445100\", XboxRebootFlags.Title);
                        break;
                    case (int)XboxShortcuts.Turn_Off_Console:
                        ShutDownConsole();
                        break;
                    case (int)XboxShortcuts.Account_Management:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Account_Management), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Achievements:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Achievements), new object[] { 0, 0, 0, 0 });//achievements
                        break;
                    case (int)XboxShortcuts.Active_Downloads:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Active_Downloads), new object[] { 0, 0, 0, 0 });//XamShowMarketplaceDownloadItemsUI
                        break;
                    case (int)XboxShortcuts.Awards:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Awards), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Beacons_And_Activiy:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Beacons_And_Activiy), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Family_Settings:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Family_Settings), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Friends:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Friends), new object[] { 0, 0, 0, 0 });//friends
                        break;
                    case (int)XboxShortcuts.Guide_Button:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Guide_Button), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Messages:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Messages), 0);//messages tab
                        break;
                    case (int)XboxShortcuts.My_Games:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.My_Games), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Open_Tray:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Open_Tray), new object[] { 0, 0, 0, 0 });

                        break;
                    case (int)XboxShortcuts.Party:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Party), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Preferences:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Preferences), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Private_Chat:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Private_Chat), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Profile:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Profile), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Recent:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Recent), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Redeem_Code:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Redeem_Code), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Select_Music:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Select_Music), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.System_Music_Player:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.System_Music_Player), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.System_Settings:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.System_Settings), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.System_Video_Player:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.System_Video_Player), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Windows_Media_Center:
                        JRPC.CallVoid(ResolveFunction("xam.xex", (int)XboxShortcuts.Windows_Media_Center), new object[] { 0, 0, 0, 0 });
                        break;

                }
        }
        /// <summary>
        /// Reboot Method flag types cold or warm reboot.
        /// </summary>
        public void Reboot(XboxReboot Warm_or_Cold)
        {
            if (Warm_or_Cold == XboxReboot.Cold)
            {
                SendTextCommand("magicboot cold");
            }
            if (Warm_or_Cold == XboxReboot.Warm)
            {
                SendTextCommand("magicboot warm");
            }
        }
        /// <summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ConsoleType()
        {
            string str = string.Concat("consolefeatures ver=", 2, " type=17 params=\"A\\0\\A\\0\\\"");
            string str1 = SendTextCommand(str);
            return str1.Substring(str1.find(" ") + 1);
        }
        /// <summary>
        /// Retrieve's The Console's Central Processing Unit Key.
        /// </summary>
        public string GetCPUKey()
        {
            string str = string.Concat("consolefeatures ver=", 2, " type=10 params=\"A\\0\\A\\0\\\"");
            string str1 = SendTextCommand(str);
            return responses.Replace("200- ", string.Empty);
        }
        /// <summary>
        /// Version Of Kernal 
        /// </summary>
        /// <returns></returns>
        public uint GetKernalVersion()
        {
            string str = string.Concat("consolefeatures ver=", 2, " type=13 params=\"A\\0\\A\\0\\\"");
            string str1 = SendTextCommand(str);
            return uint.Parse(str1.Substring(str1.find(" ") + 1));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TemperatureType"></param>
        /// <returns></returns>
        public uint GetTemperature(TemperatureFlag TemperatureType)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", 2, " type=15 params=\"A\\0\\A\\1\\", 1, "\\", (int)TemperatureType, "\\\"" };
            string str = SendTextCommand(string.Concat(jRPCVersion));
            return uint.Parse(str.Substring(str.find(" ") + 1), NumberStyles.HexNumber);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Top_Left"></param>
        /// <param name="Top_Right"></param>
        /// <param name="Bottom_Left"></param>
        /// <param name="Bottom_Right"></param>
        public void SetLeds(LEDState Top_Left, LEDState Top_Right, LEDState Bottom_Left, LEDState Bottom_Right)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", 2, " type=14 params=\"A\\0\\A\\4\\", 1, "\\", (uint)Top_Left, "\\", 1, "\\", (uint)Top_Right, "\\", 1, "\\", (uint)Bottom_Left, "\\", 1, "\\", (uint)Bottom_Right, "\\\"" };
            SendTextCommand(string.Concat(jRPCVersion));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public uint XamGetCurrentTitleId()
        {
            string str = string.Concat("consolefeatures ver=", 2, " type=16 params=\"A\\0\\A\\0\\\"");
            string str1 = SendTextCommand(str);
            return uint.Parse(str1.Substring(str1.find(" ") + 1), NumberStyles.HexNumber);
        }
        /// <summary>
        /// Turns Off Console.
        /// </summary>
        public void ShutDownConsole()
        {
            try
            {
                string str = string.Concat("consolefeatures ver=", 2, " type=11 params=\"A\\0\\A\\0\\\"");
                SendTextCommand(str);
            }
            catch
            {
            }
        }
        #endregion

        #region MemoryEdits {Get; Set;}
        private bool GetMeMex()
        {
            return GetMemory2(_startDumpOffset, DumpLength);
        }

        private bool GetMemory2(uint Address, uint length)
        {
            if (ValidConnection) return true;
            //ADDR=0xDA1D0000 - The start offset in the physical memory I want the dump to start
            //LENGTH = Length of the dump
            XboxName.Client.Send(
                Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR={0} LENGTH={1}\r\n", Address, length)));
            var response = new byte[1024];
            XboxName.Client.Receive(response);
            string reponseString = Encoding.ASCII.GetString(response).Replace("\0", string.Empty);
            //validate connection
            ValidConnection = reponseString.Substring(0, 3) == "203";
            return ValidConnection;
        }
        public void constantMemorySet(uint Address, uint Value)
        {
            constantMemorySetting(Address, Value, false, 0, false, 0);
        }

        public void constantMemorySet(uint Address, uint Value, uint TitleID)
        {
            constantMemorySetting(Address, Value, false, 0, true, TitleID);
        }

        public void constantMemorySet(uint Address, uint Value, uint IfValue, uint TitleID)
        {
            constantMemorySetting(Address, Value, true, IfValue, true, TitleID);
        }
        public void constantMemorySetting(uint Address, uint Value, bool useIfValue, uint IfValue, bool usetitleID, uint TitleID)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", 2, " type=18 params=\"A\\", Address.ToString("X"), "\\A\\5\\", 1, "\\", Functions.UIntToInt(Value), "\\", 1, "\\", (useIfValue ? 1 : 0), "\\", 1, "\\", IfValue, "\\", 1, "\\", (usetitleID ? 1 : 0), "\\", 1, "\\", Functions.UIntToInt(TitleID), "\\\"" };
            SendTextCommand(string.Concat(jRPCVersion));
        }
        public void SetMemory(uint address, string data)
        {
            int sent = 0;
            try
            {
                // Send the setmem command
                XboxName.Client.Send(
                    Encoding.ASCII.GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n", address.ToString("X2"), data)));
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    // socket buffer is probably full, wait and try again
                    Thread.Sleep(30);
                }
                else
                    throw new Exception(ex.Message + " - " + sent); // any serious error occurr
            }
        }
        public void SetMemory(uint Address, byte[] Data)
        {
            SetMemory(Address, 0, Data, out _);
        }
        public void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten)//aka response
        {

            // Send the setmem command
            XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n", Address.ToString("X2"), BitConverter.ToString(Data).Replace("-", string.Empty))));

            // Check to see our response
            byte[] Packet = new byte[1026];
            XboxName.Client.Receive(Packet);
            BytesWritten = 0;
            //BytesWritten = Convert.ToUInt32(Encoding.ASCII.GetString(Packet));
            if (Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Substring(0, 11) == "0 bytes set")
                throw new Exception("A problem occurred while writing bytes. 0 bytes set");
        }
        public byte[] GetMemory(uint Address, uint Length)
        {

            {
                uint num = 0;
                byte[] numArray = new byte[Length];
                GetMemory(Address, Length, numArray, out num);
                InvalidateMemoryCache(true, Address, Length);
                return numArray;
            }
        }
        public void GetMemory(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead)
        {
            BytesRead = 0;
            List<byte> ReturnData = new List<byte>();
            byte[] Packet = new byte[1026];
            Data = new byte[1024];

            // Send getmemex command.

            XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), BytesToRead.ToString("X2"))));

            // Receieve the 203 response to verify we are going to recieve raw data in packets
            XboxName.Client.Receive(Packet);

            if (Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Substring(0, 3) != "203")
                throw new Exception("GETMEMEX 203 response not recieved. Cannot read memory.");

            // It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
            // Length / 1024 will get how many packets there are to recieve
            for (uint i = 0; i < BytesToRead / 1024; i++)
            {
                XboxName.Client.Receive(Packet);

                // Store the data minus the first two bytes
                // This was a cheap way of removing the 2 byte header
                Array.Copy(Packet, 2, Data, 0, 1024);
                ReturnData.AddRange(Data);
            }
        }
        /// <summary>
        /// Dump the memory
        /// </summary>
        /// <param name="filename">The file to save to</param>
        /// <param name="startDumpAddress">The start dump address</param>
        /// <param name="dumpLength">The dump length</param>
        public void Dump(string filename, string startDumpAddress, string dumpLength)
        {
            Dump(filename, Functions.Convert(startDumpAddress), Functions.Convert(dumpLength));
        }

        /// <summary>
        /// Dump the memory
        /// </summary>
        /// <param name="filename">The file to save to</param>
        /// <param name="startDumpAddress">The start dump address</param>
        /// <param name="dumpLength">The dump length</param>
        public void Dump(string filename, uint startDumpAddress, uint dumpLength)
        {
            if (!CheckConnection()) return; //Call function - If not connected return
            if (!GetMemory2(startDumpAddress, dumpLength))
                return; //call function - If not connected or if something wrong return

            var readWriter = new RwStream(filename);
            try
            {
                var data = new byte[1026]; //byte chuncks
                //Writing each byte chuncks========
                for (int i = 0; i < dumpLength / 1024; i++)
                {
                    XboxName.Client.Receive(data);
                    readWriter.WriteBytes(data, 2, 1024);
                }
                //Write whatever is left
                var extra = (int)(dumpLength % 1024);
                if (extra > 0)
                {
                    XboxName.Client.Receive(data);
                    readWriter.WriteBytes(data, 2, extra);
                }
                readWriter.Flush();
            }
            catch (SocketException)
            {
                readWriter.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                readWriter.Close(false);
                XboxName.Close(); //close connection
                Connected = false;
                ValidConnection = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ModuleName"></param>
        /// <param name="Ordinal"></param>
        /// <returns></returns>
        public uint ResolveFunction(string ModuleName, uint Ordinal)
        {
            object[] XBDMVersion = new object[] { "consolefeatures ver=", 2/*THTVersion*/, " type=9 params=\"A\\0\\A\\2\\", /*String*/0, "/", ModuleName.Length, "\\", ModuleName.ToHexString(), "\\", /*Int*/0, "\\", Ordinal, "\\\"" };
            string str = SendTextCommand(string.Concat(XBDMVersion));
            return uint.Parse(str.Substring(str.find(" ") + 1), NumberStyles.HexNumber);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="groupSize"></param>
        private void ReverseBytes(byte[] buffer, int groupSize)
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
        #endregion

        #region PeekPoker
        #region Methods


        /// <summary>Poke the Memory</summary>
        /// <param name="memoryAddress">The memory address to Poke Example:0xCEADEADE - Uses *.FindOffset</param>
        /// <param name="value">The value to poke Example:000032FF (hex string)</param>
        public void Poke(string memoryAddress, string value)
        {
            Poke(Functions.Convert(memoryAddress), value);
        }

        /// <summary>Poke the Memory</summary>
        /// <param name="memoryAddress">The memory address to Poke Example:0xCEADEADE - Uses *.FindOffset</param>
        /// <param name="value">The value to poke Example:000032FF (hex string)</param>
        public void Poke(uint memoryAddress, string value)
        {
            if (!Functions.IsHex(value))
                throw new Exception("Not a valid Hex String!");
            if (!CheckConnection()) return; //Call function - If not connected return
            try
            {
                SetMemory(memoryAddress, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                XboxName.Close(); //close connection
                Connected = false;
            }
        }

        /// <summary>Peek into the Memory</summary>
        /// <param name="startDumpAddress">The Hex offset to start dump Example:0xC0000000 </param>
        /// <param name="dumpLength">The Length or size of dump Example:0xFFFFFF </param>
        /// <param name="memoryAddress">The memory address to peek Example:0xC5352525 </param>
        /// <param name="peekSize">The byte size to peek Example: "0x4" or "4"</param>
        /// <returns>Return the hex string of the value</returns>
        public string Peek(string startDumpAddress, string dumpLength, string memoryAddress, string peekSize)
        {
            return Peek(Functions.Convert(startDumpAddress), Functions.Convert(dumpLength), Functions.Convert(memoryAddress), Functions.ConvertSigned(peekSize));
        }

        /// <summary>Peek into the Memory</summary>
        /// <param name="startDumpAddress">The Hex offset to start dump Example:0xC0000000 </param>
        /// <param name="dumpLength">The Length or size of dump Example:0xFFFFFF </param>
        /// <param name="memoryAddress">The memory address to peek Example:0xC5352525 </param>
        /// <param name="peekSize">The byte size to peek Example: "0x4" or "4"</param>
        /// <returns>Return the hex string of the value</returns>
        private string Peek(uint startDumpAddress, uint dumpLength, uint memoryAddress, int peekSize)
        {
            uint total = (memoryAddress - startDumpAddress);
            if (memoryAddress > (startDumpAddress + dumpLength) || memoryAddress < startDumpAddress)
                throw new Exception("Memory Address Out of Bounds");

            if (!CheckConnection()) return null; //Call function - If not connected return
            if (!GetMemory2(startDumpAddress, dumpLength))
                return null; //call function - If not connected or if somethign wrong return

            var readWriter = new RwStream();
            try
            {
                var data = new byte[1026]; //byte chuncks

                //Writing each byte chuncks========
                for (int i = 0; i < dumpLength / 1024; i++)
                {
                    XboxName.Client.Receive(data);
                    readWriter.WriteBytes(data, 2, 1024);
                }
                //Write whatever is left
                var extra = (int)(dumpLength % 1024);
                if (extra > 0)
                {
                    XboxName.Client.Receive(data);
                    readWriter.WriteBytes(data, 2, extra);
                }
                readWriter.Flush();
                readWriter.Position = total;
                byte[] value = readWriter.ReadBytes(peekSize);
                return Functions.ToHexString(value);
            }
            catch (SocketException se)
            {
                readWriter.Flush();
                readWriter.Position = total;
                byte[] value = readWriter.ReadBytes(peekSize);
                return Functions.ToHexString(value);
                throw new Exception(se.Message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                readWriter.Close(true);
                XboxName.Close(); //close connection
                Connected = false;
                ValidConnection = false;
            }
        }

        /// <summary>
        /// Find pointer offset
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BindingList<SearchResults> FindHexOffset(string pointer)
        {
            _stopSearch = false;
            if (pointer == null)
                throw new Exception("Empty Search string!");
            if (!Functions.IsHex(pointer))
                throw new Exception(string.Format("{0} is not a valid Hex string.", pointer));
            if (!CheckConnection()) return null; //Call function - If not connected return
            if (!GetMeMex()) return null; //call function - If not connected or if something wrong return
            BindingList<SearchResults> values;
            try
            {
                //LENGTH or Size = Length of the dump
                uint size = DumpLength;
                _readWriter = new RwStream();
                var data = new byte[1026]; //byte chuncks

                //Writing each byte chuncks========
                //No need to mess with it :D
                for (int i = 0; i < size / 1024; i++)
                {
                    if (_stopSearch)
                        return new BindingList<SearchResults>();
                    XboxName.Client.Receive(data);
                    _readWriter.WriteBytes(data, 2, 1024);
                }
                //Write whatever is left
                var extra = (int)(size % 1024);
                if (extra > 0)
                {
                    if (_stopSearch)
                        return new BindingList<SearchResults>();
                    XboxName.Client.Receive(data);
                    _readWriter.WriteBytes(data, 2, extra);
                }
                _readWriter.Flush();
                //===================================
                //===================================
                if (_stopSearch)
                    return new BindingList<SearchResults>();
                _readWriter.Position = 0;
                values = _readWriter.SearchHexString(Functions.StringToByteArray(pointer),
                                                     _startDumpOffset);
                return values;
            }
            catch (SocketException)
            {
                _readWriter.Flush();
                //===================================
                //===================================
                if (_stopSearch)
                    return new BindingList<SearchResults>();
                _readWriter.Position = 0;
                values = _readWriter.SearchHexString(Functions.StringToByteArray(pointer),
                                                     _startDumpOffset);

                return values;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _readWriter.Close(true);
                XboxName.Close(); //close connection
                Connected = false;
                ValidConnection = false;
            }
        }
        #endregion
        #endregion

        #region Yelo debug stuff







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
            _ = Stopwatch.StartNew();
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
                        if (sw.ElapsedMilliseconds > 5000)
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
            catch { Connected = false; }
        }
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
                catch { Connected = false; }
            }
        }
        /// <summary>
        /// Retrieves actual xbox connection status. Average execution time of 3600 executions per second.
        /// </summary>
        /// <returns>Connection status</returns>
        public bool Ping()
        {
            return Ping(Timeout);
        }
        /// <summary>
        /// Retrieves actual xbox connection status. Average execution time of 3600 executions per second.
        /// </summary>
        /// <param name="waitTime">Time to wait for a response</param>
        /// <returns>Connection status</returns>
        public bool Ping(int waitTime)
        {
            int oldTimeOut = 5000;
            try
            {
                if (XboxName != null)
                {
                    if (XboxName.Available > 0)
                        XboxName.Client.Receive(new byte[XboxName.Available]);

                    XboxName.Client.Send(Encoding.ASCII.GetBytes(Environment.NewLine));
                    Timeout = waitTime;
                    FlushSocketBuffer(16);    // throw out garbage response "400- Unknown Command\r\n"
                    Connected = true;
                    return true;
                }
                return false;
            }
            catch
            {
                Connected = false;
                XboxName.Close();
                return false;
            }
            finally
            {
                Timeout = oldTimeOut;   // make sure to restore old timeout
            }
        }
        #endregion

        //TODO: add {Set;} Double,long etc
        #region Types {Get; Set;}

        #region Bool {Get; Set;}
        public bool SetBool(uint Address)
        {
            return GetMemory(Address, 1)[0] != 0;
        }

        public void SetBool(uint Address, bool Value)
        {
            object obj;
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
            SetMemory(address, numArray);
        }

        public void SetBool(uint Address, bool[] Value)
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
            SetMemory(Address, numArray);
        }
        #endregion

        #region String {Get; Set;}
        public string GetString(uint Address, uint size)
        {
            return Encoding.UTF8.GetString(GetMemory(Address, size));
        }

        public void SetString(uint Address, string String)
        {
            byte[] numArray = new byte[0];
            string str = String;
            for (int i = 0; i < str.Length; i++)
            {
                byte num = (byte)str[i];
                numArray.Push(out numArray, num);
            }
            numArray.Push(out numArray, 0);
            SetMemory(Address, numArray);
        }
        #endregion

        #region Float {Get; Set;}

        public float GetFloat(uint Address)
        {
            if (Connected == true)
            {
                Console.WriteLine(" Command On Address ==>" + Address + " <== Is Being checked");
                byte[] memory = GetMemory(Address, 4);
                ReverseBytes(memory, 4);
                Console.WriteLine(" Command On Address ==>" + Address + " <== Is Returned");
                return BitConverter.ToSingle(memory, 0);
            }
            else
            {
                Console.WriteLine(Connection_Error);
                return 0;
            }
        }

        public float[] GetFloat(uint Address, uint ArraySize)
        {
            {
                float[] single = new float[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 4);
                ReverseBytes(memory, 4);
                for (int i = 0; i < ArraySize; i++)
                {
                    single[i] = BitConverter.ToSingle(memory, i * 4);
                }
                return single;
            }
        }

        public void SetFloat(uint Address, float Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            Array.Reverse(bytes);
            SetMemory(Address, bytes);
        }

        public void SetFloat(uint Address, float[] Value)
        {
            byte[] numArray = new byte[Value.Length * 4];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 4);
            }
            ReverseBytes(numArray, 4);
            SetMemory(Address, numArray);
        }
        #endregion

        #region Byte {Get; Set;}
        public byte GetByte(uint Address)
        {
            return GetMemory(Address, 1)[0];
        }

        public void SetByte(uint Address, byte Value)
        {
            SetMemory(Address, new byte[] { Value });
        }

        public void SetByte(uint Address, byte[] Value)
        {
            SetMemory(Address, Value);
        }
        #endregion

        #region SByte {Get; Set;}
        public sbyte GetSByte(uint Address)
        {
            return (sbyte)GetMemory(Address, 1)[0];
        }
        public void SetSByte(uint Address, sbyte Value)
        {
            byte[] bytes = new byte[] { BitConverter.GetBytes(Value)[0] };
            SetMemory(Address, bytes);
        }

        public void SetSByte(uint Address, sbyte[] Value)
        {
            byte[] numArray = new byte[0];
            sbyte[] value = Value;
            for (int i = 0; i < value.Length; i++)
            {
                numArray.Push(out numArray, (byte)value[i]);
            }
            SetMemory(Address, numArray);
        }
        #endregion

        #region Int16 {Get; Set;}
        public short GetInt16(uint Address)
        {
            byte[] memory = GetMemory(Address, 2);
            ReverseBytes(memory, 2);
            return BitConverter.ToInt16(memory, 0);
        }

        public short[] GetInt16(uint Address, uint ArraySize)
        {

            {
                short[] num = new short[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 2);
                ReverseBytes(memory, 2);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToInt16(memory, i * 2);
                }
                return num;
            }
        }

        public void SetInt16(uint Address, short Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 2);
            SetMemory(Address, bytes);
        }

        public void SetInt16(uint Address, short[] Value)
        {
            byte[] numArray = new byte[Value.Length * 2];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 2);
            }
            ReverseBytes(numArray, 2);
            SetMemory(Address, numArray);
        }
        #endregion

        #region Int32 {Get; Set;}
        public int GetInt32(uint Address)
        {
            byte[] memory = GetMemory(Address, 4);
            ReverseBytes(memory, 4);
            return BitConverter.ToInt32(memory, 0);
        }

        public int[] GetInt32(uint Address, uint ArraySize)
        {
            {
                int[] num = new int[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 4);
                ReverseBytes(memory, 4);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToInt32(memory, i * 4);
                }
                return num;
            }
        }
        public void SetInt32(uint Address, int Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 4);
            SetMemory(Address, bytes);
        }

        public void SetInt32(uint Address, int[] Value)
        {
            byte[] numArray = new byte[Value.Length * 4];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 4);
            }
            ReverseBytes(numArray, 4);
            SetMemory(Address, numArray);
        }
        #endregion

        #region Int64 {Get; Set;}
        public long GetInt64(uint Address)
        {
            byte[] memory = GetMemory(Address, 8);
            ReverseBytes(memory, 8);
            return BitConverter.ToInt64(memory, 0);
        }

        public long[] GetInt64(uint Address, uint ArraySize)
        {

            {
                long[] num = new long[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 8);
                ReverseBytes(memory, 8);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt32(memory, i * 8);
                }
                return num;
            }
        }
        public void SetInt64(uint Address, long Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 8);
            SetMemory(Address, bytes);
        }

        public void SetInt64(uint Address, long[] Value)
        {
            byte[] numArray = new byte[Value.Length * 8];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 8);
            }
            ReverseBytes(numArray, 8);
            SetMemory(Address, numArray);
        }
        #endregion

        #region UInt16 {Get; Set;}
        public ushort GetUInt16(uint Address)
        {
            byte[] memory = GetMemory(Address, 2);
            ReverseBytes(memory, 2);
            return BitConverter.ToUInt16(memory, 0);
        }

        public ushort[] GetUInt16(uint Address, uint ArraySize)
        {

            {
                ushort[] num = new ushort[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 2);
                ReverseBytes(memory, 2);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt16(memory, i * 2);
                }
                return num;
            }
        }
        public void SetUInt16(uint Address, ushort Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 2);
            SetMemory(Address, bytes);
        }

        public void SetUInt16(uint Address, ushort[] Value)
        {
            byte[] numArray = new byte[Value.Length * 2];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 2);
            }
            ReverseBytes(numArray, 2);
            SetMemory(Address, numArray);
        }


        #endregion

        #region UInt32 {Get; Set;}
        public uint GetUInt32(uint Address)
        {
            byte[] memory = GetMemory(Address, 4);
            ReverseBytes(memory, 4);
            return BitConverter.ToUInt32(memory, 0);
        }

        public uint[] GetUInt32(uint Address, uint ArraySize)
        {

            {
                uint[] num = new uint[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 4);
                ReverseBytes(memory, 4);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt32(memory, i * 4);
                }
                return num;
            }
        }
        public void SetUInt32(uint Address, uint Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 4);
            SetMemory(Address, bytes);
        }
        public void SetUInt32(uint Address, uint[] Value)
        {
            byte[] numArray = new byte[Value.Length * 4];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 4);
            }
            ReverseBytes(numArray, 4);
            SetMemory(Address, numArray);
        }
        #endregion

        #region UInt64 {Get; Set;}
        public ulong GetUInt64(uint Address)
        {
            byte[] memory = GetMemory(Address, 8);
            ReverseBytes(memory, 8);
            return BitConverter.ToUInt64(memory, 0);
        }

        public ulong[] GetUInt64(uint Address, uint ArraySize)
        {

            {
                ulong[] num = new ulong[ArraySize];
                byte[] memory = GetMemory(Address, ArraySize * 8);
                ReverseBytes(memory, 8);
                for (int i = 0; i < ArraySize; i++)
                {
                    num[i] = BitConverter.ToUInt32(memory, i * 8);
                }
                return num;
            }
        }

        public void SetUInt64(uint Address, ulong Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            ReverseBytes(bytes, 8);
            SetMemory(Address, bytes);
        }

        public void SetUInt64(uint Address, ulong[] Value)
        {
            byte[] numArray = new byte[Value.Length * 8];
            for (int i = 0; i < Value.Length; i++)
            {
                BitConverter.GetBytes(Value[i]).CopyTo(numArray, i * 8);
            }
            ReverseBytes(numArray, 8);
            SetMemory(Address, numArray);
        }

        public void InvalidateMemoryCache(bool v, uint address, uint length)
        {

        }
        #endregion

        #region Double {get; Set;}
        public double GetDouble(uint Address)
        {
            byte[] memory = GetMemory(Address, 4);
            ReverseBytes(memory, 4);
            return BitConverter.ToDouble(memory, 0);
        }
        #endregion

        #region Long {get; Set;}
        public long Getlong(uint Address)
        {
            byte[] memory = GetMemory(Address, 4);
            ReverseBytes(memory, 4);

            return Convert.ToUInt32(memory);
        }
        #endregion

        #endregion
    }

    #region Misc
    /// <summary>
    /// Credits To James The JRPC Dev.
    /// </summary>
    public static class JRPC
    {


        private static HashSet<Type> ValidReturnTypes { get; set; }

        public static void CallVoid(uint Address, params object[] Arguments)
        {
            CallArgs(true, 0, typeof(void), null, 0, Address, 0, Arguments);
        }

        public static void CallVoid(ThreadType Type, string module, int ordinal, params object[] Arguments)
        {
            CallArgs(Type == ThreadType.System, 0 /*Void*/, typeof(void), module, ordinal, 0, 0, Arguments);
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

        /// <summary>
        /// EDITED:  Do Not Use
        /// </summary>
        private static object CallArgs(bool SystemThread, uint Type, Type t, string module, int ordinal, uint Address, uint ArraySize, params object[] Arguments)
        {
            Xbox xbox = new Xbox();
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

                xbox.ConversationTimeout = 4000000;
                xbox.ConnectTimeout = 4000000;
                object[] XBDMVersion = new object[] { "consolefeatures ver=", 2 /*version*/, " type=", Type, null, null, null, null, null, null, null, null, null };
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
                        object[] num2 = new object[] { obj, /*Int*/0, "\\", Functions.UIntToInt((uint)obj2), "\\" };
                        str2 = string.Concat(num2);
                        flag = true;
                    }
                    if (obj2 is int || obj2 is bool || obj2 is byte)
                    {
                        if (!(obj2 is bool))
                        {
                            object obj3 = str2;
                            object[] objArray2 = new object[] { obj3, /*Int*/0, "\\", null, null };
                            objArray2[3] = (obj2 is byte ? Convert.ToByte(obj2).ToString() : Convert.ToInt32(obj2).ToString());
                            objArray2[4] = "\\";
                            str2 = string.Concat(objArray2);
                        }
                        else
                        {
                            object obj4 = str2;
                            object[] num3 = new object[] { obj4, /*Int*/0, "/", Convert.ToInt32((bool)obj2), "\\" };
                            str2 = string.Concat(num3);
                        }
                        flag = true;
                    }
                    else if (obj2 is int[] || obj2 is uint[])
                    {
                        byte[] numArray = Functions.IntArrayToByte((int[])obj2);
                        object obj5 = str2;
                        object[] str3 = new object[] { obj5, 0.ToString(), "/", numArray.Length, "\\" };
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
                        object[] objArray3 = new object[] { obj6, 0.ToString(), "/", str4.Length, "\\", ((string)obj2).ToHexString(), "\\" };
                        str2 = string.Concat(objArray3);
                        flag = true;
                    }
                    else if (obj2 is double)
                    {
                        double num4 = (double)obj2;
                        str = str2;
                        strArrays = new string[] { str, 0.ToString(), "\\", num4.ToString(), "\\" };
                        str2 = string.Concat(strArrays);
                        flag = true;
                    }
                    else if (obj2 is float)
                    {
                        float single = (float)obj2;
                        str = str2;
                        strArrays = new string[] { str, 0.ToString(), "\\", single.ToString(), "\\" };
                        str2 = string.Concat(strArrays);
                        flag = true;
                    }
                    else if (obj2 is float[])
                    {
                        float[] singleArray = (float[])obj2;
                        str = str2;
                        strArrays = new string[] { str, 0.ToString(), "/", null, null };
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
                        name = new object[] { obj, 0.ToString(), "/", numArray1.Length, "\\" };
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
                        strArrays = new string[] { str, 0.ToString(), "\\", null, null };
                        ulong num5 = Functions.ConvertToUInt64(obj2);
                        strArrays[3] = num5.ToString();
                        strArrays[4] = "\\";
                        str2 = string.Concat(strArrays);
                    }
                }
                str2 = string.Concat(str2, "\"");
                string str5 = xbox.SendTextCommand(str2);
                string str6 = "buf_addr=";
                while (str5.Contains(str6))
                {
                    Thread.Sleep(250);
                    uint num6 = uint.Parse(str5.Substring(str5.find(str6) + str6.Length), NumberStyles.HexNumber);
                    str5 = xbox.SendTextCommand(string.Concat("consolefeatures ", str6, "0x", num6.ToString("X")));
                }
                xbox.ConversationTimeout = 2000;
                xbox.ConnectTimeout = 5000;
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
                                return Functions.UIntToInt(num7);
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
                            if (Type == 0)
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
        public static void Push(this byte[] InArray, out byte[] OutArray, byte Value)
        {
            OutArray = new byte[InArray.Length + 1];
            InArray.CopyTo(OutArray, 0);
            OutArray[InArray.Length] = Value;
        }

    }
    #endregion
}