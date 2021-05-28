//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace XDCKIT
{

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XboxClient
    {
        #region Property's
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TcpClient XboxName { get; set; }
        /// <summary>
        /// Checks For Connection, Defaults To False.
        /// </summary>
        public static bool Connected
        {
            get
            {
                XboxName = new TcpClient();
                if (XboxName.Connected)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            set
            {
                Connected = value;
            }
        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Client"></param>
        /// <param name="ConsoleNameOrIP"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool Connect(this XboxConsole Source, out XboxConsole Client, string ConsoleNameOrIP = "default", int Port = 730)
        {
            Client = Source;
            Client = new XboxConsole();//sets Class For Client

            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("default") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    XboxName = new TcpClient(IPAddress, 730);
                    Reader = new StreamReader(XboxName.GetStream());
                    IPAddress = ConsoleNameOrIP;

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
        public static bool Connect(string ConsoleNameOrIP = "default", int Port = 730)
        {
            // If user specifies to find their console IP address
            if (ConsoleNameOrIP.Equals("default") | ConsoleNameOrIP.Equals(string.Empty) | ConsoleNameOrIP.ToCharArray().Any(char.IsLetter))
            {
                if (FindConsole())//if true then continue
                {
                    XboxName = new TcpClient(IPAddress, 730);
                    Reader = new StreamReader(XboxName.GetStream());
                    IPAddress = ConsoleNameOrIP;

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
                    
                }
            }

            return Connected;
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        static DoWorkEventHandler BackgroundSlave()
        {
            string ips = "192.168.0.";

            for (int i = 0; i <= 255; i += 1)
            {
                XboxName = new TcpClient();

                try
                {
                    if (XboxName.ConnectAsync(ips + i, 730).Wait(12))//keep calm just code..
                    {
                        IPAddress = ips + i;

                        return null;
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
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool Ping (string host, int attempts, int timeout)
        {
            System.Net.NetworkInformation.Ping ping =
                                             new System.Net.NetworkInformation.Ping();

            System.Net.NetworkInformation.PingReply pingReply;

            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    pingReply = ping.Send(host, timeout);

                    // If there is a successful ping then return true.
                    if (pingReply != null &&
                        pingReply.Status == System.Net.NetworkInformation.IPStatus.Success)
                        return true;
                }
                catch
                {
                    // Do nothing and let it try again until the attempts are exausted.
                    // Exceptions are thrown for normal ping failurs like address lookup
                    // failed.  For this reason we are supressing errors.
                }
            }

            // Return false if we can't successfully ping the server after several attempts.
            return false;
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
            }

            BackgroundSlave();

            if (Connected == true)
            {
                return true;
            }
            else
            {
                int noOfRetries = 0;

                if (noOfRetries < 3)
                {
                    BackgroundSlave();
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
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool FindConsole(int retryAttepts)
        {
            return DoWithRetry(FindConsole(), TimeSpan.FromSeconds(5), retryAttepts);
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static bool DoWithRetry(bool action, TimeSpan sleepPeriod, int tryCount = 3)
        {
            if (tryCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(tryCount));

            while (action == false)
            {
                try
                {
                    if (action)
                    {
                        return true;
                    }
                    break; // success!
                    //else retrun false fixes issue

                }
                catch
                {
                    if (--tryCount == 0)
                        throw;
                    Thread.Sleep(sleepPeriod);

                }

            }
            return false;
        }

        private static bool Delay(int millisecond)
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
