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
        List<RomInformation> _TempRomInfoList = new List<RomInformation>();
        ObservableCollection<RomInformation> _RomList = new ObservableCollection<RomInformation>();
        ProgressForm _ProgressForm = new ProgressForm();

        public MainWindow()
        {
            BQLog.initilize(_ProgressForm.UpdateProgress);
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            this.Title = "BQ 3DS Rom Manager V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major + "." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor;
        }

        private void TaskStart(Action task, Action continueTask)
        {
            if (_ProgressForm.Owner == null)
            {
                _ProgressForm.Owner = this;
            }
            _ProgressForm.UpdateProgress("", 1, 100);
            _ProgressForm.RunTask(task, continueTask);
            _ProgressForm.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            dgGameList.ItemsSource = _RomList;

            InitilizeForm();
        }

        private void InitilizeForm()
        {
            TaskStart(new Action(() =>
            {
                try
                {
                    BQLog.UpdateProgress("初始化组件", 1, 5);
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

                try
                {
                    BQLog.UpdateProgress("初始化Rom列表", 4, 5);
                    InitializeRomList();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Initialize Rom List Error");
                    System.Windows.MessageBox.Show(ex.ToString());
                }

                BQLog.UpdateProgress("初始化完成", 5, 5);
            }), UpdateRomList);
        }

        List<RomInformation> lExistRomList;
        private void InitializeRomList()
        {
            _TempRomInfoList = BQCore.InitializeFirstRomList();
        }

        private void UpdateRomList()
        {
            foreach (var romInfo in _TempRomInfoList)
            {
                romInfo.ExpandInfo.LargeIcon = BQIO.GetRomLargeIco(romInfo);
                romInfo.ExpandInfo.SmallIcon = BQIO.GetRomSmallIco(romInfo);
            }

            _TempRomInfoList.ForEach(rominfo =>
            {
                if (_RomList.FirstOrDefault(rom=>rom.BasicInfo.Serial == rominfo.BasicInfo.Serial) == null)
                {
                    _RomList.Add(rominfo);
                }
            });
        }

        private void MenuItem_Click_UpdateDataBaseFrom3dsdb(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_UpdateIconFromGameTDB(object sender, RoutedEventArgs e)
        {

        }

        #region Left Func Buttons
        private void MenuItem_Click_LoadRom(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3dsRom|*.3ds;*.3dz;*.cia|ZIPFile|*.zip;*.rar;*.7z";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadRom(openFileDialog.FileName);
            }
        }

        private void LoadRom(string fileName)
        {
            TaskStart(new Action(() =>
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _TempRomInfoList = BQCore.LoadRom(fileInfo);

            }), UpdateRomList);
        }

        private void MenuItem_Click_LoadRomByFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadRomByFolder(folderBrowserDialog.SelectedPath);
            }
        }

        private void LoadRomByFolder(string foldername)
        {
            TaskStart(new Action(() =>
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(foldername);
                _TempRomInfoList = BQCore.LoadRom(directoryInfo);

            }), UpdateRomList);
        }

        private void MenuItem_Click_Favorite(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_Setting(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Local Right Mouse Click
        private void MenuItem_Click_ShowRomDetail(object sender, RoutedEventArgs e)
        {
            //WRomInfo wRomInfo = new WRomInfo();
            //wRomInfo.ShowDialog();
            RomInfoWindow rominfowindow = new BQ3DSRomManager.RomInfoWindow();
            rominfowindow.Show();
        }

        private void MenuItem_Click_ShowDuplicateRoms(object sender, RoutedEventArgs e)
        {

        }

        private void MenuIte_Click_ShowDifferentRoms(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region SD Right Mouse Click
        private void MenuItem_Click_ShowSDRomDetail(object sender, RoutedEventArgs e)
        {
            WRomInfo wRomInfo = new WRomInfo();
            wRomInfo.ShowDialog();
        }
        #endregion

        private void Button_Click_MoveToSD(object sender, RoutedEventArgs e)
        {
        }
    }
}
