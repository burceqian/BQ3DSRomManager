using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSRomLoader.UT
{
    public static class StreamExtensions
    {
        public static T ReadStruct<T>(this Stream fs)
        {
            byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
            fs.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T local = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return local;
        }

        public static byte[] ToByteArray<T>(this T structure) where T : struct
        {
            int cb = Marshal.SizeOf(structure);
            byte[] destination = new byte[cb];
            IntPtr ptr = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr(structure, ptr, false);
            Marshal.Copy(ptr, destination, 0, cb);
            Marshal.FreeHGlobal(ptr);
            return destination;
        }

        public static void WriteDummyBytes(this FileStream fs, long offset, long len, byte dummyByte = 0xff)
        {
            long num = (len > 0x200L) ? 0x200L : len;
            byte[] buffer = new byte[num];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = dummyByte;
            }
            int tickCount = Environment.TickCount;
            fs.Seek(offset, SeekOrigin.Begin);
            long num4 = 0L;
            while (num4 < len)
            {
                long num5 = len - num4;
                long num6 = Math.Min(num5, num);
                fs.Write(buffer, 0, (int)num6);
                num4 += num6;
                if ((Environment.TickCount - tickCount) > 200)
                {
                    fs.Flush(true);
                    tickCount = Environment.TickCount;
                }
            }
        }
    }
}
