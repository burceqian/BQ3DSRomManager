using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BQStructure
{
    public class RomInformation: INotifyPropertyChanged
    {
        public RomBasicInfo BasicInfo { get; set; }
        public RomExpandInfo ExpandInfo { get; set; }

        public RomInformation()
        {
            BasicInfo = new RomBasicInfo();
            ExpandInfo = new RomExpandInfo();
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                //throw new NotImplementedException();
                return;
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}
