﻿//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...
using System;
using System.Net.Sockets;
using System.Text;

namespace XDevkit
{
    /// <summary>
    /// Xbox Emulation Class
    /// Made By TeddyHammer
    /// </summary>
    public partial class Xbox
    {
        #region Types {Get; Set;}
        #region x
        /// <summary>
        /// Sends binary data to the xbox.
        /// </summary>
        /// <param name="data"></param>
        public void SendBinaryData(byte[] data)
        {
            ConnectionCheck();
            FlushSocketBuffer();
            XboxName.Client.Send(data);
        }

        /// <summary>
        /// Sends binary data of specified length to the xbox.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        public void SendBinaryData(byte[] data, int length)
        {
            ConnectionCheck();
            FlushSocketBuffer();
            XboxName.Client.Send(data, length, SocketFlags.None);
        }

        /// <summary>
        /// Receives all available binary data sent from the xbox.
        /// </summary>
        /// <returns></returns>
        public byte[] ReceiveBinaryData()
        {
            if (XboxName.Available > 0)
            {
                byte[] binData = new byte[XboxName.Available];
                XboxName.Client.Receive(binData, binData.Length, SocketFlags.None);
                return binData;
            }
            else return null;
        }

        /// <summary>
        /// Receives binary data of specified size sent from the xbox.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public byte[] ReceiveBinaryData(int size)
        {
            Wait(size);
            byte[] binData = new byte[size];
            XboxName.Client.Receive(binData, binData.Length, SocketFlags.None);
            return binData;
        }

        /// <summary>
        /// Receives binary data of specified size sent from the xbox.
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveBinaryData(byte[] data)
        {
            Wait(data.Length);
            XboxName.Client.Receive(data, data.Length, SocketFlags.None);
        }

        /// <summary>
        /// Receives binary data of specified size sent from the xbox.
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveBinaryData(byte[] data, int offset, int size)
        {
            Wait(size);
            XboxName.Client.Receive(data, offset, size, SocketFlags.None);
        }
        #endregion
        #region Bool {Get; Set;}
        public bool SetBool(uint Address) { return GetMemory(Address, 1)[0] != 0; }

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
        public string GetString(uint Address, uint size) { return Encoding.UTF8.GetString(GetMemory(Address, size)); }

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
        public byte GetByte(uint Address) { return GetMemory(Address, 1)[0]; }

        public void SetByte(uint Address, byte Value) { SetMemory(Address, new byte[] { Value }); }

        public void SetByte(uint Address, byte[] Value) { SetMemory(Address, Value); }
        #endregion

        #region SByte {Get; Set;}
        public sbyte GetSByte(uint Address) { return (sbyte)GetMemory(Address, 1)[0]; }

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


        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        #endregion

        #endregion
    }
}