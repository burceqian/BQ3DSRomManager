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
    /// WRomInfo.xaml 的交互逻辑
    /// </summary>
    public partial class WRomInfo : Window
    {
        public RomInfo GameRomInfo { get; set; }

        public WRomInfo()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSerial.Text = GameRomInfo.Serial;
            txtCapacity.Text = GameRomInfo.Capacity;
            txtTitle_ID.Text = GameRomInfo.Title_ID;
            txtCard_Type.Text = Enum.GetName(typeof(CardType),GameRomInfo.Card_Type);
            txtCard_ID.Text = GameRomInfo.Card_ID;
            txtChip_ID.Text = GameRomInfo.Chip_ID;
            txtManufacturer.Text = GameRomInfo.Manufacturer;
        }
    }
}
