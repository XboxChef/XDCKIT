//Do Not Delete This Comment... 
//Made By Serenity on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace XDCKIT
{

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XboxClient
    {
        #region Property's
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TcpClient XboxName;
        /// <summary>
        /// Checks For Connection, Defaults To False.
        /// </summary>
        public static bool Connected = false;
        public static int Port
        {
            get => 730;
            set
            {
                Port = value;
            }
        }
        public static string IPAddress { get; set; } = "192.000.000.000";
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static StreamReader Reader;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        static readonly BackgroundWorker FindConsoleBc = new BackgroundWorker();
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        static readonly BackgroundWorker FindConsolegc = new BackgroundWorker();
        #endregion

        #region Networking
        public static string DefaultConsole
        {
            get
            {
                if (Connected)
                {
                    return IPAddress;
                }
                else
                {
                    return "Error";
                }
            }
            set
            {
                DefaultConsole = value;
            }
        }
        #region Connection Types
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Client"></param>
        /// <param name="ConsoleNameOrIP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool Connect(this XboxConsole Source, out XboxConsole Client)
        {
            string ConsoleNameOrIP = "jtag";
            Client = Source = new XboxConsole();//sets Class For Client

            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("jtag") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    try
                    {
                        XboxName = new TcpClient(IPAddress, 730);
                        Reader = new StreamReader(XboxName.GetStream());
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            // If User Supply's IP To US.
            else if (ConsoleNameOrIP.ToCharArray().Any(char.IsDigit))
            {
                try
                {
                    IPAddress = ConsoleNameOrIP;
                    XboxName = new TcpClient(ConsoleNameOrIP, Port);
                    Reader = new StreamReader(XboxName.GetStream());

                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Connected;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Client"></param>
        /// <param name="ConsoleNameOrIP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool Connect(this XboxConsole Source, out XboxConsole Client, string ConsoleNameOrIP = "jtag")
        {
            Client = Source = new XboxConsole();//sets Class For Client

            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("jtag") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    try
                    {
                        XboxName = new TcpClient(IPAddress, 730);
                        Reader = new StreamReader(XboxName.GetStream());
                        IPAddress = ConsoleNameOrIP;
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            // If User Supply's IP To US.
            else if (ConsoleNameOrIP.ToCharArray().Any(char.IsDigit))
            {
                try
                {
                    IPAddress = ConsoleNameOrIP;
                    XboxName = new TcpClient(ConsoleNameOrIP, Port);
                    Reader = new StreamReader(XboxName.GetStream());

                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Connected;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Client"></param>
        /// <param name="ConsoleNameOrIP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool Connect(this XboxConsole Source, out XboxConsole Client, string ConsoleNameOrIP = "jtag", int Port = 730)
        {
            Client = Source = new XboxConsole();

            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("jtag") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    try
                    {
                        XboxName = new TcpClient(IPAddress, 730);
                        Reader = new StreamReader(XboxName.GetStream());
                        IPAddress = ConsoleNameOrIP;
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            // If User Supply's IP To US.
            else if (ConsoleNameOrIP.ToCharArray().Any(char.IsDigit))
            {
                try
                {
                    IPAddress = ConsoleNameOrIP;
                    XboxName = new TcpClient(ConsoleNameOrIP, Port);
                    Reader = new StreamReader(XboxName.GetStream());

                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Connected;
        }
        public static bool Connect(string ConsoleNameOrIP = "jtag", int Port = 730)
        {
            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("jtag") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    try
                    {
                        XboxName = new TcpClient(IPAddress, 730);
                        Reader = new StreamReader(XboxName.GetStream());
                        IPAddress = ConsoleNameOrIP;
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            // If User Supply's IP To US.
            else if (ConsoleNameOrIP.ToCharArray().Any(char.IsDigit))
            {
                try
                {
                    IPAddress = ConsoleNameOrIP;
                    XboxName = new TcpClient(ConsoleNameOrIP, Port);
                    Reader = new StreamReader(XboxName.GetStream());
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Connected;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Client"></param>
        /// <param name="ConsoleNameOrIP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool Connect(this XboxConsole Source, string ConsoleNameOrIP = "jtag")
        {
            Source = new XboxConsole();
            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("jtag") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    try
                    {
                        XboxName = new TcpClient(IPAddress, 730);
                        Reader = new StreamReader(XboxName.GetStream());
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            // If User Supply's IP To US.
            else if (ConsoleNameOrIP.ToCharArray().Any(char.IsDigit))
            {
                try
                {
                    IPAddress = ConsoleNameOrIP;
                    XboxName = new TcpClient(ConsoleNameOrIP, Port);
                    Reader = new StreamReader(XboxName.GetStream());

                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return Connected;
        }
        #endregion
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        static DoWorkEventHandler FindConsoleFunction()//TODO: Add a Second Search with a bool if connection is true then it will stop both for a faster search
        {
            string ips = "192.168.0.";

            for (int i = 1; i <= 100; i += 1)
            {
                XboxName = new TcpClient();

                try
                {
                    if (i == 100)
                    {
                        if (!XboxName.Connected)
                        {
                            Console.WriteLine("Slave1 Reached 100 With No IPS Found");
                            Console.WriteLine("Slave1 Is Console Even ON!");
                        }
                        else
                        {
                            return null;
                        }

                    }
                    else
                    {
                        //if connection is false then it will continue 
                        if (!Connected)
                        {
                            if (XboxName.ConnectAsync(ips + i, 730).Wait(20))//keep calm just code..
                            {
                                //if Connection was A Success then it will set the found IP and it will signal the connection was true
                                IPAddress = ips + i;
                                Connected = true;
                                Console.WriteLine("Connected");
                                return null;
                            }
                        }
                    }
                }
                catch
                {

                    return null;
                }
            }

            Connected = false;
            return null;
        }
        public static void FindConsole(uint Retries, uint RetryDelay)
        {

        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        static bool FindConsole()
        {
            if (FindConsoleBc.IsBusy == true)
            {
                FindConsolegc.RunWorkerAsync();
            }
            else
            {
                FindConsoleBc.RunWorkerAsync();
                FindConsoleFunction();
            }
            if (XboxName.Connected)
            {
                return true;
            }
            else
            {
                int noOfRetries = 0;

                if (noOfRetries < 3)
                {
                    FindConsoleFunction();
                    noOfRetries++;
                }
                if (noOfRetries < 6)
                {
                    return false;
                }
            }
            return false;
        }


        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void Disconnect()
        {
            try
            {
                if (Connected)
                {
                    XboxConsole.SendTextCommand("bye");
                    XboxName.Client.Dispose();
                    IPAddress = "000.000.000.000";
                    XboxName.Close();
                    Connected = false;
                }
            }
            catch
            {

            }
        }

        public static bool Delay(int millisecond)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool flag = false;
            while (!flag)
            {
                if (sw.ElapsedMilliseconds > millisecond)
                {
                    flag = true;
                }
            }
            sw.Stop();
            return true;

        }

        internal static void Reconnect()
        {
            Delay(1000);
            Disconnect();
            if (Connect(IPAddress, Port))
            {
                return;
            }
        }
        #endregion
    }
}
