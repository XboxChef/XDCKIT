//Do Not Delete This Comment... 
//Made By Serenity on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2016.
//Thank You for looking love you guys...

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace XDCKIT
{
    /// <summary>
    /// Xbox Emulation Class
    /// Made By Serenity
    /// </summary>
    public partial class XboxConsole //XboxMemory Commands
    {
        private Stream _fStream;
        private BinaryWriter _bWriter;

        private uint _startDumpOffset { set; get; }

        public static bool Connect(string IP)
        {
            return XboxClient.Connect(IP);
        }

        /// <summary>
        /// Set or Get the start dump offset
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint DumpOffset { set { _startDumpOffset = value; } }


        /// <summary>
        /// Set or Get the dump length
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public uint DumpLength { set; get; }
        public byte[] ReadMemory(uint Address, uint Length)
        {
            List<byte> ReturnData = new List<byte>();
            byte[] Packet = new byte[1026];
            byte[] PacketData = new byte[1024];

            // Send getmemex command.
            XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), Length.ToString("X2"))));

            // Receieve the 203 response to verify we are going to receive raw data in packets
            XboxClient.XboxName.Client.Receive(Packet);

            if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "203")
                throw new Exception("GETMEMEX 203 response not recieved. Cannot read memory.");

            // It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
            // Length / 1024 will get how many packets there are to receive
            for (uint i = 0; i < Length / 1024; i++)
            {
                XboxClient.XboxName.Client.Receive(Packet);

                // Store the data minus the first two bytes
                // This was a cheap way of removing the 2 byte header
                Array.Copy(Packet, 2, PacketData, 0, 1024);
                ReturnData.AddRange(PacketData);


            }

            // Get the remainder of Length / 1024 to see if we are receiving extra.
            uint Remainder = (Length % 1024);

            // If there is a remainder, read it
            if (Remainder > 0)
            {
                XboxClient.XboxName.Client.Receive(Packet);

                // Store the data minus the first two bytes
                // This was a cheap way of removing the 2 byte header
                Array.Copy(Packet, 2, PacketData, 0, 1024);
                ReturnData.AddRange(PacketData);
            }

            return ReturnData.ToArray();
        }
        public void DumpMemory(uint Address, uint Length, string FileName)
        {
            byte[] Packet = new byte[1026];

            // Send getmemex command.
            XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), Length.ToString("X2"))));

            // Receieve the 203 response to verify we are going to recieve raw data in packets
            XboxClient.XboxName.Client.Receive(Packet);

            if (Encoding.ASCII.GetString(Packet).Replace("\0", "").Substring(0, 3) != "203")
                throw new Exception("GETMEMEX 203 response not recieved. Cannot read memory.");

            FileStream outfile = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

            // It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
            // Length / 1024 will get how many packets there are to recieve
            for (uint i = 0; i < Length / 1024; i++)
            {
                XboxClient.XboxName.Client.Receive(Packet);

                // Write the data minus the first two bytes
                outfile.Write(Packet, 2, 1024);


            }

            // Get the remainder of Length / 1024 to see if we are recieving extra.
            uint Remainder = (Length % 1024);

            // If there is a remainder, read it
            if (Remainder > 0)
            {
                XboxClient.XboxName.Client.Receive(Packet);

                // Write the data minus the first two bytes
                outfile.Write(Packet, 2, 1024);
            }


            // Flush and close the file
            outfile.Flush();
            outfile.Close();
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void InvalidateMemoryCache(bool ExecutablePages, uint Address, uint Size)
        {
            //TODO:
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void SetBreakpoint(uint Address)
        {

        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void RemoveBreakpoint(uint Address)
        {

        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void RemoveAllBreakpoints()
        {

        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void SetInitialBreakpoint()
        {

        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void SetDataBreakpoint(uint Address, XboxBreakpointType Type, uint Size)
        {

        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IsBreakpoint(uint Address, out XboxBreakpointType Type)
        {
            Type = XboxBreakpointType.NoBreakpoint;
        }
        public void NULLAddress(uint address, uint Size)
        {

            byte[] buffer1 = new byte[Size];
            buffer1[0] = 0x60;
            SetMemory(address, buffer1);
        }
        public void Responses(string type)
        {

        }
        /// <summary>
        /// Sends Commands Based On User's Input
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        public static string SendTextCommand(string Command)
        {
            byte[] Packet = new byte[1026];
            if (XboxClient.XboxName != null)
            {

                try
                {
                    XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + "\r\n"));
                    Thread.Sleep(1000);
                    XboxClient.XboxName.Client.Receive(Packet);
                    Console.WriteLine(Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Replace("\r\n", string.Empty).Replace("\"", string.Empty).Replace("202- multiline response follows\n", string.Empty).Replace("201- connected\n", string.Empty.Replace("200-", string.Empty)));
                    return Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Replace("\r\n", string.Empty).Replace("\"", string.Empty).Replace("202- multiline response follows\n", string.Empty).Replace("201- connected\n", string.Empty.Replace("200-", string.Empty));//


                }
                catch
                {
                    throw;
                }
            }
            else throw new Exception("No Connection Detected");
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends Commands Based On User's Input
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        public void SendTextCommand(string command, params object[] args)
        {
            if (XboxClient.XboxName != null)
            {

                try
                {
                    XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format(command, args) + Environment.NewLine));
                }
                catch
                {

                }
            }
            else throw new Exception("No Connection Detected");
        }
        /// <summary>
        /// Sends Commands Based On User's Input
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="response"></param>
        public static void SendTextCommand(string Command, out string response)//TODO: Make A More Efficient string Remover
        {
            response = string.Empty;
            try
            {
                // Max packet size is 1026
                byte[] Packet = new byte[1026];
                if (XboxClient.Connected == true)
                {
                    Console.WriteLine("SendTextCommand " + Command + " ==> Sending Command... <==");
                    XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(Command + Environment.NewLine));
                    Thread.Sleep(1000);
                    XboxClient.XboxName.Client.Receive(Packet);
                    response = Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Replace("\r\n", string.Empty).Replace("\"", string.Empty).Replace("202- multiline response follows\n", string.Empty).Replace("201- connected\n", string.Empty.Replace("200-", string.Empty));//
                    response = response.Substring(0, response.Length - 1);
                    Response = response;

                }
                else
                {
                    XboxClient.XboxName = new TcpClient(XboxClient.IPAddress, XboxClient.Port);
                }

            }
            catch
            {

            }
        }

        public void PokeMemory(uint Address, string Value)
        {
            SetMemory(Address, Value);
        }
        public void PokeMemory(uint Address, byte[] Data)
        {
            SetMemory(Address, Data);
        }
        /// <summary>
        /// Poke the Memory
        /// </summary>
        /// <param name="memoryAddress">The memory address to Poke Example:0xCEADEADE - Uses *.FindOffset</param>
        /// <param name="value">The value to poke Example:000032FF (hex string)</param>
        public void Poke(uint memoryAddress, string value)
        {
            SetMemory((uint)memoryAddress, value);
        }
        public void PokeMemory(uint offset, string poketype, string amount)
        {
            if (Connected)
            {
                try
                {

                    if (poketype == "Unicode String")
                    {
                        //IO.Out.WriteUnicodeString(amount, amount.Length);
                    }
                    if (poketype == "ASCII String")
                    {
                        //IO.Out.WriteUnicodeString(amount, amount.Length);
                    }
                    if ((poketype == "String") | (poketype == "string"))
                    {
                        SetMemory(offset, amount);
                    }
                    if (poketype.ToLower() == "float")
                    {
                        SetMemory(offset, float.Parse(amount).ToString());
                    }
                    if (poketype.ToLower() == "double")
                    {
                        //SetMemory(offset, double.Parse(amount));
                    }
                    if (poketype.ToLower() == "short")
                    {
                        //SetMemory((short)Convert.ToUInt32(amount, 0x10));
                    }
                    if (poketype.ToLower() == "byte")
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(amount);
                        PokeMemory(offset, bytes);
                    }
                    if (poketype.ToLower() == "long")
                    {
                        //SetMemory(offset, (long)Convert.ToUInt32(amount, 0x10));
                    }
                    if (poketype.ToLower() == "quad")
                    {
                        // SetMemory(offset, (long)Convert.ToUInt64(amount, 0x10));
                    }
                    if (poketype.ToLower() == "int")
                    {
                        SetMemory(offset, Convert.ToUInt32(amount, 0x10).ToString());
                    }
                    Console.WriteLine("Poked", "Successfully poked the " + poketype + " " + amount + " to the offset 0x" + offset.ToString("X"));
                }
                catch
                {
                    Console.WriteLine("Error", "Couldn't poke XDK. ");//SetMemory(offset, amount);
                }
            }
            else
            {
                Console.WriteLine("Error", "You are not connected to your XDK. ");
            }
        }

        public string PeekXbox(uint offset, PeekType type)
        {
            switch (type)
            {
                case PeekType.String:
                    return ReadString(offset);
                case PeekType.Float:
                    return GetMemory(offset, 10).ToString();
                case PeekType.Double:
                    return ReadDouble(offset).ToString("X");
                case PeekType.Short:
                    return ReadInt16(offset).ToString("X");
                case PeekType.Byte:
                    return ReadByte(offset).ToString("X");
                case PeekType.Long:
                    return ReadInt32(offset).ToString("X");
                case PeekType.Quad:
                    return ReadInt64(offset).ToString("X");
                default:
                    return null;


            }

        }
        public void constantMemorySet(uint Address, uint Value)
        { constantMemorySetting(Address, Value, false, 0, false, 0); }

        public void constantMemorySet(uint Address, uint Value, uint TitleID)
        { constantMemorySetting(Address, Value, false, 0, true, TitleID); }

        public void constantMemorySet(uint Address, uint Value, uint IfValue, uint TitleID)
        { constantMemorySetting(Address, Value, true, IfValue, true, TitleID); }

        public void constantMemorySetting(uint Address, uint Value, bool useIfValue, uint IfValue, bool usetitleID, uint TitleID)
        {
            object[] Version = new object[]
            {
                "consolefeatures ver=",
                2,
                " type=18 params=\"A\\",
                Address.ToString("X"),
                "\\A\\5\\",
                1,
                "\\",
                XboxExtention.UIntToInt(Value),
                "\\",
                1,
                "\\",
                (useIfValue ? 1 : 0),
                "\\",
                1,
                "\\",
                IfValue,
                "\\",
                1,
                "\\",
                (usetitleID ? 1 : 0),
                "\\",
                1,
                "\\",
                XboxExtention.UIntToInt(TitleID),
                "\\\""
            };
            SendTextCommand(string.Concat(Version));
        }

        public void SetMemory(uint address, string data)
        {

            int sent = 0;
            try
            {
                Thread.Sleep(1000);
                // Send the setmem command
                XboxClient.XboxName.Client
                    .Send(Encoding.ASCII
                        .GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n", address.ToString("X2"), data)));
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

        public void SetMemory(uint Address, byte[] Data) { SetMemory(Address, 0, Data, out _); }

        public void SetMemory(uint Address, uint BytesToWrite, byte[] Data, out uint BytesWritten)//aka response
        {

            // Send the setmem command
            XboxClient.XboxName.Client
                .Send(Encoding.ASCII
                    .GetBytes(string.Format("SETMEM ADDR=0x{0} DATA={1}\r\n",
                                            Address.ToString("X2"),
                                            BitConverter.ToString(Data).Replace("-", string.Empty))));

            // Check to see our response
            byte[] Packet = new byte[1026];
            Thread.Sleep(1000);
            XboxClient.XboxName.Client.Receive(Packet);
            BytesWritten = Convert.ToUInt32(Encoding.ASCII.GetString(Packet));
            if (Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Substring(0, 11) == "0 bytes set")
                throw new Exception("A problem occurred while writing bytes. 0 bytes set");
        }


        public static byte[] GetMemory(uint Address, uint Length)
        {
            GetMemory(Address, Length, new byte[Length], out _);
            InvalidateMemoryCache(true, Address, Length);
            return new byte[Length];

        }
        public static void GetMemory(uint Address, uint BytesToRead, byte[] Data, out uint BytesRead)
        {

            BytesRead = 0;
            List<byte> ReturnData = new List<byte>();
            byte[] Packet = new byte[1026];
            Data = new byte[1024];
            Thread.Sleep(1000);
            // Send getmemex command.

            XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR=0x{0} LENGTH=0x{1}\r\n", Address.ToString("X2"), BytesToRead.ToString("X2"))));

            // Receieve the 203 response to verify we are going to recieve raw data in packets
            XboxClient.XboxName.Client.Receive(Packet);

            if (Encoding.ASCII.GetString(Packet).Replace("\0", string.Empty).Substring(0, 3) != "203")
                Console.WriteLine("GETMEMEX 203 response not recieved. Cannot read memory.");

            // It will return with data in 1026 byte size packets, first two bytes I think are flags and the rest is the data
            // Length / 1024 will get how many packets there are to recieve
            for (uint i = 0; i < BytesToRead / 1024; i++)
            {
                XboxClient.XboxName.Client.Receive(Packet);

                // Store the data minus the first two bytes
                // This was a cheap way of removing the 2 byte header
                Array.Copy(Packet, 2, Data, 0, 1024);
                ReturnData.AddRange(Data);
            }
        }

        /// <summary>
        /// Waits for a specified amount of data to be received.  Use with file IO.
        /// </summary>
        /// <param name="targetLength">Amount of data to wait for</param>
        public static void Wait(int targetLength)
        {
            if (XboxClient.XboxName != null)
            {
                if (XboxClient.XboxName.Available < targetLength) // avoid waiting if we already have data in our buffer...
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    while (XboxClient.XboxName.Available < targetLength)
                    {
                        Thread.Sleep(0);
                        if (sw.ElapsedMilliseconds > 5000)
                        {

                        }
                    }
                }
            }
            else
                throw new Exception("No Connection Detected");
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
            if (XboxClient.XboxName != null)
            {
                Stopwatch sw = Stopwatch.StartNew();
                switch (type)
                {
                    // waits for data to start being received
                    case WaitType.Partial:
                        while (XboxClient.XboxName.Available == 0)
                        {
                            Thread.Sleep(0);
                            if (sw.ElapsedMilliseconds > 5000)
                            {

                            }
                        }
                        break;

                    // waits for data to start and then stop being received
                    case WaitType.Full:

                        // do a partial wait first
                        while (XboxClient.XboxName.Available == 0)
                        {
                            Thread.Sleep(0);
                            if (sw.ElapsedMilliseconds > 5000)
                            {

                            }
                        }

                        // wait for rest of data to be received
                        int avail = XboxClient.XboxName.Available;
                        Thread.Sleep(0);
                        while (XboxClient.XboxName.Available != avail)
                        {
                            avail = XboxClient.XboxName.Available;
                            Thread.Sleep(0);
                        }
                        break;

                    // waits for data to stop being received
                    case WaitType.Idle:
                        int before = XboxClient.XboxName.Available;
                        Thread.Sleep(0);
                        while (XboxClient.XboxName.Available != before)
                        {
                            before = XboxClient.XboxName.Available;
                            Thread.Sleep(0);
                            if (sw.ElapsedMilliseconds > 5000)
                            {

                            }
                        }
                        break;
                }
            }
            else
                throw new Exception("No Connection Detected");
        }
        private bool GetMeMex(uint startDump, uint length)
        {
            if (Connected) return true;
            //ADDR=0xDA1D0000 - The start offset in the physical memory I want the dump to start
            //LENGTH = Length of the dump
            XboxClient.XboxName.Client.Send(
                Encoding.ASCII.GetBytes(string.Format("GETMEMEX ADDR={0} LENGTH={1}\r\n", startDump, length)));
            var response = new byte[1024];
            XboxClient.XboxName.Client.Receive(response);
            string reponseString = Encoding.ASCII.GetString(response).Replace("\0", "");
            //validate connection
            return reponseString.Substring(0, 3) == "203";
        }
        /// <summary>
        /// Dump the memory
        /// </summary>
        /// <param name="filename">The file to save to</param>
        /// <param name="startDumpAddress">The start dump address</param>
        /// <param name="dumpLength">The dump length</param>
        public void Dump(string filename, uint startDumpAddress, uint dumpLength)
        {

            if (!Connected)
                return; //Call function - If not connected return
            if (!GetMeMex(startDumpAddress, dumpLength))
                return; //call function - If not connected or if something wrong return
            _fStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            _bWriter = new BinaryWriter(_fStream);
            try
            {
                var data = new byte[1026]; //byte chuncks
                //Writing each byte chuncks========
                for (int i = 0; i < dumpLength / 1024; i++)
                {
                    XboxClient.XboxName.Client.Receive(data);
                    _bWriter.Write(data, 2, 1024);
                }
                //Write whatever is left
                var extra = (int)(dumpLength % 1024);
                if (extra > 0)
                {
                    XboxClient.XboxName.Client.Receive(data);
                    _bWriter.Write(data, 2, extra);
                }
                _fStream.Flush();
                _bWriter.Flush();
            }
            catch (SocketException)
            {
                _fStream.Flush();
                _bWriter.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void WriteHook(uint Offset, uint Destination, bool Linked)
        {
            uint[] Func = new uint[4];
            if ((Destination & 0x8000) != 0)
                Func[0] = 0x3D600000 + (((Destination >> 16) & 0xFFFF) + 1);
            else
                Func[0] = 0x3D600000 + ((Destination >> 16) & 0xFFFF);
            Func[1] = 0x396B0000 + (Destination & 0xFFFF);
            Func[2] = 0x7D6903A6;
            Func[3] = 0x4E800420;
            if (Linked)
                Func[3]++;
            byte[] buffer = new byte[0x10];
            byte[] f1 = BitConverter.GetBytes(Func[0]);
            byte[] f2 = BitConverter.GetBytes(Func[1]);
            byte[] f3 = BitConverter.GetBytes(Func[2]);
            byte[] f4 = BitConverter.GetBytes(Func[3]);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(f1);
                Array.Reverse(f2);
                Array.Reverse(f3);
                Array.Reverse(f4);
            }
            for (int i = 0; i < 4; i++)
                buffer[i] = f1[i];
            for (int i = 4; i < 8; i++)
                buffer[i] = f2[i - 4];
            for (int i = 8; i < 0xC; i++)
                buffer[i] = f3[i - 8];
            for (int i = 0xC; i < 0x10; i++)
                buffer[i] = f4[i - 0xC];
            WriteByte(Offset, buffer);
        }
        public struct Vector
        {
            public float x, y, z;
        }
        public void WriteVector1(uint Offset, Vector Vector)
        {
            byte[] bytes = new byte[8];
            byte[] x = BitConverter.GetBytes(Vector.x);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            WriteByte(Offset, bytes);
        }
        public void WriteVector2(uint Offset, Vector Vector)
        {
            byte[] bytes = new byte[8];
            byte[] x = BitConverter.GetBytes(Vector.x);
            byte[] y = BitConverter.GetBytes(Vector.y);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
                Array.Reverse(y);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            Array.Copy(y, 0, bytes, 4, 4);
            WriteByte(Offset, bytes);
        }
        public void WriteVector3(uint Offset, Vector Vector)
        {
            byte[] bytes = new byte[12];
            byte[] x = BitConverter.GetBytes(Vector.x);
            byte[] y = BitConverter.GetBytes(Vector.y);
            byte[] z = BitConverter.GetBytes(Vector.z);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
                Array.Reverse(y);
                Array.Reverse(z);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            Array.Copy(y, 0, bytes, 4, 4);
            Array.Copy(z, 0, bytes, 8, 4);
            WriteByte(Offset, bytes);
        }
        private uint SendBinary(string Buffer, int Lenght)
        {
            if (XboxClient.XboxName != null)
            {
                return (uint)XboxClient.XboxName.Client.Send(Encoding.ASCII.GetBytes(Buffer), Lenght, SocketFlags.None);

            }
            else
            {
                throw new Exception("Connection Not Found");
            }
        }

        private void ScreenShot(int a1)
        {

        }

        private int ReceiveStatusResponse(TimeSpan timeout, string Arg, int Response)
        {
            string result = ReceiveSocketLine();
            if (result.Length >= 0)
            {
                return Status_Translator(Arg);
            }
            else
            {
                return result.Length;
            }


        }
        private int Status_Translator(string inputstring)
        {
            //char v2;  bl
            //char v3;  cl
            if (inputstring.Length != 50 && inputstring.Length != 52)
            {
                return -2099642102;
            }
            if (inputstring[1] < 48)
            {
                return -2099642102;
            }
            if (inputstring[1] > 57)
            {
                return -2099642102;
            }
            if (inputstring[2] < 48 || inputstring[2] > 57 || inputstring[3] != 45)
            {
                return -2099642102;
            }
            if (inputstring.Length == 52)
            {
                return (int)((inputstring[2] + 10 * (inputstring[1] + 520) - 5728) | 0x82DA0000);
            }
            return (inputstring[2] + 10 * (inputstring[1] + 10 * inputstring.Length) - 5528) | 0x2DA0000;
        }
        /// <summary>
        /// Waits for a line of text to be received from the xbox.
        /// </summary>
        /// <returns></returns>
        public string ReceiveSocketLine()
        {
            if (XboxClient.XboxName != null)
            {
                string lineText;
                byte[] textBuffer = new byte[256];  // buffer large enough to contain a line of text

                Wait(WaitType.Partial); // wait for data to appear in the receive buffer

                Stopwatch sw = Stopwatch.StartNew();
                while (true)
                {
                    int avail = XboxClient.XboxName.Available;   // only get once
                    if (avail < textBuffer.Length)
                    {
                        XboxClient.XboxName.Client.Receive(textBuffer, avail, SocketFlags.Peek);
                        lineText = ASCIIEncoding.ASCII.GetString(textBuffer, 0, avail);
                    }
                    else
                    {
                        XboxClient.XboxName.Client.Receive(textBuffer, textBuffer.Length, SocketFlags.Peek);
                        lineText = ASCIIEncoding.ASCII.GetString(textBuffer);
                    }

                    int eolIndex = lineText.IndexOf("\r\n");
                    if (eolIndex != -1)
                    {
                        XboxClient.XboxName.Client.Receive(textBuffer, eolIndex + 2, SocketFlags.None);
                        return ASCIIEncoding.ASCII.GetString(textBuffer, 0, eolIndex);
                    }

                    // end of line not found yet, lets wait some more...
                    Thread.Sleep(0);
                    if (sw.ElapsedMilliseconds > 5000)
                    {
                        if (!Ping(250)) Disconnect();  // only disconnect if actually disconnected
                        throw new TimeoutException();
                    }
                }
            }
            else throw new Exception("No Connection Detected...");
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
                if (XboxClient.XboxName != null)
                {
                    if (XboxClient.XboxName.Available > 0)
                        XboxClient.XboxName.Client.Receive(new byte[XboxClient.XboxName.Available]);

                    XboxClient.XboxName.Client.Send(ASCIIEncoding.ASCII.GetBytes(Environment.NewLine));
                    XboxClient.XboxName.SendTimeout = waitTime;
                    FlushSocketBuffer(16);    // throw out garbage response "400- Unknown Command\r\n"
                    Connected = true;
                    return true;
                }
                return false;
            }
            catch
            {
                Connected = false;
                XboxClient.XboxName.Close();
                return false;
            }
            finally
            {
                XboxClient.XboxName.SendTimeout = oldTimeOut;   // make sure to restore old timeout
            }
        }
        public string memstat()
        {
            return SendTextCommand("consolemem");
        }
        public void CMDLine(string Command)
        {
            SendTextCommand("cmdline=\""+Command);
        }
        public void NOTIFYAT(string port)
        {
            SendTextCommand("NOTIFYAT PORT=" + port);
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
                    XboxClient.XboxName.Client.Receive(new byte[size]);
                }
                catch { Connected = false; }
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
            string Command = "consolefeatures ver=" + (uint)2 + " type=9 params=\"A\\0\\A\\2\\" + (uint)2 + "/" + ModuleName.Length + "\\" + ModuleName.ToHexString() + "\\" + (uint)1 + "\\" + Ordinal + "\\\"";
            SendTextCommand(Command);
            string String = SendTextCommand(Command);

            try
            {
                return uint.Parse(String.Substring(String.find(" ") + 1), NumberStyles.HexNumber);
            }
            catch
            {
                Console.WriteLine("SendCommand Return String.Empty");
                return 0;
            }
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
    }
}