//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it since 2015.
//Thank You for looking love you guys...

namespace XDevkit
{
    using System;
    using System.IO;

    public class EndianWriter : BinaryWriter
    {
        private readonly EndianType EndianStyle = EndianType.LittleEndian;

        public EndianWriter(Stream stream, EndianType endianstyle) : base(stream)
        {
            this.EndianStyle = endianstyle;
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

        public override void Write(double value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(short value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(int value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(long value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(float value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(ushort value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(uint value)
        {
            Write(value, EndianStyle);
        }

        public override void Write(ulong value)
        {
            Write(value, EndianStyle);
        }

        public void Write(double value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(short value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(int value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(long value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(float value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(ushort value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(uint value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void Write(ulong value, EndianType EndianType)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (EndianType == EndianType.BigEndian)
            {
                Array.Reverse(bytes);
            }
            base.Write(bytes);
        }

        public void WriteAsciiString(string String, int Length)
        {
            WriteAsciiString(String, Length, EndianStyle);
        }

        public void WriteAsciiString(string String, int Length, EndianType EndianType)
        {
            int length = String.Length;
            int num2 = 0;
            while (true)
            {
                bool flag = num2 < length;
                if (!flag || (num2 > Length))
                {
                    int num4 = Length - length;
                    if (num4 > 0)
                    {
                        Write(new byte[num4]);
                    }
                    return;
                }
                byte num3 = (byte)String[num2];
                Write(num3);
                num2++;
            }
        }

        public void WriteUnicodeString(string String, int Length)
        {
            WriteUnicodeString(String, Length, EndianStyle);
        }

        public void WriteUnicodeString(string String, int Length, EndianType EndianType)
        {
            int length = String.Length;
            int num2 = 0;
            while (true)
            {
                bool flag = num2 < length;
                if (!flag || (num2 > Length))
                {
                    int num4 = (Length - length) * 2;
                    if (num4 > 0)
                    {
                        Write(new byte[num4]);
                    }
                    return;
                }
                ushort num3 = String[num2];
                Write(num3, EndianType);
                num2++;
            }
        }
    }
}

