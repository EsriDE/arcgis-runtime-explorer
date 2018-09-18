using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EsriDe.RuntimeExplorer.Properties;
using MahApps.Metro.Controls;

namespace EsriDe.RuntimeExplorer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        //ToDo: not so pretty her - has to be in ViewModel
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Settings.Default.UserLiscenseHasSeenOnce)
            {
                ShowAbout(null, null);
                Settings.Default.UserLiscenseHasSeenOnce = true;
                Settings.Default.Save();
            }
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

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            var flyout = Flyouts.Items.OfType<Flyout>().First(f => f.Name == "AboutFlyout");
            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}
