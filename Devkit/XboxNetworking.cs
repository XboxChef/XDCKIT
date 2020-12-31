//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace XDevkit
{
    /// <summary>
    /// Xbox Emulation Class
    /// Made By TeddyHammer
    /// </summary>
    public partial class Xbox
    {
        #region Property's
        public FileSystem FileSystem { get; }
        private bool ValidConnection;
        private RwStream _readWriter;
        private uint _startDumpOffset;
        private bool _stopSearch;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string timeStamp = GetTimestamp(DateTime.Now);
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetTimestamp(DateTime value) { return value.ToString("M" + "MM-" + "dd" + "-" + "yyyy"); }




        [Browsable(false)]
        /// <summary>
        /// Set or Get the start dump offset
        /// </summary>
        public uint DumpOffset { set { _startDumpOffset = value; } }
        [Browsable(false)]
        /// <summary>
        /// Set or Get the dump length
        /// </summary>
        public uint DumpLength { set; get; }
        [Browsable(false)]
        /// <summary>
        /// Stop any searching
        /// </summary>
        public bool StopSearch
        {
            get
            {
                if (!_readWriter.Accessed)
                    return false;
                return _readWriter.StopSearch;
            }
            set
            {
                if (!_readWriter.Accessed)
                    return;
                _readWriter.StopSearch = value;
                _stopSearch = value;
            }
        }








        #endregion

        #region Networking
        /// <summary>
        /// Gets or sets the maximum waiting time given (in milliseconds) for a response.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Timeout { get => 5000; set => Timeout = value; }

        private const string Connection_Error = "Console Not Connected";
        private const string XAMModule = "xam.xex";
        private const string krnlModule = "xboxkrnl.exe";

        public string Name { get; set; }
        public string IPAddress { get; set; }
        [Browsable(false)]
        public static TcpClient XboxName;
        [Browsable(false)]
        public StreamReader Reader;
        public static string response;
        [Browsable(false)]
        public bool Connected { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string xdkRegistryPath = @"HKEY_CURRENT_USER\Software\Microsoft\XboxSDK";
        /// <summary>
        /// Gets or sets the last Xbox connection used. (PC Use Only)
        /// </summary>
        [Browsable(false)]
        public string LastConnectionUsed
        {
            get { return (string)Microsoft.Win32.Registry.GetValue(xdkRegistryPath, "XboxName", string.Empty); }
            set { Microsoft.Win32.Registry.SetValue(xdkRegistryPath, "XboxName", value); }
        }
        public object DefaultConsole { get; }
        public uint ConversationTimeout { get; private set; }
        public uint ConnectTimeout { get; private set; }
        private bool on = true;
        /// <summary>
        /// Checks the connection status between xbox and pc.
        /// This function only checks what Yelo.Debug believes to be the current connection status.
        /// For a true status check you will need to ping the xbox regularly.
        /// </summary>
        public void ConnectionCheck()
        {
            //takes too much time to ping when used by continuously called functions 
            //if connection drops attempt to reconnect, otherwise fuck them, let it crash and burn...
            if (!Connected) Reconnect(250);    // try to re-establish a connection
        }
        /// <summary>
        /// Re-establishes a connection with the currently selected xbox console.
        /// </summary>
        public bool Reconnect()
        {
            return Reconnect(1000);
        }
        /// <summary>
        /// Attempts to re-establish a connection with the currently selected xbox console.
        /// </summary>
        /// <param name="timeout"></param>
        /// <remarks>Bugfix by xmt</remarks>
        public bool Reconnect(int timeout)
        {
            Disconnect();   // close our old connection
            DateTime Before = DateTime.Now;

            while (!Ping(100))
            {
                try
                {
                    Connect();    // create a new one using the current connection information
                }
                catch
                {
                    TimeSpan Elapse = DateTime.Now - Before;
                    if (Elapse.TotalMilliseconds > timeout)
                    {
                        Disconnect();
                        //throw new TimeoutException("Connection lost - unable to reconnect.");
                        return false;
                    }
                }
            }
            return true;
        }
        private DoWorkEventHandler FindConsole_OnBackground()
        {

            while (on)
            {
                int i = 0;

                for (; ; )
                {
                    if (i < 255)
                    {
                        XboxName = new TcpClient();
                        if (XboxName.ConnectAsync(IPS() + i, 730).Wait(10))//keep calm just code..
                        {
                            IPAddress = IPS() + i;
                            IP.Default["IPAddress"] = IPS() + i;
                            IP.Default.Save(); // Saves settings in application configuration file
                            Connected = true;
                            on = false;
                            return null;
                        }
                        else
                        {
                            i++;
                        }

                    }
                    else
                    {
                        on = false;
                        Connected = false;
                        return null;
                    }
                }
            }
            return null;
        }

        public static string IPS()
        {
            return "192.168.0.";
        }

        BackgroundWorker FindConsoleBG = new BackgroundWorker();
        /// <summary>
        /// Connect to the  using port 730 using the given ip address
        /// </summary>
        /// <returns>True if connection was successful and False if not</returns>
        public bool CheckConnection()
        {
            try
            {
                if (IPAddress.Length < 5)
                    throw new Exception("Invalid IP");
                if (Connected)
                    return true; //If you are already connected then return
                var response = new byte[1024];
                XboxName.Client.Receive(response);
                string reponseString = Encoding.ASCII.GetString(response).Replace("\0", string.Empty);
                //validate connection
                Connected = reponseString.Substring(0, 3) == "201";

                return Connected;
            }
            catch
            {
                throw new FailedConnectionException();
            }
        }



        public bool FindConsole()//proper looping 
        {
            on = true;
            FindConsoleBG.RunWorkerAsync();
            int n = 0;
            switch (n)
            {
                case 0:
                    FindConsole_OnBackground();
                    goto case 1;
                case 1:
                    if (Connected == true)
                    {
                        on = false;
                        return true;
                    }
                    else
                    {
                        if (n < 3)
                        {
                            Console.WriteLine("Connection Fail Safe Activated....");
                            n++;
                            FindConsole_OnBackground();
                        }
                        if (n < 6)
                        {
                            Console.WriteLine("Connection Must Not Be Available, Please Make Sure Your On the Same Network As Your Console Otherwise Please Try Again Later.....");
                            Console.WriteLine("Connection Fail Safe Terminated, Reason: FindConsole Has Failed User Not Connected To Network....");
                            on = false;
                            return false;
                        }
                        n++;
                        goto case 1;
                    }
            }
            on = false;
            return false;
        }


        public T Call<T>(string module, int ordinal, params object[] Arguments) where T : struct
        {
            return (T)CallArgs(true, TypeToType<T>(false), typeof(T), module, ordinal, 0U, 0U, Arguments);
        }
        private uint TypeToType<T>(bool Array) where T : struct
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

        /// <summary>
        /// Connects Local Tcp Connection From Device To Xbox Console
        /// 
        /// </summary>
        public bool Connect(string XboxNameOrIP = "defualt")
        {

            //User Enter's Nothing
            if (XboxNameOrIP == "defualt")
            {
                XboxName = new TcpClient();
                if (XboxName.ConnectAsync(IP.Default.IPAddress, 730).Wait(5))//wait time for this can be less..
                {
                    IPAddress = IP.Default.IPAddress;
                    Console.WriteLine("/Connection - I01/....(" + IPAddress + ")");
                    return Connected = true;
                }
                else if (FindConsole())//if true then continue
                {
                    XboxName = new TcpClient(IPAddress, 730);
                    Reader = new StreamReader(XboxName.GetStream());
                    Console.WriteLine("/Connection - F01/....(" + IPAddress + ")");
                    return Connected = true;
                }
                else// if top fails
                {
                    return false;
                }
            }
            // If User Supply's IP To US.
            else if (XboxNameOrIP.ToCharArray().Any(char.IsDigit))
            {
                string IPAddress = XboxNameOrIP;
                XboxName = new TcpClient(XboxNameOrIP, 730);
                Reader = new StreamReader(XboxName.GetStream());
                Console.WriteLine("/Connection - Degits/....(" + "Manual Connection Mode" + ")");
                return Connected = true;
            }
            //Get IP Via Name
            else if (XboxNameOrIP.ToCharArray().Any(char.IsLetter))//uses ip to find console makes user think it finds it via name 
            {

                if (FindConsole())//if true then continue
                {
                    XboxName = new TcpClient(IPAddress, 730);
                    Reader = new StreamReader(XboxName.GetStream());
                    Console.WriteLine("/Connection - Letter/....(" + "Manual Connection Mode" + ")");
                    return Connected = true;
                }
                else
                {
                    Console.WriteLine("/Connection - Letter/....(" + "Manual Connection Mode Failed" + ")");
                    return false;
                }

            }

            else
            {
                Console.WriteLine("/Connection - SkyFall/....(" + "Unknown Bug" + ")");
                return false;
            }


        }


        public void CloseConnection(uint Connection = 0)
        {
            SendTextCommand("bye");
            Reader.Close();
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
            catch
            {
            }
        }
        #region Yelo debug Networking


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
                if (line[0] == '.')
                    break;
                else
                    response.Append(line);
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
                            if (!Ping(250))
                                Disconnect();  // only disconnect if actually disconnected
                            throw new TimeoutException();
                        }
                    }
                }
            }
            else
                throw new NoConnectionException();
        }

        /// <summary>
        /// Waits for data to be received.  During execution this method will enter a spin-wait loop and appear to use
        /// 100% cpu when in fact it is just a suspended thread.   This is much more efficient than waiting a
        /// millisecond since most commands take fractions of a millisecond. It will either resume after the condition
        /// is met or throw a timeout exception.
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
                                if (!Ping(250))
                                    Disconnect();  // only disconnect if actually disconnected
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
                                if (!Ping(250))
                                    Disconnect();  // only disconnect if actually disconnected
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
                                if (!Ping(250))
                                    Disconnect();  // only disconnect if actually disconnected
                                throw new TimeoutException();
                            }
                        }
                        break;
                }
            }
            else
                throw new NoConnectionException();
        }

        /// <summary>
        /// Waits for the receive buffer to stop receiving, then clears it. Call this before you send anything to the
        /// xbox to help keep the channel in sync.
        /// </summary>
        public void FlushSocketBuffer()
        {
            Wait(WaitType.Idle);    // waits for the link to be idle...
            try
            {
                if (XboxName.Available > 0)
                    XboxName.Client.Receive(new byte[XboxName.Available]);
            }
            catch
            {
                Connected = false;
            }
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
                catch
                {
                    Connected = false;
                }
            }
        }

        /// <summary>
        /// Retrieves actual xbox connection status. Average execution time of 3600 executions per second.
        /// </summary>
        /// <returns>Connection status</returns>
        public bool Ping() { return Ping(Timeout); }

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
                SendTextCommand(Command, out response);
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
            try
            {
                // Max packet size is 1026
                byte[] Packet = new byte[1026];
                if (XboxName.Connected == true)
                {
                    FlushSocketBuffer();
                    Console.WriteLine("SendTextCommand " + Command + " ==> Sending Command... <==");
                    XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
                    XboxName.Client.Receive(Packet);
                    response = Encoding.ASCII.GetString(Packet);

                }
                else
                {
                    Console.WriteLine("SendTextCommand ==> " + Assembly.GetEntryAssembly().GetName().Name + " Connection = " + Connected);
                    Console.WriteLine("Failed to SendTextCommand ==> All Three Checks Failed.. <==");
                }

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
            FlushSocketBuffer();
            return SendTextCommand("BOXID").Replace("200- ", string.Empty);
        }

        /// <summary>
        /// Turns The Console's Default Neighborhood Icon to any of the following...(black , blue , bluegray , nosidecar
        /// , white) Also Changes The Type Of Console It Is.
        /// </summary>
        /// <param name="Color"></param>
        public void SetConsoleColor(XboxColor Color)
        {
            FlushSocketBuffer();
            SendTextCommand("setcolor name=" + Enum.GetName(typeof(int), Color).ToLower());
        }

        /// <summary>
        /// Get's The Consoles ID.
        /// </summary>
        /// <returns></returns>
        public string GetConsoleID()
        {
            FlushSocketBuffer();
            return SendTextCommand(string.Concat("getconsoleid")).Replace("200- consoleid=", string.Empty);
        }

        /// <summary>
        /// Gets the debug Monitor version Number.
        /// </summary>
        public string GetDMVersion()
        {
            FlushSocketBuffer();
            return SendTextCommand("dmversion").Replace("200- ", string.Empty);

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
                                return s.Substring(Start + 4, End - 4);
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
                            return SendTextCommand(string.Concat("consoletype")).Replace("200- ", string.Empty);
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
                                return s.Substring(Start + 9, End - 1).Substring(Start);
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
                                return s.Substring(Start + End + 4, End - 4).Substring(Start);
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
                                return s.Substring(Start - 10, End);
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
                                return s.Substring(Start + 6, End);
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
                            SendTextCommand(string.Concat("systeminfo"), out response);
                            string[] Info = new[] { ReceiveMultilineResponse().ToString().ToLower() };
                            foreach (string s in Info)
                            {
                                return s.Substring(s.IndexOf("xdk=") + 4, 12);
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
    }
}