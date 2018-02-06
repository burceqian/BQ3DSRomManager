using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CIAHeader
    {
        public uint HeaderSize;
        public ushort Type;
        public ushort Version;
        public uint CertSize;
        public uint TicketSize;
        public uint TMDSize;
        public uint MetaSize;
        public long ContentSize;
    }
}
