using BQ3DSCommonFunction;
using BQ3DSQLite;
using BQ3DSRomLoader;
using BQGetRomInfoListOnline;
using BQInterface;
using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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
            if (lOFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            IRomLoader ll = new Loader3DS();
            //ll = new LoaderCIA();
            lRomInfo = ll.GetRomInfo(new FileInfo(lOFD.FileName));
            //WRomInfo lWRI = new WRomInfo();
            //lWRI.GameRomInfo = lRomInfo;
            //lWRI.ShowDialog();

            Webgametdb3ds webgametdb3Ds = new Webgametdb3ds();
            //webgametdb3Ds.GetGameConver(lRomInfo.SubSerial);
            webgametdb3Ds.GetGameInfo(lRomInfo.SubSerial);

        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Web3dsdb w3d = new Web3dsdb();
            //w3d.Update3dsreleases();

            List<Rom3dsdbInfo> lRom3dsdbInfoList = w3d.GetRomInfoFrom3dsreleaseXML();

            if (BQSqlite.CheckDBExist() == false)
            {
                BQSqlite.CreateNewDB();
            }

            BQSqlite.InsertRom3dsdbInfo(lRom3dsdbInfoList);

            //foreach (var rom3dsdbInfo in lRom3dsdbInfoList)
            //{
            //    BQSqlite.InsertRom3dsdbInfo(rom3dsdbInfo);
            //}

            Rom3dsdbInfo lRom3dsdbInfo = BQSqlite.GetRom3dsdbInfo(lRom3dsdbInfoList[0].serial);
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();

            DirectoryInfo lDirectoryInfo = new DirectoryInfo(m_Dir);
            reportProcess(1, 1, "Start Scan All Game File");

            Task task = new Task(new Action(() => { ScanRomFolder(lDirectoryInfo); }));
            task.Start();
        }

        private void ScanRomFolder(DirectoryInfo folder)
        {
            if (BQSqlite.CheckDBExist() == false)
            {
                BQSqlite.CreateNewDB();
            }

            List<FileInfo> lGameList = new List<FileInfo>();
            lGameList = BQIO.GetAllFilesInDirectory(folder, lGameList);
            reportProcess(1, 1, "Found " + lGameList.Count + "Game Roms");

            for (int i = 0; i < lGameList.Count; i++)
            {
                
                FileInfo romfile = lGameList[i];
                reportProcess(i + 1, lGameList.Count, "Start Analize Game: " + romfile.Name);

                if (romfile.Extension.ToLower() == ".7z" ||
                    romfile.Extension.ToLower() == ".rar" ||
                        romfile.Extension.ToLower() == ".zip")
                {
                    BQZip.ClearUnZipFolder();
                    BQZip.UnZipFile(romfile);
                    List<FileInfo> unziprom = BQZip.GetUnZipRom();
                    for (int j = 0; j < unziprom.Count; j++)
                    {
                        AnalizeRom(unziprom[j]);
                    }
                    BQZip.ClearUnZipFolder();
                }
                else if (romfile.Extension.ToLower() == ".3ds" ||
                        romfile.Extension.ToLower() == ".3dz" ||
                        romfile.Extension.ToLower() == ".cia")
                {
                    AnalizeRom(romfile);
                }
                else
                {
                    BQIO.CopyToRomTemp(romfile);
                }
            }
            System.Windows.Forms.MessageBox.Show("Scan Done.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AnalizeRom(FileInfo romfile)
        {
            IRomLoader lRomLoader;

            if (romfile.Extension.ToLower() == ".3ds" ||
                    romfile.Extension.ToLower() == ".3dz")
            {
                lRomLoader = new Loader3DS();
            }
            else if (romfile.Extension.ToLower() == ".cia")
            {
                lRomLoader = new LoaderCIA();
            }
            else
            {
                return;
            }

            RomInfo lRomInfo = lRomLoader.GetRomInfo(romfile);
            if (lRomInfo.Serial.Trim() != "")
            {
                BQIO.CopyRomToRomFolder(lRomInfo, romfile);

                if (BQSqlite.GetRomInfo(lRomInfo.Serial) == null)
                {
                    BQSqlite.InsertRomInfo(lRomInfo);
                }

                if (BQSqlite.GetRomgamedb3dsInfo(lRomInfo.Serial) == null)
                {
                    Webgametdb3ds webgametdb3Ds = new Webgametdb3ds();
                    Romgamedb3dsInfo romgamedb3DsInfo = webgametdb3Ds.GetGameInfo(lRomInfo.SubSerial);
                    if (romgamedb3DsInfo != null)
                    {
                        romgamedb3DsInfo.serial = lRomInfo.Serial;
                        BQSqlite.InsertRomgamedb3dsInfo(romgamedb3DsInfo);
                        webgametdb3Ds.GetGameConver(lRomInfo.SubSerial);
                    }
                }
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

        private void btnUpdate3dsdb_Click(object sender, RoutedEventArgs e)
        {
            Update3dsdb();
        }

        private void Update3dsdb()
        {
            Web3dsdb w3d = new Web3dsdb();
            w3d.Update3dsreleases();

            List<Rom3dsdbInfo> lRom3dsdbInfoList = w3d.GetRomInfoFrom3dsreleaseXML();

            if (BQSqlite.CheckDBExist() == false)
            {
                BQSqlite.CreateNewDB();
            }

            BQSqlite.InsertRom3dsdbInfo(lRom3dsdbInfoList);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WRomInfo wRomInfo = new WRomInfo();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3ds|*.3ds|3dz|*.3dz|CIA|*.cia";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                AnalizeRom(fileInfo);
            }

        }

        private void MenuItem_Click_OpenFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                List<FileInfo> pRomList = new List<FileInfo>();
                GetAllFiles(directoryInfo, pRomList);
                foreach (var rom in pRomList)
                {
                    AnalizeRom(rom);
                }
            }
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

        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3ds|*.3ds|3dz|*.3dz|CIA|*.cia";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                AnalizeRom(fileInfo);
            }
        }

        private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
