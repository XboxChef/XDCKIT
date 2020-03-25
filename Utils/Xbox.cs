using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XDevkit.Utils
{
    /// <summary>
    /// Todo:
    /// </summary>
    public static class Xbox//will handle connection and commands and will use XDevkit as a gateway for commands to be passed threw
    {
        /// <summary>
        /// Gets the main connection used for pc to xbox communication.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static TcpClient xboxName = new TcpClient();
        public static string GivenIP;
        private static StreamReader sreader;

        public static IXboxConsole XConsole;
        /// <summary>
        /// Gets or sets the maximum waiting time given (in milliseconds) for a response.
        /// </summary>
        [Browsable(false)]
        public static int Timeout { get { return timeout; } set { timeout = value; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int timeout = 5000;

        /// Freezes/Stops Console.
        /// </summary>
        public static void Freeze_Console(bool FalseorTrue)
        {
            if (FalseorTrue == true)
            {
                string responses;
                SendTextCommand("stop", out responses);
            }
            else if (FalseorTrue == false)
            {
                string responses;
                SendTextCommand("go", out responses);
            }
        }
        /// <summary>
        /// Turns The Console's Default neighborhood Icon to any of the following...(black , blue , bluegray , nosidecar , white)
        /// </summary>
        /// <param name="Color"></param>
        public static void SetColor(string Color)
        {
            SendTextCommand($"setcolor name={Color}", out _);
        }
        public static void ConsoleColor(ConsoleColor Color)
        {
            SendTextCommand("setcolor name=" + System.Enum.GetName(typeof(int), Color).ToLower(),out _);
        }
        public  static string GetConsoleid()
        {
         string responses;
            SendTextCommand(string.Concat("getconsoleid"), out responses);
            return responses.Replace("200- consoleid=", "");
        }
        public static void DebugName(string DebugName)
        {
            SendTextCommand("dbgname name=" + DebugName,out _);
        }
        public static TcpClient XboxName
        {
            get
            {
                return xboxName;
            }
            set
            {
                xboxName = value;
            }
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
        /// <summary>
        /// Dont use this, higher-level methods are available.  Use GetDriveFreeSpace or GetDriveSize instead.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="FreeBytesAvailableToCaller"></param>
        /// <param name="TotalNumberOfBytes"></param>
        /// <param name="totalFreeBytes"></param>
        public static void GetDriveInformation(ushort drive, out ulong FreeBytesAvailableToCaller, out ulong TotalNumberOfBytes, out ulong totalFreeBytes)
        {
            FreeBytesAvailableToCaller = 0; TotalNumberOfBytes = 0; totalFreeBytes = 0;
            SendTextCommand("drivefreespace name=\"{0}\"", drive.ToString() + ":\\");

            string msg = ReceiveMultilineResponse();
            FreeBytesAvailableToCaller = Convert.ToUInt64(msg.Substring(msg.IndexOf("freetocallerlo") + 17, 8), 16);
            FreeBytesAvailableToCaller |= (Convert.ToUInt64(msg.Substring(msg.IndexOf("freetocallerhi") + 17, 8), 16) << 32);

            TotalNumberOfBytes = Convert.ToUInt64(msg.Substring(msg.IndexOf("totalbyteslo") + 15, 8), 16);
            TotalNumberOfBytes |= (Convert.ToUInt64(msg.Substring(msg.IndexOf("totalbyteshi") + 15, 8), 16) << 32);

            totalFreeBytes = Convert.ToUInt64(msg.Substring(msg.IndexOf("totalfreebyteslo") + 19, 8), 16);
            totalFreeBytes |= (Convert.ToUInt64(msg.Substring(msg.IndexOf("totalfreebyteshi") + 19, 8), 16) << 32);
        }
        public static string[] SendText(string Text)
        {

            new BinaryWriter(XboxName.GetStream()).Write(Encoding.ASCII.GetBytes(Text + "\r\n"));
            return sreader.ReadToEnd().Split("\n".ToCharArray());
        }
        public static XBOX_Hardware_Info HardwareInfo()
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

        private static void SendTextCommand(string v1, string v2)
        {
            XConsole.SendTextCommand(v1+v2, out _);
        }

        //public static bool Connect(this IXboxConsole console, out IXboxConsole Console, string XboxNameOrIP = "default")
        //{

        //    if (XboxNameOrIP == "default")
        //    {
        //        XboxNameOrIP = new XboxManager().DefaultConsole;
        //    }

        //    IXboxConsole xboxConsole = new XboxManager().OpenConsole(XboxNameOrIP);
        //    TcpClient ConsoleName;
        //    console = new XboxManager().OpenConsole(XboxNameOrIP);
        //    ConsoleName = new TcpClient();
        //    ConsoleName.Connect(GivenIP, 730);
        //    // First thing that XBDM does is send a packet to us when we connect

        //    // Max packet size is 1026
        //    byte[] Packet = new byte[1026];
        //    ConsoleName.Client.Receive(Packet);
        //    XboxName = ConsoleName;
        //    if (string.Compare(Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Substring(0, 3), "201", StringComparison.Ordinal) != 0)
        //    XBDM.Show("XBDM did not send connect message");
        //    Console = xboxConsole;
        //    return true;
        //}
        private static void SendTextCommand(string v, out string responses)
        {
            XConsole.SendTextCommand(v,out responses);
        }
        /// <summary>
        /// Receives multiple lines of text from the xbox.
        /// </summary>
        /// <returns></returns>
        public static string ReceiveMultilineResponse()
        {
                StringBuilder response = new StringBuilder();
                while (true)
                {
                    string line = ReceiveSocketLine() + Environment.NewLine;
                    if (line[0] == '.') break;
                    else response.Append(line);
                }
                return response.ToString();
        }
        public static string ReceiveSocketLine()
        {
            
                string Line;
                byte[] textBuffer = new byte[256];  // buffer large enough to contain a line of text

                Thread.Sleep(0);

                Stopwatch sw = Stopwatch.StartNew();
                while (true)
                {
                    int avail = xboxName.Available;   // only get once
                    if (avail < textBuffer.Length)
                    {
                        xboxName.Client.Receive(textBuffer, avail, SocketFlags.Peek);
                        Line = ASCIIEncoding.ASCII.GetString(textBuffer, 0, avail);
                    }
                    else
                    {
                        xboxName.Client.Receive(textBuffer, textBuffer.Length, SocketFlags.Peek);
                        Line = ASCIIEncoding.ASCII.GetString(textBuffer);
                    }

                    int eolIndex = Line.IndexOf("\r\n");
                    if (eolIndex != -1)
                    {
                        xboxName.Client.Receive(textBuffer, eolIndex + 2, SocketFlags.None);
                        return ASCIIEncoding.ASCII.GetString(textBuffer, 0, eolIndex);
                    }

                    // end of line not found yet, lets wait some more...
                    Thread.Sleep(0);
                }
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
    }

    }

