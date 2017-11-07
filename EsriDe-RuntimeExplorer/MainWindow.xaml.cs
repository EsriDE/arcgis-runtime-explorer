using System.Windows;
using Esri.ArcGISRuntime.UI;
using EsriDe.RuntimeExplorer.ViewModel;

namespace EsriDe.RuntimeExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GeoView_OnDrawStatusChanged(object sender, DrawStatusChangedEventArgs e)
        {
            if (this.DataContext is ViewModelLocator locator)
            {
                locator.Main.AppStatus = e.Status.ToString();
            }
        }
    }
}
