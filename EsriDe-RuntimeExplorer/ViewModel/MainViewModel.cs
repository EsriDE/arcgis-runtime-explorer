using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using EsriDe.RuntimeExplorer.Events;
using EsriDe.RuntimeExplorer.Theme;
using EsriDe.RuntimeExplorer.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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
                dlg.Filter = string.Join("|", new[]
                {
                    "ArcGIS Runtime Mobile Formats|*.mmpk;*.geodatabase",
                    "Mobile Map Packages|*.mmpk",
                    "Unpacked Mobile Map Packages|*.info",
                    "Mobile Geodatabases|*.geodatabase",
                    "All Files|*.*"
                });
                if (dlg.ShowDialog() == true)
                {
                    var selectedPath = dlg.FileName;
                    if (selectedPath.EndsWith(".info"))
                    {
                        selectedPath = System.IO.Path.GetDirectoryName(selectedPath);
                    }
                    FilePath = selectedPath;
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
                        ValidateSpatialReference(map);
                    }
                },
                () => mainDataViewModel.SelectedMapView != null);
            AddFileBasemapCommand = new RelayCommand(() =>
            {
                var dlg = new OpenFileDialog();
                dlg.Filter =
                    "ArcGIS Offline Basemap Packages|*.tpk;*.vtpk|ArcGIS Tile Package|*.tpk|ArcGIS Vector Tile Package|*.vtpk|All Files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    var map = mainDataViewModel?.SelectedMapView?.Map;
                    if (map != null)
                    {
                        switch (System.IO.Path.GetExtension(dlg.FileName).ToLower())
                        {
                            case ".tpk":
                                var tileCache = new TileCache(dlg.FileName);
                                var tileLayer = new ArcGISTiledLayer(tileCache);
                                map.Basemap = new Basemap(tileLayer);
                                ValidateSpatialReference(map);
                                break;
                            case ".vtpk":
                                //var vectorTileCache = new Esri.ArcGISRuntime.Mapping.VectorTileCache()
                                var vectorTileLayer = new ArcGISVectorTiledLayer(new Uri(dlg.FileName));
                                map.Basemap = new Basemap(vectorTileLayer);
                                ValidateSpatialReference(map);
                                break;
                            default:
                                break;
                        }

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

            ZoomToLayerCommand = new RelayCommand(async () =>
                {
                    var mapView = mainDataViewModel?.SelectedMapView?.MapView;
                    if (mapView != null)
                    {
                        var extent = mainDataViewModel.SelectedMapView.SelectedLayer.FullExtent;
                        await mapView.SetViewpointAsync(new Viewpoint(extent));
                    }

                },
                () => mainDataViewModel?.SelectedMapView?.SelectedLayer != null);

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(FilePath))
                {
                    mainDataViewModel.FilePath = FilePath;
                }
                if (args.PropertyName == nameof(MapDrawStatus))
                {
                    AppStatus = MapDrawStatus.ToString();
                }
            };

        }

        private static void ValidateSpatialReference(Map map)
        {
            var baseLayer = map.Basemap.BaseLayers.FirstOrDefault();
            if (baseLayer.LoadStatus != LoadStatus.Loaded)
            {
                baseLayer.Loaded += (sender, args) => BaseLayerOnLoaded(baseLayer, new MapEventArgs(map));
            }
            else
            {
                BaseLayerOnLoaded(baseLayer, new MapEventArgs(map));
            }
        }

        private static void BaseLayerOnLoaded(object o, MapEventArgs args)
        {
            var layer = (o as Layer);
            if (layer.LoadStatus == LoadStatus.Loaded)
            {
                var map = args.Map;
                var mapRef = map.SpatialReference.BaseGeographic;
                var baseLayer = map.Basemap.BaseLayers.FirstOrDefault();
                var baseRef = baseLayer?.SpatialReference?.BaseGeographic;

                if (mapRef != baseRef)
                {
                    MessageBox.Show(
                        string.Format(Properties.Resources.ErrorSpatialReferencesDoNotMatch, mapRef.Wkid.ToString() ?? "empty", baseRef.Wkid.ToString() ?? "empty"),
                        Properties.Resources.MessageBoxHint, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
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
        public ICommand AddFileBasemapCommand { get; private set; }
        public ICommand ShowAboutCommand { get; private set; }
        public ICommand ToggleLayerExtentGraphicsVisibility { get; private set; }
        public ICommand ZoomToLayerCommand { get; private set; }
        public ICommand IdentifyCommand { get; private set; }
    }
}