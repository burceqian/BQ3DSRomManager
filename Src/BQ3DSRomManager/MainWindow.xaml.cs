using BQ3DSCore;
using BQStructure;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using BQUtility;

namespace BQ3DSRomManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<RomInformation> _AllRomList = new List<RomInformation>();
        ObservableCollection<RomInformation> _RomList = new ObservableCollection<RomInformation>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Title = "BQ 3DS Rom Manager V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
            try
            {
                BQCore.Initialize();
            }
            catch (BQException bqE)
            {
                System.Windows.MessageBox.Show(bqE.BQErrorMessage);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            dgGameList.ItemsSource = _RomList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InitializeRomList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Initialize Rom List Error");
                System.Windows.MessageBox.Show(ex.ToString());
            }

        }

        private void reportProcess(int current, int max, string message)
        {
            this.Dispatcher.Invoke(new Action(()=>{
                progressbar.Value = current;
                progressbar.Maximum = max;
                labProgress.Content = message;
            }));
        }

        private void InitializeRomList()
        {
            List<RomInformation> _AllRomList = BQCore.InitializeFirstRomList();
            _AllRomList.Where(rominfo => BQIO.CheckRomFileExist(rominfo)).ToList().ForEach(rominfo => _RomList.Add(rominfo));
        }

        private void MenuItem_Click_UpdateDataBaseFrom3dsdb(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_UpdateIconFromGameTDB(object sender, RoutedEventArgs e)
        {

        }

        private void GetAllFiles(DirectoryInfo pDirectoryInfo, List<FileInfo> pFileInfoList)
        {
            FileInfo[] lFileList = pDirectoryInfo.GetFiles();
            foreach (var fileInfo in lFileList)
            {
                if (fileInfo.Extension.ToLower() == ".3ds" ||
                    fileInfo.Extension.ToLower() == ".3dz" ||
                    fileInfo.Extension.ToLower() == ".cia")
                {
                    pFileInfoList.Add(fileInfo);
                }
            }

            DirectoryInfo[] directoryInfoList = pDirectoryInfo.GetDirectories();
            foreach (var dir in directoryInfoList)
            {
                GetAllFiles(dir, pFileInfoList);
            }
        }

        #region Left Func Buttons
        private void MenuItem_Click_LoadRom(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3ds|*.3ds|3dz|*.3dz|CIA|*.cia|ZIP|*.zip|RAR|*.rar";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                List<RomInformation> tRomList = BQCore.LoadRom(fileInfo);
                foreach (var rom in tRomList)
                {
                    if (_RomList.FirstOrDefault(r => r.BasicInfo.Serial == rom.BasicInfo.Serial) == null)
                    {
                        _RomList.Add(rom);
                    }
                }
            }
        }

        private void MenuItem_Click_LoadRomByFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                List<RomInformation> tRomList = BQCore.LoadRom(directoryInfo);
                foreach (var rom in tRomList)
                {
                    if (_RomList.FirstOrDefault(r => r.BasicInfo.Serial == rom.BasicInfo.Serial) == null)
                    {
                        _RomList.Add(rom);
                    }
                }
            }
        }

        private void MenuItem_Click_Favorite(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_Setting(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }



        #region Local Right Mouse Click
        private void MenuItem_Click_ShowRomDetail(object sender, RoutedEventArgs e)
        {
            WRomInfo wRomInfo = new WRomInfo();
            wRomInfo.ShowDialog();
        }

        private void MenuItem_Click_ShowDuplicateRoms(object sender, RoutedEventArgs e)
        {

        }

        private void MenuIte_Click_ShowDifferentRoms(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
        #region SD Right Mouse Click
        private void MenuItem_Click_ShowSDRomDetail(object sender, RoutedEventArgs e)
        {
            WRomInfo wRomInfo = new WRomInfo();
            wRomInfo.ShowDialog();
        }
        #endregion
    }
}
