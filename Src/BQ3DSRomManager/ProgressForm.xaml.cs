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
    /// ProgressForm.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressForm : Window
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void RunTask(Action task,Action continueTask)
        {
            Task tTask = new Task(task);
            tTask.ContinueWith(new Action<Task>((t) => {
                this.Dispatcher.Invoke(new Action(() => {
                    continueTask?.Invoke();
                    this.Visibility = Visibility.Hidden;
                }));
            }));
            tTask.Start();
        }

        public void UpdateProgress(string message, int value, int max)
        {
            this.Dispatcher.Invoke(new Action(() => {
                progressbar.Value = value;
                progressbar.Maximum = max;
                progressmessage.Content = message;
            }));
        }
    }
}
