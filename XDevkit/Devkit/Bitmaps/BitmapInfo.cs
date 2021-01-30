namespace XDevkit
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfo
    {
        public List<BitmapSubmap> bitmapList;
        public List<int> rawIdTable1;
        public List<int> rawIdTable2;
        //public BitmapInfo(int TagIndex)
        //{
        //    int num3;
        //    IO.SeekTo((int) (map.Index_Items[TagIndex].Offset + 0x7c));
        //    int num = map.IO.In.ReadInt32();
        //    int offset = map.IO.In.ReadInt32() - map.Map_Header.mapMagic;
        //    bitmapList = new List<BitmapSubmap>();
        //    map.IO.SeekTo(offset);
        //    for (num3 = 0; num3 < num; num3++)
        //    {
        //        bitmapList.Add(new BitmapSubmap(map.IO.In));
        //    }
        //    map.IO.SeekTo((int) (map.Index_Items[TagIndex].Offset + 0xa8));
        //    num = map.IO.In.ReadInt32();
        //    offset = map.IO.In.ReadInt32() - map.Map_Header.mapMagic;
        //    rawIdTable1 = new List<int>();
        //    map.IO.SeekTo(offset);
        //    for (num3 = 0; num3 < num; num3++)
        //    {
        //        rawIdTable1.Add(map.IO.In.ReadInt32());
        //        map.IO.In.ReadInt32();
        //    }
        //    map.IO.SeekTo((int) (map.Index_Items[TagIndex].Offset + 180));
        //    num = map.IO.In.ReadInt32();
        //    offset = map.IO.In.ReadInt32() - map.Map_Header.mapMagic;
        //    rawIdTable2 = new List<int>();
        //    map.IO.SeekTo(offset);
        //    for (num3 = 0; num3 < num; num3++)
        //    {
        //        rawIdTable2.Add(map.IO.In.ReadInt32());
        //        map.IO.In.ReadInt32();
        //    }
        //}

        public byte[] GetBitmapData(int BitmapDataIndex)
        {
            int rawID = -1;
            int dataLength = 0;
            int sourceIndex = 0;
            if (rawIdTable1.Count != 0)
            {
                rawID = rawIdTable1[0];
                dataLength = bitmapList[BitmapDataIndex].RawLength;
            }
            else if (rawIdTable2.Count != 0)
            {
                rawID = rawIdTable2[bitmapList[BitmapDataIndex].Index1];
                for (int i = 0; i < bitmapList.Count; i++)
                {
                    if (bitmapList[i].Index1 == bitmapList[BitmapDataIndex].Index1)
                    {
                        dataLength += bitmapList[i].RawLength;
                        if (i == BitmapDataIndex)
                        {
                            break;
                        }
                        sourceIndex += bitmapList[i].RawLength;
                    }
                }
            }
            if (rawID == -1)
            {
                return new byte[0];
            }
            byte[] rawDataFromID = null;//RawInformation.GetRawDataFromID(rawID, dataLength);
            if (sourceIndex != 0)
            {
                byte[] destinationArray = new byte[bitmapList[BitmapDataIndex].RawLength];
                Array.Copy(rawDataFromID, sourceIndex, destinationArray, 0, destinationArray.Length);
                rawDataFromID = destinationArray;
            }
            return rawDataFromID;
        }

        public Bitmap GeneratePreview(int BitmapIndex)
        {
            BitmapSubmap submap = bitmapList[BitmapIndex];
            int vHeight = submap.vHeight;
            int vWidth = submap.vWidth;
            byte[] bitmapData = GetBitmapData(BitmapIndex);
            if (submap.IsDefined)
            {
                bitmapData = DXTDecoder.ConvertToLinearTexture(bitmapData, vWidth, vHeight, submap.Format);
            }
            switch (submap.Format)
            {
                case TextureFormat.A8:
                    bitmapData = DXTDecoder.DecodeA8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.Y8:
                    bitmapData = DXTDecoder.DecodeY8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.AY8:
                    bitmapData = DXTDecoder.DecodeAY8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.A8Y8:
                    bitmapData = DXTDecoder.DecodeA8Y8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.R5G6B5:
                    bitmapData = DXTDecoder.DecodeR5G6B5(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.A1R5G5B5:
                    bitmapData = DXTDecoder.DecodeA1R5G5B5(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.A4R4G4B4:
                    bitmapData = DXTDecoder.DecodeA4R4G4B4(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.X8R8G8B8:
                case TextureFormat.A8R8G8B8:
                    break;

                case TextureFormat.DXT1:
                    bitmapData = DXTDecoder.DecodeDXT1(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.DXT3:
                    bitmapData = DXTDecoder.DecodeDXT3(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.DXT5:
                    bitmapData = DXTDecoder.DecodeDXT5(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.DXN:
                    bitmapData = DXTDecoder.DecodeDXN(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.CTX1:
                    bitmapData = DXTDecoder.DecodeCTX1(bitmapData, vWidth, vHeight);
                    break;

                default:
                    return null;
            }
            Bitmap bitmap = new Bitmap(submap.Width, submap.Height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, submap.Width, submap.Height);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] destinationArray = new byte[(submap.Width * submap.Height) * 4];
            for (int i = 0; i < submap.Height; i++)
            {
                Array.Copy(bitmapData, (int) ((i * vWidth) * 4), destinationArray, (int) ((i * submap.Width) * 4), (int) (submap.Width * 4));
            }
            Marshal.Copy(destinationArray, 0, bitmapdata.Scan0, destinationArray.Length);
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public Bitmap GeneratePreview(int BitmapIndex, TextureFormat textureFormat)
        {
            BitmapSubmap submap = bitmapList[BitmapIndex];
            submap.Format = textureFormat;
            if (!submap.IsSupported)
            {
                Bitmap image = new Bitmap(200, 200);
                Graphics graphics = Graphics.FromImage(image);
                graphics.Clear(Color.Gray);
                string text = "No Preview Available";
                SizeF ef = graphics.MeasureString(text, new Font(FontFamily.GenericSerif, 15f));
                graphics.DrawString(text, new Font(FontFamily.GenericSerif, 15f), Brushes.Black, new PointF(100f - (ef.Width / 2f), 100f - (ef.Height / 2f)));
                return image;
            }
            int vHeight = submap.vHeight;
            int vWidth = submap.vWidth;
            byte[] bitmapData = GetBitmapData(BitmapIndex);
            if (submap.IsDefined)
            {
                bitmapData = DXTDecoder.ConvertToLinearTexture(bitmapData, vWidth, vHeight, submap.Format);
            }
            switch (textureFormat)
            {
                case TextureFormat.A8:
                    bitmapData = DXTDecoder.DecodeA8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.Y8:
                    bitmapData = DXTDecoder.DecodeY8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.AY8:
                    bitmapData = DXTDecoder.DecodeAY8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.A8Y8:
                    bitmapData = DXTDecoder.DecodeA8Y8(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.R5G6B5:
                    bitmapData = DXTDecoder.DecodeR5G6B5(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.A1R5G5B5:
                    bitmapData = DXTDecoder.DecodeA1R5G5B5(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.A4R4G4B4:
                    bitmapData = DXTDecoder.DecodeA4R4G4B4(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.X8R8G8B8:
                case TextureFormat.A8R8G8B8:
                    break;

                case TextureFormat.DXT1:
                    bitmapData = DXTDecoder.DecodeDXT1(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.DXT3:
                    bitmapData = DXTDecoder.DecodeDXT3(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.DXT5:
                    bitmapData = DXTDecoder.DecodeDXT5(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.DXN:
                    bitmapData = DXTDecoder.DecodeDXN(bitmapData, vWidth, vHeight);
                    break;

                case TextureFormat.CTX1:
                    bitmapData = DXTDecoder.DecodeCTX1(bitmapData, vWidth, vHeight);
                    break;

                default:
                    return null;
            }
            Bitmap bitmap2 = new Bitmap(submap.Width, submap.Height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, submap.Width, submap.Height);
            BitmapData bitmapdata = bitmap2.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] destinationArray = new byte[(submap.Width * submap.Height) * 4];
            for (int i = 0; i < submap.Height; i++)
            {
                Array.Copy(bitmapData, (int) ((i * vWidth) * 4), destinationArray, (int) ((i * submap.Width) * 4), (int) (submap.Width * 4));
            }
            Marshal.Copy(destinationArray, 0, bitmapdata.Scan0, destinationArray.Length);
            bitmap2.UnlockBits(bitmapdata);
            return bitmap2;
        }

        public void ExtractRaw(string FilePath, int BitmapIndex)
        {
            FileStream output = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(GetBitmapData(BitmapIndex));
            writer.Close();
        }

        public void Extract(string FilePath, int BitmapIndex)
        {
            FileStream output = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(output);
            BitmapSubmap bitmapInfo = bitmapList[BitmapIndex];
            new DDS(bitmapInfo).Write(bw);
            int vHeight = bitmapInfo.vHeight;
            int vWidth = bitmapInfo.vWidth;
            byte[] array = DXTDecoder.ConvertToLinearTexture(GetBitmapData(BitmapIndex), vWidth, vHeight, bitmapInfo.Format);
            if (bitmapInfo.Format != TextureFormat.A8R8G8B8)
            {
                for (int i = 0; i < array.Length; i += 2)
                {
                    Array.Reverse(array, i, 2);
                }
            }
            bw.Write(array);
            bw.Close();
        }
    }
}

