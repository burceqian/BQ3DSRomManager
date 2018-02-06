using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    internal class CXI
    {
        private static NCCHPartitionType GetNCCHPartitionType(byte ncchType)
        {
            byte num = NCCHContentType.ExeFS.ByteValue();
            byte num2 = NCCHContentType.RomFS.ByteValue();
            byte num3 = NCCHContentType.Manual.ByteValue();
            byte num4 = NCCHContentType.SystemUpdate.ByteValue();
            byte num5 = NCCHContentType.Child.ByteValue();
            if (((ncchType & num) != num) && ((ncchType & num2) == num2))
            {
                if (((ncchType & num3) == num3) && ((ncchType & num4) != num4))
                {
                    return NCCHPartitionType.CFAManual;
                }
                if ((ncchType & num5) == num5)
                {
                    return NCCHPartitionType.CFADLPChild;
                }
                if (((ncchType & num4) == num4) && ((ncchType & num3) != num3))
                {
                    return NCCHPartitionType.CFAUpdate;
                }
                return NCCHPartitionType.CFASimple;
            }
            if ((ncchType & num) == num)
            {
                return NCCHPartitionType.CXI;
            }
            return NCCHPartitionType.Unknown;
        }

        public bool Read(Stream fs)
        {
            this.Header = fs.ReadStruct<CXIHeader>();
            return this.Header.IsValid;
        }

        public CXIHeader Header { get; private set; }

        public string MakerCode
        {
            get
            {
                return this.Header.MakerCode.ToASCIIString();
            }
        }

        public NCCHPartitionType PartitionType
        {
            get
            {
                return GetNCCHPartitionType(this.Header.Flags.Type);
            }
        }

        public string ProductCode
        {
            get
            {
                return this.Header.ProductCode.ToASCIIString();
            }
        }

        public string TitleId
        {
            get
            {
                return this.Header.TitleId.ToString("X16");
            }
        }
    }
}
