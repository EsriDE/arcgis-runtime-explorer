using System.Diagnostics;
using System.Windows.Input;
using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Esri.ArcGISRuntime.UI;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(MainDataViewModel mainDataViewModel)
        {
            LayerDetailsCommand = new RelayCommand(() =>
                {
                    LayerDetailWindow view = new LayerDetailWindow();
                    view.ShowDialog();
                },
                () => mainDataViewModel.SelectedMapView != null);

            FileOpenCommand = new RelayCommand(() =>
            {
                Debug.WriteLine("File open");
                var dlg = new OpenFileDialog();
                dlg.Filter =
                    "ArcGIS Runtime Mobile Formats|*.mmpk;*.geodatabase|Mobile Map Packages (*.mmpk)|*.mmpk|Mobile Geodatabases (*.geodatabase)|*.geodatabase|All Files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    FilePath = dlg.FileName;
                }
            });

            InspectMmpkCommand = new RelayCommand(() =>
            {
                var inspectWindow = new InspectWindow
                {
                    DataContext = mainDataViewModel.Mmpk
                };
                inspectWindow.Show();
            }, () => mainDataViewModel.Mmpk != null);
            InspectGeodatabaseCommand = new RelayCommand(() =>
            {
                var inspectWindow = new InspectWindow
                {
                    DataContext = mainDataViewModel.Geodatabase
                };
                inspectWindow.Show();
            }, () => mainDataViewModel.Geodatabase != null);
            InspectMapCommand = new RelayCommand(() =>
            {
                var inspectWindow = new InspectWindow
                {
                    DataContext = mainDataViewModel.SelectedMapView.Map
                };
                inspectWindow.Show();
            }, () => mainDataViewModel.SelectedMapView != null);
            InspectLayerCommand = new RelayCommand(() =>
            {
                var inspectWindow = new InspectWindow
                {
                    DataContext = mainDataViewModel.SelectedMapView.SelectedLayer
                };
                inspectWindow.Show();
            }, () => mainDataViewModel?.SelectedMapView?.SelectedLayer != null);

            AddBasemapCommand = new RelayCommand(() =>
                {
                    var map = mainDataViewModel?.SelectedMapView?.Map;
                    if (map != null)
                    {
                        map.Basemap = Basemap.CreateTopographic();
                    }
                },
                () => mainDataViewModel.SelectedMapView != null);

            ShowAboutCommand = new RelayCommand(() =>
                {
                   // await((MainWindow)sender).ShowMessageAsync("", $"You clicked on {menuItem.Label} button");
                },
                () => mainDataViewModel.SelectedMapView != null);

            PropertyChanged += (sender, args) =>
            {
                mainDataViewModel.FilePath = FilePath;
            };

        }

        public ICommand FileOpenCommand { get; private set; }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { Set(ref _filePath, value); }
        }

        private DrawStatus _mapDrawStatus = DrawStatus.Completed;

        public DrawStatus MapDrawStatus
        {
            get { return _mapDrawStatus; }
            set { Set(ref _mapDrawStatus, value); }
        }

        public ICommand LayerDetailsCommand { get; private set; }
        public ICommand InspectMmpkCommand { get; private set; }
        public ICommand InspectGeodatabaseCommand { get; private set; }
        public ICommand InspectMapCommand { get; private set; }
        public ICommand InspectLayerCommand { get; private set; }
        public ICommand AddBasemapCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
    }
}