namespace XDevkit
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct BitmapSubmap
    {
        public int Width;
        public int Height;
        public int Depth;
        public TextureType Type;
        public TextureFormat Format;
        public int RawLength;
        public byte Index1;
        public byte Index2;
        public int vWidth =>
            GetVirtualDimension(Width);
        public int vHeight =>
            GetVirtualDimension(Height);
        public BitmapSubmap(EndianReader er)
        {
            Width = er.ReadInt16();
            Height = er.ReadInt16();
            Depth = er.ReadInt16();
            Type = (TextureType)er.ReadInt16();
            Format = (TextureFormat)er.ReadInt16();
            er.ReadInt16();
            er.ReadInt16();
            er.ReadInt16();
            er.ReadByte();
            er.ReadByte();
            Index1 = er.ReadByte();
            Index2 = er.ReadByte();
            er.ReadInt32();
            er.ReadInt32();
            er.ReadInt32();
            RawLength = er.ReadInt32();
            er.ReadInt32();
            er.ReadInt32();
        }

        public bool IsDefined =>
            Enum.IsDefined(typeof(TextureFormat), Format);
        public bool IsSupported =>
            (((((Format == TextureFormat.DXT1) || (Format == TextureFormat.DXT3)) || ((Format == TextureFormat.DXT5) || (Format == TextureFormat.CTX1))) || (Format == TextureFormat.DXN)) || (Format == TextureFormat.A8R8G8B8));
        public int GetVirtualDimension(int Size)
        {
            if ((Size % 0x80) != 0)
            {
                Size += 0x80 - (Size % 0x80);
            }
            return Size;
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum TextureType : short
        {
            CubeMap = 2,
            DXT5 = 0x10,
            Sprite = 3,
            Texture2D = 0,
            Texture3D = 1,
            UIBitmap = 4
        }
    }
}

