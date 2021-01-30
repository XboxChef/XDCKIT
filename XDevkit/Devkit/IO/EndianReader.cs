//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...

namespace XDevkit
{
    using System;
    using System.IO;

    public class EndianReader : BinaryReader
    {
        public EndianType endianstyle;

        public EndianReader(Stream stream, EndianType endianstyle) : base(stream)
        {
            endianstyle = endianstyle;
        }

        public string ReadAsciiString(int Length) =>
            ReadAsciiString(Length, endianstyle);

        public string ReadAsciiString(int Length, EndianType EndianType)
        {
            string str = "";
            int num = 0;
            int num2 = 0;
            while (true)
            {
                if (num2 < Length)
                {
                    char ch = (char)ReadByte();
                    num++;
                    if (ch != '\0')
                    {
                        str = str + ch;
                        num2++;
                        continue;
                    }
                }
                int num3 = Length - num;
                BaseStream.Seek(num3, SeekOrigin.Current);
                return str;
            }
        }

        public override double ReadDouble() =>
            ReadDouble(endianstyle);

        public double ReadDouble(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(8);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToDouble(array, 0);
        }

        public override short ReadInt16() =>
            ReadInt16(endianstyle);

        public short ReadInt16(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(2);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt16(array, 0);
        }

        public int ReadInt24() =>
            ReadInt24(endianstyle);

        public int ReadInt24(EndianType EndianType)
        {
            byte[] destinationArray = new byte[4];
            Array.Copy(base.ReadBytes(3), 0, destinationArray, 0, 3);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(destinationArray);
            }
            return BitConverter.ToInt32(destinationArray, 0);
        }

        public override int ReadInt32() =>
            ReadInt32(endianstyle);

        public int ReadInt32(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(4);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt32(array, 0);
        }

        public override long ReadInt64() =>
            ReadInt64(endianstyle);

        public long ReadInt64(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(8);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt64(array, 0);
        }

        public string ReadNullTerminatedString()
        {
            string str = "";
            while (true)
            {
                char ch;
                bool flag = (ch = ReadChar()) != '\0';
                if (!flag || (ch == '\0'))
                {
                    return str;
                }
                str = str + ch;
            }
        }

        public override float ReadSingle() =>
            ReadSingle(endianstyle);

        public float ReadSingle(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(4);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToSingle(array, 0);
        }

        public override string ReadString()
        {
            string str = "";
            int num = 0;
            while (true)
            {
                char ch = (char)ReadByte();
                num++;
                if (ch == '\0')
                {
                    int num2 = str.Length - num;
                    BaseStream.Seek(num2 + 1, SeekOrigin.Current);
                    return str;
                }
                str = str + ch;
            }
        }

        public string ReadString(int Length) =>
            ReadAsciiString(Length);

        public override ushort ReadUInt16() =>
            ReadUInt16(endianstyle);

        public ushort ReadUInt16(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(2);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToUInt16(array, 0);
        }

        public override uint ReadUInt32() =>
            ReadUInt32(endianstyle);

        public uint ReadUInt32(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(4);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToUInt32(array, 0);
        }

        public override ulong ReadUInt64() =>
            ReadUInt64(endianstyle);

        public ulong ReadUInt64(EndianType EndianType)
        {
            byte[] array = base.ReadBytes(8);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToUInt64(array, 0);
        }

        public string ReadUnicodeString(int Length) =>
            ReadUnicodeString(Length, endianstyle);

        public string ReadUnicodeString(int Length, EndianType EndianType)
        {
            string str = "";
            int num = 0;
            int num2 = 0;
            while (true)
            {
                if (num2 < Length)
                {
                    char ch = (char)ReadUInt16(EndianType);
                    num++;
                    if (ch != '\0')
                    {
                        str = str + ch;
                        num2++;
                        continue;
                    }
                }
                int num3 = (Length - num) * 2;
                BaseStream.Seek(num3, SeekOrigin.Current);
                return str;
            }
        }

        public void SeekTo(int offset)
        {
            SeekTo(offset, SeekOrigin.Begin);
        }

        public void SeekTo(long offset)
        {
            SeekTo((int)offset, SeekOrigin.Begin);
        }

        public void SeekTo(uint offset)
        {
            SeekTo((int)offset, SeekOrigin.Begin);
        }

        public void SeekTo(int offset, SeekOrigin SeekOrigin)
        {
            BaseStream.Seek(offset, SeekOrigin);
        }
    }
}

