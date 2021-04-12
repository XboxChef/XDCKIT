namespace XDevkit
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct DDS
    {
        public int dwMagic;
        public int dwSize;
        public DDSD dwFlags;
        public int dwHeight;
        public int dwWidth;
        public int dwPitchOrLinearSize;
        public int dwDepth;
        public int dwMipMapCount;
        public int[] dwReserved1;
        public int dwSize2;
        public DDPF dwFlags2;
        public FourCC dwFourCC;
        public int dwRGBBitCount;
        public int dwRBitMask;
        public int dwGBitMask;
        public int dwBBitMask;
        public uint dwRGBAlphaBitMask;
        public DDSCAPS dwCaps1;
        public DDSCAPS2 dwCaps2;
        public int[] Reserved2;
        public int dwReserved3;
        public DDS(BitmapSubmap BitmapInfo)
        {
            dwMagic = 0x20534444;
            dwSize = 0x7c;
            dwFlags = 0;
            dwFlags |= DDSD.CAPS;
            dwFlags |= DDSD.HEIGHT;
            dwFlags |= DDSD.WIDTH;
            dwFlags |= DDSD.PIXELFORMAT;
            dwHeight = BitmapInfo.vHeight;
            dwWidth = BitmapInfo.vWidth;
            if (BitmapInfo.Format == TextureFormat.A8R8G8B8)
            {
                dwFlags |= DDSD.PITCH;
                dwPitchOrLinearSize = dwWidth * 4;
            }
            else
            {
                dwFlags |= DDSD.LINEARSIZE;
                dwPitchOrLinearSize = BitmapInfo.RawLength;
            }
            dwDepth = 0;
            dwMipMapCount = 0;
            dwReserved1 = new int[11];
            dwSize2 = 0x20;
            dwFlags2 = 0;
            dwFourCC = FourCC.None;
            switch (BitmapInfo.Format)
            {
                case TextureFormat.A8R8G8B8:
                    dwFlags2 |= DDPF.RGB;
                    dwFlags2 |= DDPF.ALPHAPIXELS;
                    break;

                case TextureFormat.DXT1:
                    dwFlags2 |= DDPF.FOURCC;
                    dwFourCC = FourCC.DXT1;
                    break;

                case TextureFormat.DXT3:
                    dwFlags2 |= DDPF.FOURCC;
                    dwFourCC = FourCC.DXT3;
                    break;

                case TextureFormat.DXT5:
                    dwFlags2 |= DDPF.FOURCC;
                    dwFourCC = FourCC.DXT5;
                    break;
            }
            if (BitmapInfo.Format == TextureFormat.A8R8G8B8)
            {
                dwRGBBitCount = 0x20;
                dwRBitMask = 0xff0000;
                dwGBitMask = 0xff00;
                dwBBitMask = 0xff;
                dwRGBAlphaBitMask = 0xff000000;
            }
            else
            {
                dwRGBBitCount = 0;
                dwRBitMask = 0;
                dwGBitMask = 0;
                dwBBitMask = 0;
                dwRGBAlphaBitMask = 0;
            }
            dwCaps1 = 0;
            dwCaps1 |= DDSCAPS.TEXTURE;
            dwCaps2 = 0;
            Reserved2 = new int[2];
            dwReserved3 = 0;
        }

        public void Read(BinaryReader br)
        {
            int num;
            dwMagic = br.ReadInt32();
            dwSize = br.ReadInt32();
            dwFlags = (DDSD)br.ReadInt32();
            dwHeight = br.ReadInt32();
            dwWidth = br.ReadInt32();
            dwPitchOrLinearSize = br.ReadInt32();
            dwDepth = br.ReadInt32();
            dwMipMapCount = br.ReadInt32();
            dwReserved1 = new int[11];
            for (num = 0; num < dwReserved1.Length; num++)
            {
                dwReserved1[num] = br.ReadInt32();
            }
            dwSize2 = br.ReadInt32();
            dwFlags2 = (DDPF)br.ReadInt32();
            dwFourCC = (FourCC)br.ReadInt32();
            dwRGBBitCount = br.ReadInt32();
            dwRBitMask = br.ReadInt32();
            dwGBitMask = br.ReadInt32();
            dwBBitMask = br.ReadInt32();
            dwRGBAlphaBitMask = br.ReadUInt32();
            dwCaps1 = (DDSCAPS)br.ReadInt32();
            dwCaps2 = (DDSCAPS2)br.ReadInt32();
            Reserved2 = new int[2];
            for (num = 0; num < Reserved2.Length; num++)
            {
                Reserved2[num] = br.ReadInt32();
            }
            dwReserved3 = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            int num;
            bw.Write(dwMagic);
            bw.Write(dwSize);
            bw.Write((int)dwFlags);
            bw.Write(dwHeight);
            bw.Write(dwWidth);
            bw.Write(dwPitchOrLinearSize);
            bw.Write(dwDepth);
            bw.Write(dwMipMapCount);
            for (num = 0; num < dwReserved1.Length; num++)
            {
                bw.Write(dwReserved1[num]);
            }
            bw.Write(dwSize2);
            bw.Write((int)dwFlags2);
            bw.Write((int)dwFourCC);
            bw.Write(dwRGBBitCount);
            bw.Write(dwRBitMask);
            bw.Write(dwGBitMask);
            bw.Write(dwBBitMask);
            bw.Write(dwRGBAlphaBitMask);
            bw.Write((int)dwCaps1);
            bw.Write((int)dwCaps2);
            for (num = 0; num < Reserved2.Length; num++)
            {
                bw.Write(Reserved2[num]);
            }
            bw.Write(dwReserved3);
        }
        [Flags]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum DDPF
        {
            ALPHAPIXELS = 1,
            FOURCC = 4,
            RGB = 0x40
        }

        [Flags]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum DDSCAPS
        {
            COMPLEX = 8,
            MIPMAP = 0x400000,
            TEXTURE = 0x1000
        }

        [Flags]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum DDSCAPS2
        {
            CUBEMAP = 0x200,
            CUBEMAP_NEGATIVEX = 0x800,
            CUBEMAP_NEGATIVEY = 0x2000,
            CUBEMAP_NEGATIVEZ = 0x8000,
            CUBEMAP_POSITIVEX = 0x400,
            CUBEMAP_POSITIVEY = 0x1000,
            CUBEMAP_POSITIVEZ = 0x4000,
            VOLUME = 0x200000
        }

        [Flags]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum DDSD
        {
            CAPS = 1,
            DEPTH = 0x800000,
            HEIGHT = 2,
            LINEARSIZE = 0x80000,
            MIPMAPCOUNT = 0x20000,
            PITCH = 8,
            PIXELFORMAT = 0x1000,
            WIDTH = 4
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public enum FourCC
        {
            DXT1 = 0x31545844,
            DXT3 = 0x33545844,
            DXT5 = 0x35545844,
            None = 0
        }
    }
}

