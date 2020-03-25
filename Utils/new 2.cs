// PhantomRTM.NET version 1.8.1
// Copyright (c) 2016 Joshua Ozeri
//
// You are free to use this code in your software
//  however you are not permitted to sell this
//  code. Give credit where it's due.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace XeClutch
{
    public class PhantomRTM
    {
        // Enumerations
        public enum ConsoleColor
        {
            Black,
            Blue,
            BlueGray,
            White,
        };
        public enum ExecutionState
        {
            Pending,
            Reboot,
            Start,
            Stop,
            TitlePending,
            TitleReboot,
            Unknown
        };

        // Structures
        public struct HardwareInfo
        {
            public uint Flags;
            public byte NumberOfProcessors, PCIBridgeRevisionID;
            public byte[] ReservedBytes;
            public ushort BldrMagic, BldrFlags;
        }
        public struct Vector2
        {
            public float x, y;
        }
        public struct Vector3
        {
            public float x, y, z;
        }

        // Variables
        private TcpClient client;
        public bool Connected;
        private StreamReader sreader;

        // Methods
        private string HexString2Ascii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            return sb.ToString();
        }
        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                          .Where(x => x % 2 == 0)
                          .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                          .ToArray();
        }
        public void Command_Eject()
        {
            Command_SendText("dvdeject");
        }
        public void Command_Reboot()
        {
            Command_SendText("magicboot cold");
            Connected = false;
            client.Client.Close();
        }
        public string[] Command_SendText(string Text)
        {
            if (!Connected)
                return new string[] { "" };
            new BinaryWriter(client.GetStream()).Write(Encoding.ASCII.GetBytes(Text + "\r\n"));
            return sreader.ReadToEnd().Split("\n".ToCharArray());
        }
        public void Command_Shutdown()
        {
            Command_SendText("shutdown");
            Connected = false;
            client.Client.Close();
        }
        public bool Connect(string XboxIP)
        {
            try
            {
                client = new TcpClient(XboxIP, 730);
                sreader = new StreamReader(client.GetStream());
                return Connected = (sreader.ReadLine().ToString().ToLower() == "201- connected");
            }
            catch
            {
                return false;
            }
        }
        public bool Disconnect()
        {
            try
            {
                new BinaryWriter(client.GetStream()).Write(Encoding.ASCII.GetBytes("bye\r\n"));
                Connected = false;
                client.Client.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Debug_Freeze()
        {
            Command_SendText("stop");
        }
        public void Debug_UnFreeze()
        {
            Command_SendText("go");
        }
        public void File_Delete(string Path)
        {
            string[] lines = Path.Split("\\".ToCharArray());
            string Directory = "";
            for (int i = 0; i < (lines.Length - 1); i++)
                Directory += lines[i] + "\\";
            Command_SendText("delete title=\"" + Path + "\" dir=\"" + Directory + "\"");
        }
        public void File_Launch(string Path)
        {
            string[] lines = Path.Split("\\".ToCharArray());
            string Directory = "";
            for (int i = 0; i < lines.Length - 1; i++)
                Directory += lines[i] + "\\";
            Command_SendText("magicboot title=\"" + Path + "\" directory=\"" + Directory + "\"");
        }
        public string Get_ConsoleID()
        {
            return Command_SendText("getconsoleid")[0].Replace("200- consoleid=", "");
        }
        public string Get_CPUKey()
        {
            return Command_SendText("getcpukey")[0].Replace("200- ", "");
        }
        public string Get_DebugName()
        {
            return Command_SendText("dbgname")[0].Replace("200- ", "");
        }
        public ExecutionState Get_ExecutionState()
        {
            string str = Command_SendText("getexecstate")[0].Replace("200- ", "");
            if (str == "pending")
                return ExecutionState.Pending;
            else if (str == "reboot")
                return ExecutionState.Reboot;
            else if (str == "start")
                return ExecutionState.Start;
            else if (str == "stop")
                return ExecutionState.Stop;
            else if (str == "pending_title")
                return ExecutionState.TitlePending;
            else if (str == "reboot_title")
                return ExecutionState.TitleReboot;
            return ExecutionState.Unknown;
        }
        public HardwareInfo Get_HardwareInfo()
        {
            string[] lines = Command_SendText("hwinfo");
            HardwareInfo info;
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
        public uint Get_ProcessID()
        {
            return uint.Parse(Command_SendText("getpid")[0].Replace("200- pid=", "").Replace("0x", ""), NumberStyles.HexNumber);
        }
        public uint Get_TitleAddress()
        {
            return uint.Parse(Command_SendText("altaddr")[0].Replace("200- addr=", "").Replace("0x", ""), NumberStyles.HexNumber);
        }
        public bool Read_Bool(uint Offset)
        {
            return Convert.ToBoolean(Read_Byte(Offset, 1));
        }
        public byte Read_Byte(uint Offset)
        {
            return Read_Byte(Offset, 1)[0];
        }
        public byte[] Read_Byte(uint Offset, uint Length)
        {
            if (!Connected)
            {
                byte[] ret = new byte[Length];
                return ret;
            }
            int size = (int)Length;
            string buffer = "";
            while (size > 0)
            {
                if (size > 0x40)
                {
                    new BinaryWriter(client.GetStream()).Write(Encoding.ASCII.GetBytes("getmem addr=" + (Offset + (Length - size)) + " length=64\r\n"));
                    while (sreader.ReadLine() != "202- memory data follows") ;
                    sreader.ReadLine();
                    buffer += sreader.ReadLine();
                    size -= 0x40;
                }
                else
                {
                    new BinaryWriter(client.GetStream()).Write(Encoding.ASCII.GetBytes("getmem addr=" + (Offset + (Length - size)) + " length=" + size + "\r\n"));
                    while (sreader.ReadLine() != "202- memory data follows") ;
                    sreader.ReadLine();
                    buffer += sreader.ReadLine();
                    break;
                }
            }
            return StringToByteArray(buffer);
        }
        public char Read_Char(uint Offset)
        {
            return (char)Read_Byte(Offset);
        }
        public double Read_Double(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 8);
            Array.Reverse(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }
        public void Read_File(uint Offset, uint Length, string Path)
        {
            File.WriteAllBytes(Path, Read_Byte(Offset, Length));
        }
        public float Read_Float(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 4);
            Array.Reverse(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }
        public short Read_Int16(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 2);
            Array.Reverse(buffer);
            return BitConverter.ToInt16(buffer, 0);
        }
        public int Read_Int32(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 4);
            Array.Reverse(buffer);
            return BitConverter.ToInt32(buffer, 0);
        }
        public long Read_Int64(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 8);
            Array.Reverse(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }
        public sbyte Read_SByte(uint Offset)
        {
            return (sbyte)Read_Byte(Offset);
        }
        public string Read_String(uint Offset, uint Length)
        {
            byte[] buffer = Read_Byte(Offset, Length);
            string str = "";
            foreach (byte bit in buffer)
                str += bit.ToString("X2");
            return HexString2Ascii(str);
        }
        public ushort Read_UInt16(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 2);
            Array.Reverse(buffer);
            return BitConverter.ToUInt16(buffer, 0);
        }
        public uint Read_UInt32(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 4);
            Array.Reverse(buffer);
            return BitConverter.ToUInt32(buffer, 0);
        }
        public ulong Read_UInt64(uint Offset)
        {
            byte[] buffer = Read_Byte(Offset, 8);
            Array.Reverse(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
        public Vector2 Read_Vector2(uint Offset)
        {
            Vector2 vec;
            vec.x = Read_Float(Offset);
            vec.y = Read_Float(Offset + 4);
            return vec;
        }
        public Vector3 Read_Vector3(uint Offset)
        {
            Vector3 vec;
            vec.x = Read_Float(Offset);
            vec.y = Read_Float(Offset + 4);
            vec.z = Read_Float(Offset + 8);
            return vec;
        }
        public uint Search_Bool(uint StartOffset, uint Length, bool Bool)
        {
            return Search_Byte(StartOffset, Length, (byte)(Bool ? 0x01 : 0x00));
        }
        public uint Search_Byte(uint StartOffset, uint Length, byte Byte)
        {
            return Search_Byte(StartOffset, Length, new byte[] { Byte });
        }
        public uint Search_Byte(uint StartOffset, uint Length, byte[] Bytes)
        {
            if (!Connected)
                return 0;
            uint addr = StartOffset;
            byte[] data = Read_Byte(StartOffset, Length);
            for (uint i = 0; i < (Length - Bytes.Length); i++)
            {
                byte[] buffer = new byte[Bytes.Length];
                Array.Copy(data, i, buffer, 0, Bytes.Length);
                if (buffer.SequenceEqual(Bytes))
                {
                    addr += i;
                    break;
                }
            }
            return addr;
        }
        public uint Search_Char(uint StartOffset, uint Length, char Char)
        {
            return Search_Byte(StartOffset, Length, (byte)Char);
        }
        public uint Search_Double(uint StartOffset, uint Length, double Double)
        {
            byte[] bytes = BitConverter.GetBytes(Double);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_Float(uint StartOffset, uint Length, float Float)
        {
            byte[] bytes = BitConverter.GetBytes(Float);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_Int16(uint StartOffset, uint Length, short Int16)
        {
            byte[] bytes = BitConverter.GetBytes(Int16);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_Int32(uint StartOffset, uint Length, int Int32)
        {
            byte[] bytes = BitConverter.GetBytes(Int32);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_Int64(uint StartOffset, uint Length, long Int64)
        {
            byte[] bytes = BitConverter.GetBytes(Int64);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_String(uint StartOffset, uint Length, string String)
        {
            return Search_Byte(StartOffset, (uint)String.Length, Encoding.ASCII.GetBytes(String));
        }
        public uint Search_SByte(uint StartOffset, uint Length, sbyte SByte)
        {
            return Search_Byte(StartOffset, Length, (byte)SByte);
        }
        public uint Search_UInt16(uint StartOffset, uint Length, ushort UInt16)
        {
            byte[] bytes = BitConverter.GetBytes(UInt16);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_UInt32(uint StartOffset, uint Length, uint UInt32)
        {
            byte[] bytes = BitConverter.GetBytes(UInt32);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_UInt64(uint StartOffset, uint Length, ulong UInt64)
        {
            byte[] bytes = BitConverter.GetBytes(UInt64);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_Vector2(uint StartOffset, uint Length, Vector2 Vector2)
        {
            byte[] bytes = new byte[8];
            byte[] x = BitConverter.GetBytes(Vector2.x);
            byte[] y = BitConverter.GetBytes(Vector2.y);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
                Array.Reverse(y);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            Array.Copy(y, 0, bytes, 4, 4);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public uint Search_Vector3(uint StartOffset, uint Length, Vector3 Vector3)
        {
            byte[] bytes = new byte[12];
            byte[] x = BitConverter.GetBytes(Vector3.x);
            byte[] y = BitConverter.GetBytes(Vector3.y);
            byte[] z = BitConverter.GetBytes(Vector3.z);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
                Array.Reverse(y);
                Array.Reverse(z);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            Array.Copy(y, 0, bytes, 4, 4);
            Array.Copy(z, 0, bytes, 8, 4);
            return Search_Byte(StartOffset, Length, bytes);
        }
        public void Set_ConsoleColor(ConsoleColor Color)
        {
            Command_SendText("setcolor name=" + System.Enum.GetName(typeof(int), Color).ToLower());
        }
        public void Set_DebugName(string DebugName)
        {
            Command_SendText("dbgname name=" + DebugName);
        }
        public void Write_Bool(uint Offset, bool Bool)
        {
            Write_Byte(Offset, (byte)(Bool ? 0x01 : 0x00));
        }
        public void Write_Byte(uint Offset, byte Byte)
        {
            Write_Byte(Offset, new byte[] { Byte });
        }
        public void Write_Byte(uint Offset, byte[] Bytes)
        {
            if (!Connected)
                return;
            string str = "setmem addr=0x" + Offset.ToString("X8") + " data=";
            foreach (byte b in Bytes)
                str += b.ToString("X2");
            str += "\r\n";
            new BinaryWriter(client.GetStream()).Write(Encoding.ASCII.GetBytes(str));
        }
        public void Write_Char(uint Offset, char Char)
        {
            Write_Byte(Offset, (byte)Char);
        }
        public void Write_Double(uint Offset, double Double)
        {
            byte[] bytes = BitConverter.GetBytes(Double);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_File(uint Offset, string Path)
        {
            Write_Byte(Offset, File.ReadAllBytes(Path));
        }
        public void Write_Float(uint Offset, float Float)
        {
            byte[] bytes = BitConverter.GetBytes(Float);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_Hook(uint Offset, uint Destination, bool Linked)
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
            Write_Byte(Offset, buffer);
        }
        public void Write_Int16(uint Offset, short Int16)
        {
            byte[] bytes = BitConverter.GetBytes(Int16);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_Int32(uint Offset, int Int32)
        {
            byte[] bytes = BitConverter.GetBytes(Int32);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_Int64(uint Offset, long Int64)
        {
            byte[] bytes = BitConverter.GetBytes(Int64);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_NOP(uint Offset)
        {
            Write_Byte(Offset, new byte[] { 0x60, 0x00, 0x00, 0x00 });
        }
        public void Write_String(uint Offset, string String)
        {
            Write_Byte(Offset, Encoding.ASCII.GetBytes(String));
        }
        public void Write_SByte(uint Offset, sbyte SByte)
        {
            Write_Byte(Offset, BitConverter.GetBytes(SByte));
        }
        public void Write_UInt16(uint Offset, ushort UInt16)
        {
            byte[] bytes = BitConverter.GetBytes(UInt16);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_UInt32(uint Offset, uint UInt32)
        {
            byte[] bytes = BitConverter.GetBytes(UInt32);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_UInt64(uint Offset, ulong UInt64)
        {
            byte[] bytes = BitConverter.GetBytes(UInt64);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            Write_Byte(Offset, bytes);
        }
        public void Write_Vector2(uint Offset, Vector2 Vector2)
        {
            byte[] bytes = new byte[8];
            byte[] x = BitConverter.GetBytes(Vector2.x);
            byte[] y = BitConverter.GetBytes(Vector2.y);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
                Array.Reverse(y);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            Array.Copy(y, 0, bytes, 4, 4);
            Write_Byte(Offset, bytes);
        }
        public void Write_Vector3(uint Offset, Vector3 Vector3)
        {
            byte[] bytes = new byte[12];
            byte[] x = BitConverter.GetBytes(Vector3.x);
            byte[] y = BitConverter.GetBytes(Vector3.y);
            byte[] z = BitConverter.GetBytes(Vector3.z);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(x);
                Array.Reverse(y);
                Array.Reverse(z);
            }
            Array.Copy(x, 0, bytes, 0, 4);
            Array.Copy(y, 0, bytes, 4, 4);
            Array.Copy(z, 0, bytes, 8, 4);
            Write_Byte(Offset, bytes);
        }
    }
}