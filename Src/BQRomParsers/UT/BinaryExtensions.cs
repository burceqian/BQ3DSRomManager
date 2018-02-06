using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    public static class BinaryExtensions
    {
        public static string FormatedDiskHexString(this long src)
        {
            return string.Format("{0:X2} bytes", src);
        }

        public static byte[] HexStringToBytes(this string hexString)
        {
            try
            {
                string str = hexString.Replace(" ", "");
                if ((str.Length % 2) != 0)
                {
                    str = str + " ";
                }
                byte[] buffer = new byte[str.Length / 2];
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Convert.ToByte(str.Substring(i * 2, 2).Trim(), 0x10);
                }
                return buffer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Memset(this byte[] src, byte val = 0)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] = val;
            }
        }

        public static ushort ReverseBytes(this ushort value)
        {
            return (ushort)(((value & 0xff) << 8) | ((value & 0xff00) >> 8));
        }

        public static uint ReverseBytes(this uint value)
        {
            return (uint)(((((value & 0xff) << 0x18) | ((value & 0xff00) << 8)) | ((value & 0xff0000) >> 8)) | ((value & -16777216) >> 0x18));
        }

        //public static ulong ReverseBytes(this ulong value)
        //{
        //    return (((((((((value & 0xffL) << 0x38) | ((value & 0xff00L) << 40)) | ((value & 0xff0000L) << 0x18)) | ((value & 0xff000000L) << 8)) | ((value & 0xff00000000L) >> 8)) | ((value & 0xff0000000000L) >> 0x18)) | ((value & 0xff000000000000L) >> 40)) | ((value & -72057594037927936L) >> 0x38));
        //}

        public static string ToASCIIString(this byte[] src)
        {
            if (src == null)
            {
                return null;
            }
            string str = Encoding.ASCII.GetString(src);
            int index = str.IndexOf('\0');
            if (index != -1)
            {
                return str.Substring(0, index);
            }
            return str;
        }

        public static string ToHexString(this byte[] src, bool toUpper = false)
        {
            if (src == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            string format = toUpper ? "{0:X2}" : "{0:x2}";
            foreach (byte num in src)
            {
                builder.AppendFormat(format, num);
            }
            return builder.ToString();
        }

        public static string ToHexView(this byte[] src)
        {
            StringBuilder builder = new StringBuilder();
            string format = "{0:X2} ";
            for (int i = 0; i < src.Length; i++)
            {
                builder.AppendFormat(format, src[i]);
                if (((i + 1) % 0x10) == 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }

        public static string ToUTF16String(this byte[] src)
        {
            if (src == null)
            {
                return null;
            }
            string str = Encoding.Unicode.GetString(src);
            int index = str.IndexOf('\0');
            if (index != -1)
            {
                return str.Substring(0, index);
            }
            return str;
        }

        public static ushort U8ToU16BE(this byte[] src)
        {
            return (ushort)(src[1] | (src[0] << 8));
        }

        public static ushort U8ToU16LE(this byte[] src)
        {
            return (ushort)(src[0] | (src[1] << 8));
        }

        public static uint U8ToU32BE(this byte[] src)
        {
            return (uint)(((src[3] | (src[2] << 8)) | (src[1] << 0x10)) | (src[0] << 0x18));
        }

        public static uint U8ToU32LE(this byte[] src)
        {
            return (uint)(((src[0] | (src[1] << 8)) | (src[2] << 0x10)) | (src[3] << 0x18));
        }

        //public static ulong U8ToU64BE(this byte[,] src, int rowIndex)
        //{
        //    ulong num = 0L;
        //    num |= src[rowIndex, 7];
        //    num |= src[rowIndex, 6] << 8;
        //    num |= src[rowIndex, 5] << 0x10;
        //    num |= src[rowIndex, 4] << 0x18;
        //    num |= src[rowIndex, 3] << 0x20;
        //    num |= src[rowIndex, 2] << 40;
        //    num |= src[rowIndex, 1] << 0x30;
        //    return (num | (src[rowIndex, 0] << 0x38));
        //}

        //public static ulong U8ToU64LE(this byte[] src)
        //{
        //    ulong num = 0L;
        //    num |= src[0];
        //    num |= src[1] << 8;
        //    num |= src[2] << 0x10;
        //    num |= src[3] << 0x18;
        //    num |= src[4] << 0x20;
        //    num |= src[5] << 40;
        //    num |= src[6] << 0x30;
        //    return (num | (src[7] << 0x38));
        //}

        //public static ulong U8ToU64LE(this byte[,] src, int rowIndex)
        //{
        //    ulong num = 0L;
        //    num |= src[rowIndex, 0];
        //    num |= src[rowIndex, 1] << 8;
        //    num |= src[rowIndex, 2] << 0x10;
        //    num |= src[rowIndex, 3] << 0x18;
        //    num |= src[rowIndex, 4] << 0x20;
        //    num |= src[rowIndex, 5] << 40;
        //    num |= src[rowIndex, 6] << 0x30;
        //    return (num | (src[rowIndex, 7] << 0x38));
        //}
    }
}
