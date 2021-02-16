//Do Not Delete This Comment... 
//Made By TeddyHammer on 08/20/16
//Any Code Copied Must Source This Project (its the law (:P)) Please.. i work hard on it 3 years and counting...
//Thank You for looking love you guys...

namespace XDevkit
{
    using System.IO;

    public class EndianIO
    {
        private readonly bool isfile;
        private bool isOpen;
        private System.IO.Stream stream;
        private readonly string filepath;
        private readonly EndianType endiantype;
        private EndianReader _in;
        private EndianWriter _out;

        public EndianIO(System.IO.Stream Stream, EndianType EndianStyle)
        {
            filepath = "";
            endiantype = EndianType.LittleEndian;
            endiantype = EndianStyle;
            stream = Stream;
            isfile = false;
        }

        public EndianIO(string FilePath, EndianType EndianStyle)
        {
            filepath = "";
            endiantype = EndianType.LittleEndian;
            endiantype = EndianStyle;
            filepath = FilePath;
            isfile = true;
        }

        public EndianIO(byte[] Buffer, EndianType EndianStyle)
        {
            filepath = "";
            endiantype = EndianType.LittleEndian;
            endiantype = EndianStyle;
            stream = new MemoryStream(Buffer);
            isfile = false;
        }

        public void Close()
        {
            if (isOpen)
            {
                stream.Close();
                _in.Close();
                _out.Close();
                isOpen = false;
            }
        }

        public void Open()
        {
            if (!isOpen)
            {
                if (isfile)
                {
                    stream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
                _in = new EndianReader(stream, endiantype);
                _out = new EndianWriter(stream, endiantype);
                isOpen = true;
            }
        }

        public void SeekTo(int offset)
        {
            SeekTo(offset, SeekOrigin.Begin);
        }

        public void SeekTo(uint offset)
        {
            SeekTo((int)offset, SeekOrigin.Begin);
        }

        public void SeekTo(int offset, SeekOrigin SeekOrigin)
        {
            stream.Seek(offset, SeekOrigin);
        }

        public bool Opened =>
            isOpen;

        public bool Closed =>
            !isOpen;

        public EndianReader In =>
            _in;

        public EndianWriter Out =>
            _out;

        public System.IO.Stream Stream =>
            stream;
    }
}

