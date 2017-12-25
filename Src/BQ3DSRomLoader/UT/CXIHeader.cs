using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSRomLoader.UT
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CXIHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] Signature;
        public uint Magic;
        public uint ContentSize;
        public ulong TitleId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] MakerCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Version;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved0;
        public ulong ProgramId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] Reserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] LogoSha256Hash;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] ProductCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] ExtendedHeaderSha256Hash;
        [MarshalAs(UnmanagedType.U4)]
        public uint ExtendedHeaderSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved2;
        public NCCHFlags Flags;
        public uint PlainRegionOffset;
        public uint PlainRegionSize;
        public uint LogoRegionOffset;
        public uint LogoRegionSize;
        public uint ExeFsOffset;
        public uint ExeFsSize;
        public uint ExeFsHashSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved4;
        public uint RomFsOffset;
        public uint RomFsSize;
        public uint RomFsHashSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] ExeFsSha256Hash;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] RomFsSha256Hash;
        public bool IsValid
        {
            get
            {
                return (this.Magic == 0x4843434e);
            }
        }
    }
}
