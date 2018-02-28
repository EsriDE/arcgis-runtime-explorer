using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using MahApps.Metro;
using EsriDe.RuntimeExplorer.Theme;
using System.Collections.Generic;
using System.Windows.Media;
//using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

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
            // create accent color menu items for the demo
            this.AccentColors = ThemeManager.Accents
                .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                .ToList();

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
            InspectBackgroundGridCommand = new RelayCommand(() =>
            {
                var inspectWindow = new InspectWindow
                {
                    DataContext = mainDataViewModel.SelectedMapView.MapView.BackgroundGrid
                };
                inspectWindow.Show();
            }, () => mainDataViewModel?.SelectedMapView?.MapView?.BackgroundGrid != null);

            AddBasemapCommand = new RelayCommand(() =>
                {
                    var map = mainDataViewModel?.SelectedMapView?.Map;
                    if (map != null)
                    {
                        map.Basemap = Basemap.CreateTopographic();
                        if (map.SpatialReference != map.Basemap.BaseLayers.First().SpatialReference)
                        {
                            MessageBox.Show(
                                "Basemap applied, but SpatialReference of current map differs from basemaps SpatialReference. Basemap is not visible.");
                        }
                    }
                },
                () => mainDataViewModel.SelectedMapView != null);
            AddTpkBasemapCommand = new RelayCommand(() =>
            {
                var dlg = new OpenFileDialog();
                dlg.Filter =
                    "ArcGIS Tile Package|*.tpk|All Files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    var map = mainDataViewModel?.SelectedMapView?.Map;
                    if (map != null)
                    {
                        var tileCache = new TileCache(dlg.FileName);
                        var tileLayer = new ArcGISTiledLayer(tileCache);
                        map.Basemap = new Basemap(tileLayer);
                    }
                }
            },
            () => mainDataViewModel.SelectedMapView != null);

            ShowAboutCommand = new RelayCommand(() =>
                {
                   // await((MainWindow)sender).ShowMessageAsync("", $"You clicked on {menuItem.Label} button");
                },
                () => mainDataViewModel.SelectedMapView != null);

            ToggleLayerExtentGraphicsVisibility = new RelayCommand(() =>
            {
                LayerExtentGraphicsVisible = !LayerExtentGraphicsVisible;
                mainDataViewModel.SelectedMapView.LayerExtentGraphicsVisible = LayerExtentGraphicsVisible;
            });

            PropertyChanged += (sender, args) =>
            {
                mainDataViewModel.FilePath = FilePath;
                if (args.PropertyName == nameof(MapDrawStatus))
                {
                    AppStatus = MapDrawStatus.ToString();
                }
            };

        }

        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }

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

        private string _appStatus = String.Empty;

        public string AppStatus
        {
            get => _appStatus;
            set => Set(ref _appStatus, value);
        }

        private bool _layerExtentGraphicsVisible;

        public bool LayerExtentGraphicsVisible
        {
            get => _layerExtentGraphicsVisible;
            set => Set(ref _layerExtentGraphicsVisible, value);
        }

        public ICommand LayerDetailsCommand { get; private set; }
        public ICommand InspectMmpkCommand { get; private set; }
        public ICommand InspectGeodatabaseCommand { get; private set; }
        public ICommand InspectMapCommand { get; private set; }
        public ICommand InspectLayerCommand { get; private set; }
        public ICommand InspectBackgroundGridCommand { get; private set; }
        public ICommand AddBasemapCommand { get; private set; }
        public ICommand AddTpkBasemapCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ToggleLayerExtentGraphicsVisibility { get; private set; }
    }
}