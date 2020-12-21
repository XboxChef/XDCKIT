namespace XDevkit
{
    using System;

    public class ActiveXMessageBoxes
    {
        public uint Size;
        public byte[] XOverlappedBytes;

        public ActiveXMessageBoxes(uint size, byte[] xOverlappedBytes)
        {
            Size = size;
            XOverlappedBytes = xOverlappedBytes;
        }
    }
}

