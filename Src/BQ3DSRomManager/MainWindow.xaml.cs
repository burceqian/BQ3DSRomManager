using BQ3DSRomLoader;
using BQGetRomInfoListOnline;
using BQInterface;
using BQStructure;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BQ3DSRomManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RomInfo lRomInfo = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog lOFD = new OpenFileDialog();
            if (lOFD.ShowDialog() != true)
            {
                return;
            }
            IRomLoader ll = new Loader3DS();
            //ll = new LoaderCIA();
            lRomInfo = ll.GetRomInfo(lOFD.FileName);
            //WRomInfo lWRI = new WRomInfo();
            //lWRI.GameRomInfo = lRomInfo;
            //lWRI.ShowDialog();

            Webgametdb3ds webgametdb3Ds = new Webgametdb3ds();
            webgametdb3Ds.GetGameConver(lRomInfo.SubSerial);
            webgametdb3Ds.GetGameInfo(lRomInfo.SubSerial);

        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Web3dsdb w3d = new Web3dsdb();
            w3d.GetRomInfo(null);
        }
    }
}
