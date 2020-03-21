
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XDevkit.Utils;

namespace XDevkit
{
    partial class XboxConsoleClass : TcpClient, IXboxConsole, IDisposable
    {
        public static  string IP;
        public static string DefaultXboxName = new XboxManager().DefaultConsole;
        public IXboxConsole ActiveXboxConsole { get; set; }

        public XboxConsoleClass(string XboxNameOrIP)
        {
            
            if(XboxNameOrIP == "default")//add 
            {

                DefaultXboxName = XboxNameOrIP;

            }
            else
            {
                IP = XboxNameOrIP;
            }


        }

        public XboxConsoleClass()
        {
        }

        private void SendTextCommand(string Command)
        {
            // Send a text command. Always have \r\n at the end of commands
            Xbox.XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
        }

        public void CloseConnection(uint Connection)
        {
            SendTextCommand("bye");
            Xbox.xboxName.Close();
        }

        public void DeleteFile(string Filename)
        {

        }

        public IXboxFiles DirectoryFiles(string Directory)
        {

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
                    ConsoleFinder();
                        break; // Sucess! Lets exit the loop!
                    }
                    catch (Exception)
                    {

                        Task.Delay(4).Wait();
                    }
                } while (true);
            
        }

        private void ConsoleFinder()
        {

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
            Xbox.GetDriveInformation(Drive, out FreeBytesAvailableToCaller, out TotalNumberOfBytes, out TotalNumberOfFreeBytes);
        }

        public IXboxFile GetFileObject(string Filename)
        {
            return null;
        }

        public uint OpenConnection(string Handler)
        {
            ActiveXboxConsole = new XboxManager().OpenConsole(Handler);
            return 0;
        }

        public void Reboot(string Name, string MediaDirectory, string CmdLine, XboxRebootFlags Flags)
        {
            object[] Reboot = new object[] { "magicboot title=\"" };//todo
            SendTextCommand(string.Concat(Reboot));
        }

        public void ReceiveSocketLine(uint Connection, out string Line)
        {

            byte[] textBuffer = new byte[256];  // buffer large enough to contain a line of text

            Thread.Sleep(0);

           Stopwatch.StartNew();
            while (true)
            {
                int avail = Xbox.xboxName.Available;   // only get once
                if (avail < textBuffer.Length)
                {
                    Xbox.xboxName.Client.Receive(textBuffer, avail, SocketFlags.Peek);
                    Line = Encoding.ASCII.GetString(textBuffer, 0, avail);
                }
                else
                {
                    Xbox.xboxName.Client.Receive(textBuffer, textBuffer.Length, SocketFlags.Peek);
                    Line = Encoding.ASCII.GetString(textBuffer);
                }

                int eolIndex = Line.IndexOf("\r\n");
                if (eolIndex != -1)
                {
                    Xbox.xboxName.Client.Receive(textBuffer, eolIndex + 2, SocketFlags.None);
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

        public void RenameFile(string OldName, string NewName)
        {

        }

        public void ScreenShot(string Filename)
        {

        }



        public void SendTextCommand(string Command, out string Response)
        {
            // Send a text command. Always have \r\n at the end of commands
            Xbox.XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
            NetworkStream stream = Xbox.XboxName.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            Response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        }


        public void XNotify(string Text)
        {
            XNotify(Text, 34);
        }

        public void XNotify(string Text, uint Type)
        {
            object[] jRPCVersion = new object[] { "consolefeatures ver=", 2, " type=12 params=\"A\\0\\A\\2\\", 2, "/", Text.Length, "\\", Text.ToHexS(), "\\", 1, "\\", Type, "\\\"" };
            SendTextCommand(string.Concat(jRPCVersion));
        }

        public void SendBinary(uint connectionId, byte[] callData, uint length)
        {
            throw new NotImplementedException();
        }

        public void ReceiveBinary(uint connectionId, byte[] numArray, uint length, out uint bytesReceived)
        {
            throw new NotImplementedException();
        }

        public uint ConnectTimeout { get => Convert.ToUInt32(Xbox.xboxName.ReceiveTimeout); set => Xbox.xboxName.Client.SendTimeout = (int)value;}


        public uint ConversationTimeout { get => Convert.ToUInt32(Xbox.xboxName.ReceiveTimeout); set => Xbox.xboxName.SendTimeout = (int)value; }

        public IXboxDebugTarget DebugTarget => null;
        public IXboxAutomation XboxAutomation => null;


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

        public XBOX_PROCESS_INFO RunningProcessInfo
        {
            get;
        }

        public string Name => throw new NotImplementedException();
    }
}
