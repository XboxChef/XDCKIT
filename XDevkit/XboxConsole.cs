using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace XDevkit
{
    public class XboxConsole
    {
        #region Objects
        public static TcpClient XboxName;
        public StreamReader sreader;
        internal int connectionId;
        private static string responses;
        public bool Connected = false;
        private string LastIPAddress = "192.168.0.5"; 
        #endregion

        #region Connect/Disconnect TCPConnection
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
                    XboxNameOrIP = LastIPAddress;
                    XboxName = new TcpClient(XboxNameOrIP, 730);
                    sreader = new StreamReader(XboxName.GetStream());
                    connectionId = 1;
                    // First thing that XBDM does is send a packet to us when we connect

                    // Max packet size is 1026
                    byte[] Packet = new byte[1026];
                    XboxName.Client.Receive(Packet);

                    if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "201")
                    {
                        throw new Exception("XBDM did not send connect message");
                    }
                    else if (XboxName.Connected)
                    {
                        Console.WriteLine("Connected == ( Method 1 )");
                        return Connected = true;
                    }

                }
                if (XboxNameOrIP.Contains("192"))
                {
                    XboxName = new TcpClient(XboxNameOrIP, 730);
                    sreader = new StreamReader(XboxName.GetStream());
                    connectionId = 1;
                    // First thing that XBDM does is send a packet to us when we connect

                    // Max packet size is 1026
                    byte[] Packet = new byte[1026];
                    XboxName.Client.Receive(Packet);

                    if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "201")
                    {
                        return Connected = false;
                        throw new Exception("XBDM did not send connect message");
                    }
                    else if (XboxName.Connected)
                    {
                        Console.WriteLine("Connected == ( Method 2 )");
                        return Connected = true;
                    }
                }

                return Connected = false;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                connectionId = 0;
                return Connected = false;
            }
        }
        public void CloseConnection(uint Connection)
        {
            SendTextCommand("bye");
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
                    SendCommand("bye"); // we cant leave without saying goodbye ;)
                }
            }
            catch { }
        }
        #endregion

        #region SendCommands
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
        public void SendTextCommand(string Command, out string response)
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
            return responses.Replace("200- consoleid=", "");
        }
        /// <summary>
        /// Gets the debug Monitor version Number.
        /// </summary>
        public string GetDMVersion()
        {
            string dre = string.Concat("dmversion");
            SendTextCommand(dre, out responses);
            return responses.Replace("200- ", "");
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

        #region XboxFile System
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
        /// Deletes a file on the xbox.
        /// </summary>
        /// <param name="fileName">File to delete.</param>
        public void DeleteFile(string fileName)
        {
            string dre = string.Concat("delete name=\"{0}\"", fileName);
            SendTextCommand(dre);
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
        #endregion

        #region Features
        public void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags)
        {
            string[] lines = Name.Split("\\".ToCharArray());
            for (int i = 0; i < lines.Length - 1; i++)
                MediaDirectory += lines[i] + "\\";
            object[] Reboot = new object[] { $"magicboot title=\"{Name}\" directory=\"{MediaDirectory}\"" };//todo
            SendTextCommand(string.Concat(Reboot));
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
                        Reboot(@"\Device\Harddisk0\SystemExtPartition\20445100\dash.xex", @"\Device\Harddisk0\SystemExtPartition\20445100\", @"\Device\Harddisk0\SystemExtPartition\20445100\", XboxRebootFlags.Title);
                        break;
                    case (int)XboxShortcuts.Turn_Off_Console:
                        SendTextCommand("shutdown");
                        break;
                    case (int)XboxShortcuts.Account_Management:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Account_Management), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Achievements:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Achievements), new object[] { 0, 0, 0, 0 });//achievements
                        break;
                    case (int)XboxShortcuts.Active_Downloads:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Active_Downloads), new object[] { 0, 0, 0, 0 });//XamShowMarketplaceDownloadItemsUI
                        break;
                    case (int)XboxShortcuts.Awards:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Awards), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Beacons_And_Activiy:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Beacons_And_Activiy), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Family_Settings:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Family_Settings), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Friends:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Friends), new object[] { 0, 0, 0, 0 });//friends
                        break;
                    case (int)XboxShortcuts.Guide_Button:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Guide_Button), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Messages:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Messages), 0);//messages tab
                        break;
                    case (int)XboxShortcuts.My_Games:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.My_Games), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Open_Tray:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Open_Tray), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Party:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Party), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Preferences:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Preferences), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Private_Chat:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Private_Chat), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Profile:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Profile), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Recent:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Recent), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Redeem_Code:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Redeem_Code), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Select_Music:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Select_Music), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.System_Music_Player:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Music_Player), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.System_Settings:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Settings), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.System_Video_Player:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.System_Video_Player), new object[] { 0, 0, 0, 0 });
                        break;
                    case (int)XboxShortcuts.Windows_Media_Center:
                        misc.CallVoid(misc.ResolveFunction("xam.xex", (int)XboxShortcuts.Windows_Media_Center), new object[] { 0, 0, 0, 0 });
                        break;

                }
        }
        /// <summary>
        /// Reboot Method flag types cold or warm reboot.
        /// </summary>
        public  void Reboot(XboxReboot Warm_or_Cold)
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
        #endregion

        #region Misc
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

        #region Float {Get; Set;}
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

        #region MemoryEdits {Get; Set;}
        public void SetMemory(uint Address, byte[] Data)
        {
            SetMemory(Address, 0, Data, out _);
        }
        public void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten)//aka response
        {

            // Send the setmem command
            XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n", Address.ToString("X2"), BitConverter.ToString(Data).Replace("-", ""))));

            // Check to see our response
            byte[] Packet = new byte[1026];
            XboxName.Client.Receive(Packet);
            BytesWritten = 0;
            //BytesWritten = Convert.ToUInt32(Encoding.ASCII.GetString(Packet));
            if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 11) == "0 bytes set")
                throw new Exception("A problem occurred while writing bytes. 0 bytes set");
        }
        public byte[] GetMemory(uint Address, uint Length)//TODO: Add == InvalidateMemoryCache
        {

            {
                uint num = 0;
                byte[] numArray = new byte[Length];
                GetMemory(Address, Length, numArray, out num);
                //console.DebugTarget.InvalidateMemoryCache(true, Address, Length);
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

            if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "203")
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
        #endregion

        #region Yelo debug stuff
        /// <summary>
        /// Gets or sets the maximum waiting time given (in milliseconds) for a response.
        /// </summary>
        [Browsable(false)]
        public int Timeout { get { return timeout; } set { timeout = value; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int timeout = 5000;
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
                timeout = oldTimeOut;   // make sure to restore old timeout
            }
        }
        #endregion

        //public void XNotify(string Text, uint Type)
        //{
        //    object[] arguments = new object[] { 0x10, 0xff, 2, "One Tool Connected".ToWCHAR(), 1 };
        //    CallVoid(ThreadType.Title, "xam.xex", 0x290, arguments);
        //}
    }
}