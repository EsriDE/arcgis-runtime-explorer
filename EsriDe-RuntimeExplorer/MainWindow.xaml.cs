using System;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace EsriDe.RuntimeExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FilePath_Changed(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            try
            {
                var text = (sender as TextBlock)?.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    Dispatcher.BeginInvoke((Action)(() => MapTabControl.SelectedIndex = 0));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
