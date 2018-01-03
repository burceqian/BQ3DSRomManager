using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSRomLoader.UT
{
    public static class ImageUtil
    {
        private static readonly int[] Convert5To8 = new int[] {
            0, 8, 0x10, 0x18, 0x20, 0x29, 0x31, 0x39, 0x41, 0x4a, 0x52, 90, 0x62, 0x6a, 0x73, 0x7b,
            0x83, 0x8b, 0x94, 0x9c, 0xa4, 0xac, 180, 0xbd, 0xc5, 0xcd, 0xd5, 0xde, 230, 0xee, 0xf6, 0xff
         };
        private static readonly byte[] TempBytes = new byte[4];

        private static Color DecodeColor(int val, PixelFormat pixelFormat)
        {
            int num2;
            int num3;
            int num4;
            int alpha = 0xff;
            switch (pixelFormat)
            {
                case PixelFormat.RGBA8:
                    num2 = (val >> 0x18) & 0xff;
                    num3 = (val >> 0x10) & 0xff;
                    num4 = (val >> 8) & 0xff;
                    alpha = val & 0xff;
                    return Color.FromArgb(alpha, num2, num3, num4);

                case PixelFormat.RGB8:
                    num2 = (val >> 0x10) & 0xff;
                    num3 = (val >> 8) & 0xff;
                    num4 = val & 0xff;
                    return Color.FromArgb(alpha, num2, num3, num4);

                case PixelFormat.RGBA5551:
                    num2 = Convert5To8[(val >> 11) & 0x1f];
                    num3 = Convert5To8[(val >> 6) & 0x1f];
                    num4 = Convert5To8[(val >> 1) & 0x1f];
                    alpha = ((val & 1) == 1) ? 0xff : 0;
                    return Color.FromArgb(alpha, num2, num3, num4);

                case PixelFormat.RGB565:
                    num2 = Convert5To8[(val >> 11) & 0x1f];
                    num3 = ((val >> 5) & 0x3f) * 4;
                    num4 = Convert5To8[val & 0x1f];
                    return Color.FromArgb(alpha, num2, num3, num4);

                case PixelFormat.RGBA4:
                    alpha = 0x11 * (val & 15);
                    num2 = 0x11 * ((val >> 12) & 15);
                    num3 = 0x11 * ((val >> 8) & 15);
                    num4 = 0x11 * ((val >> 4) & 15);
                    return Color.FromArgb(alpha, num2, num3, num4);

                case PixelFormat.LA8:
                    num2 = val >> 8;
                    alpha = val & 0xff;
                    return Color.FromArgb(alpha, num2, num2, num2);

                case PixelFormat.HILO8:
                    num2 = val >> 8;
                    return Color.FromArgb(alpha, num2, num2, num2);

                case PixelFormat.L8:
                    return Color.FromArgb(alpha, val, val, val);

                case PixelFormat.A8:
                    return Color.FromArgb(val, alpha, alpha, alpha);

                case PixelFormat.LA4:
                    num2 = val >> 4;
                    alpha = val & 15;
                    return Color.FromArgb(alpha, num2, num2, num2);
            }
            return Color.White;
        }

        private static void DecodeTile(int iconSize, int tileSize, int ax, int ay, Bitmap bmp, Stream fs, PixelFormat pixelFormat)
        {
            if (tileSize == 0)
            {
                fs.Read(TempBytes, 0, 2);
                bmp.SetPixel(ax, ay, DecodeColor((TempBytes[1] << 8) + TempBytes[0], pixelFormat));
            }
            else
            {
                for (int i = 0; i < iconSize; i += tileSize)
                {
                    for (int j = 0; j < iconSize; j += tileSize)
                    {
                        DecodeTile(tileSize, tileSize / 2, j + ax, i + ay, bmp, fs, pixelFormat);
                    }
                }
            }
        }

        private static void EncodeColor(Color color, PixelFormat pixelFormat, byte[] bytes)
        {
            switch (pixelFormat)
            {
                case PixelFormat.RGBA8:
                    bytes[0] = color.A;
                    bytes[1] = color.B;
                    bytes[2] = color.G;
                    bytes[3] = color.R;
                    return;

                case PixelFormat.RGB8:
                    bytes[0] = color.B;
                    bytes[1] = color.G;
                    bytes[2] = color.R;
                    return;

                case PixelFormat.RGBA5551:
                    bytes[1] = (byte)((color.G & 0xe0) >> 5);
                    bytes[1] = (byte)(bytes[1] + ((byte)(color.R & 0xf8)));
                    bytes[0] = (byte)((color.B & 0xf8) >> 2);
                    bytes[0] = (color.A > 0x80) ? ((byte)1) : ((byte)0);
                    bytes[0] = (byte)(bytes[0] + ((byte)((color.G & 0x18) << 3)));
                    return;

                case PixelFormat.RGB565:
                    bytes[1] = (byte)((color.G & 0xe0) >> 5);
                    bytes[1] = (byte)(bytes[1] + ((byte)(color.R & 0xf8)));
                    bytes[0] = (byte)(color.B >> 3);
                    bytes[0] = (byte)(bytes[0] + ((byte)((color.G & 0x1c) << 3)));
                    return;

                case PixelFormat.RGBA4:
                    bytes[1] = (byte)((color.G & 240) >> 4);
                    bytes[1] = (byte)(bytes[1] + ((byte)(color.R & 240)));
                    bytes[0] = (byte)((color.A & 240) >> 4);
                    bytes[0] = (byte)(bytes[0] + ((byte)(color.B & 240)));
                    return;
            }
            bytes[0] = 0;
            bytes[1] = 0;
        }

        private static void EncodeTile(int iconSize, int tileSize, int ax, int ay, Bitmap bmp, Stream fs, PixelFormat pixelFormat)
        {
            if (tileSize == 0)
            {
                EncodeColor(bmp.GetPixel(ax, ay), pixelFormat, TempBytes);
                fs.Write(TempBytes, 0, PixelFormatBytes(pixelFormat));
            }
            else
            {
                for (int i = 0; i < iconSize; i += tileSize)
                {
                    for (int j = 0; j < iconSize; j += tileSize)
                    {
                        EncodeTile(tileSize, tileSize / 2, j + ax, i + ay, bmp, fs, pixelFormat);
                    }
                }
            }
        }

        private static byte GetLuminance(byte red, byte green, byte blue)
        {
            return (byte)(((((0x4cb2 * red) + (0x9691 * green)) + (0x1d3e * blue)) >> 0x10) & 0xff);
        }

        private static int PixelFormatBytes(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.RGBA8:
                    return 4;

                case PixelFormat.RGB8:
                    return 3;

                case PixelFormat.RGBA5551:
                case PixelFormat.RGB565:
                case PixelFormat.LA8:
                case PixelFormat.LA4:
                case PixelFormat.ETC1:
                case PixelFormat.ETC1A4:
                    return 2;

                case PixelFormat.HILO8:
                case PixelFormat.L8:
                case PixelFormat.A8:
                case PixelFormat.L4:
                    return 1;
            }
            return 0;
        }

        public static Bitmap ReadImageFromStream(Stream fs, int width, int height, PixelFormat pixelFormat)
        {
            Bitmap bmp = new Bitmap(width, height);
            for (int i = 0; i < height; i += 8)
            {
                for (int j = 0; j < width; j += 8)
                {
                    DecodeTile(8, 8, j, i, bmp, fs, pixelFormat);
                }
            }
            return bmp;
        }

        public static void WriteImageToStream(Image source, Stream fs, PixelFormat pixelFormat)
        {
            Bitmap bmp = new Bitmap(source);
            for (int i = 0; i < bmp.Height; i += 8)
            {
                for (int j = 0; j < bmp.Width; j += 8)
                {
                    EncodeTile(8, 8, 0, 0, bmp, fs, pixelFormat);
                }
            }
        }

        public enum PixelFormat
        {
            RGBA8,
            RGB8,
            RGBA5551,
            RGB565,
            RGBA4,
            LA8,
            HILO8,
            L8,
            A8,
            LA4,
            L4,
            ETC1,
            ETC1A4
        }
    }
}
