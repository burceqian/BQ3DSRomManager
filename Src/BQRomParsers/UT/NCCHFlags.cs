using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NCCHFlags
    {
        public byte Unknown0;
        public byte Unknown1;
        public byte Unknown2;
        public byte CryptoMethod;
        public byte Platform;
        public byte Type;
        public byte UnitSize;
        public byte BitMasks;
    }
}
