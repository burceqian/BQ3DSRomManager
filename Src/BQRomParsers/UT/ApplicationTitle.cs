using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    public class ApplicationTitle
    {
        public void Read(FileStream fs)
        {
            try
            {
                byte[] buffer = new byte[0x100];
                fs.Read(buffer, 0, 0x80);
                this.ShortDescription = buffer.ToUTF16String();
                fs.Read(buffer, 0, 0x100);
                this.LongDescription = buffer.ToUTF16String();
                fs.Read(buffer, 0, 0x80);
                this.Publisher = buffer.ToUTF16String();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string LongDescription { get; private set; }

        public string Publisher { get; private set; }

        public string ShortDescription { get; private set; }
    }
}
