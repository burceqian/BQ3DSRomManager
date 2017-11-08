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
            lRomInfo = ll.GetRomInfo(lOFD.FileName);
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
                IRomLoader lRomLoader;
                FileInfo romfile = lGameList[i];
                reportProcess(i + 1, lGameList.Count, "Start Analize Game: " + romfile.Name);
                if (romfile.Extension.ToLower() == ".3ds")
                {
                    lRomLoader = new Loader3DS();
                }
                else if (romfile.Extension.ToLower() == ".cia")
                {
                    lRomLoader = new LoaderCIA();
                }
                else
                {
                    continue;
                }

                RomInfo lRomInfo = lRomLoader.GetRomInfo(romfile.FullName);
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
                        romgamedb3DsInfo.serial = lRomInfo.Serial;
                        BQSqlite.InsertRomgamedb3dsInfo(romgamedb3DsInfo);
                        webgametdb3Ds.GetGameConver(lRomInfo.SubSerial);
                    }
                }




                Thread.Sleep(1);
            }
            System.Windows.Forms.MessageBox.Show("Scan Done.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void reportProcess(int current, int max, string message)
        {
            this.Dispatcher.Invoke(new Action(()=>{
                progressbar.Value = current;
                progressbar.Maximum = max;
                labProgress.Content = message;
            }));
        }
    }
}
