using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSRomLoader.UT
{
    internal class CIA
    {
        private static long Align64(long val)
        {
            return (((long)Math.Ceiling((double)(Convert.ToDouble(val) / 64.0))) * 0x40L);
        }

        private static uint Align64(uint val)
        {
            return (((uint)Math.Ceiling((double)(Convert.ToDouble(val) / 64.0))) * 0x40);
        }

        public bool Read(Stream fs)
        {
            this.Header = fs.ReadStruct<CIAHeader>();
            fs.Seek((long)this.ContentOffset, SeekOrigin.Begin);
            this.CXIContent = new CXI();
            this.CXIContent.Read(fs);
            return this.CXIContent.Header.IsValid;
        }

        public uint CertOffset
        {
            get
            {
                return this.HeaderSize;
            }
        }

        public uint CertSize
        {
            get
            {
                return Align64(this.Header.CertSize);
            }
        }

        public uint ContentOffset
        {
            get
            {
                return (this.TMDOffset + this.TMDSize);
            }
        }

        public long ContentSize
        {
            get
            {
                return Align64(this.Header.ContentSize);
            }
        }

        public CXI CXIContent { get; private set; }

        public CIAHeader Header { get; private set; }

        public uint HeaderSize
        {
            get
            {
                return Align64(this.Header.HeaderSize);
            }
        }

        public string MakerCode
        {
            get
            {
                return this.CXIContent.MakerCode;
            }
        }

        public long MetaOffset
        {
            get
            {
                return (this.ContentOffset + this.ContentSize);
            }
        }

        public uint MetaSize
        {
            get
            {
                return Align64(this.Header.MetaSize);
            }
        }

        public string ProductCode
        {
            get
            {
                return this.CXIContent.ProductCode;
            }
        }

        public uint TicketOffset
        {
            get
            {
                return (this.CertOffset + this.CertSize);
            }
        }

        public uint TicketSize
        {
            get
            {
                return Align64(this.Header.TicketSize);
            }
        }

        public string TitleId
        {
            get
            {
                return this.CXIContent.TitleId;
            }
        }

        public uint TMDOffset
        {
            get
            {
                return (this.TicketOffset + this.TicketSize);
            }
        }

        public uint TMDSize
        {
            get
            {
                return Align64(this.Header.TMDSize);
            }
        }
    }
}
