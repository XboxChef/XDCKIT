using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using XDevkit.Utils;

namespace XDevkit
{
    class XboxDebugTarget : IXboxDebugTarget
    {
        TcpClient xboxName = Xbox.xboxName;
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
}
