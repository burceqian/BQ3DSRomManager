using BQStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BQ3DSRomManager
{
    /// <summary>
    /// Interaction logic for RomInfoWindow.xaml
    /// </summary>
    public partial class RomInfoWindow : Window
    {
        public RomInformation _RomInfo { get; set; }

        public RomInfoWindow(RomInformation pRomInfo)
        {
            _RomInfo = pRomInfo;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            imgGameIco.DataContext = _RomInfo;
            labSerial.DataContext = _RomInfo;
            labEnglishTitle.DataContext = _RomInfo;
            labTitleID.DataContext = _RomInfo;
            labPlayers.DataContext = _RomInfo;
            //gridBasicInfo.DataContext = _RomInfo;

            Image p1 = new Image() { Stretch = Stretch.UniformToFill, MaxWidth = 200};
            p1.Source = new BitmapImage(new Uri(@"D:\Documents\Pictures\5S\2010_05\20100507_IMG_0706.JPG"));
            listBoxRomPic.Items.Add(p1);

            //<Image Stretch="UniformToFill" MaxWidth="200" Width="auto"  Source="C:\aa.jpg"></Image>
        }
    }
}
