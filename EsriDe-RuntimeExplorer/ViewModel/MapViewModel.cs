using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.UI;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        private Esri.ArcGISRuntime.UI.Controls.MapView _mapView;

        public Esri.ArcGISRuntime.UI.Controls.MapView MapView
        {
            get => _mapView;
            set => Set(ref _mapView, value);
        }

        private Map _map = new Map(Basemap.CreateStreetsVector());

        public ObservableCollection<LegendItemViewModel> LegendItems { get; } = new ObservableCollection<LegendItemViewModel>();

        public Map Map
        {
            get => _map;
            set => Set(ref _map, value);
        }

        private GraphicsOverlayCollection _graphicsOverlays = new GraphicsOverlayCollection();

        public GraphicsOverlayCollection GraphicsOverlays
        {
            get => _graphicsOverlays;
            set => Set(ref _graphicsOverlays, value);
        }

        private Layer _selectedLayer;
        private LegendItemViewModel _selectedLegendItem;

        public Layer SelectedLayer
        {
            get => _selectedLayer;
            set => Set(ref _selectedLayer, value);
        }

        public LegendItemViewModel SelectedLegendItem
        {
            get => _selectedLegendItem;
            set
            {
                if (value == null) return;
                Set(ref _selectedLegendItem, value);
                SelectedLayer = value.Layer;
            }
        }

        private double _viewScale;

        public double ViewScale
        {
            get => _viewScale;
            set => Set(ref _viewScale, value);
        }

        private GeocodeResult _selectedGeocodeResult;

        public GeocodeResult SelectedGeocodeResult
        {
            get => _selectedGeocodeResult;
            set => Set(ref _selectedGeocodeResult, value);
        }

        public MapViewModel()
        {
            var fullExtentOverlay = new GraphicsOverlay();
            var fullExtentRenderer = new SimpleRenderer();
            var outlineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, Colors.Red, 1.0);
            var fillSymbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Null, Color.FromArgb(255, 0, 80, 0), outlineSymbol);
            fullExtentRenderer.Symbol = fillSymbol;
            fullExtentOverlay.Renderer = fullExtentRenderer;
            GraphicsOverlays.Add(fullExtentOverlay);

            PropertyChanged += async (sender, args) =>
            {
                if (args.PropertyName == nameof(Map))
                {
                    LegendItems.Clear();
                    foreach (var layer in Map.OperationalLayers.Reverse())
                    {
                        LegendItems.Add(new LegendItemViewModel(this, layer));
                    }
                    AllLayersCount = Map.AllLayers.Count;
                    OperationalLayersCount = Map.OperationalLayers.Count;
                    var fullExtentGraphics = await BuildFullExtentGraphicsAsync();
                    foreach (var fullExtentGraphic in fullExtentGraphics)
                    {
                        fullExtentOverlay.Graphics.Add(fullExtentGraphic);
                    }
                }
                if (args.PropertyName == nameof(SelectedBookmark))
                {
                    MapView.SetViewpoint(SelectedBookmark.Viewpoint);
                }
                if (args.PropertyName == nameof(SelectedGeocodeResult))
                {
                    if (SelectedGeocodeResult != null)
                    {
                        await MapView.SetViewpointGeometryAsync(SelectedGeocodeResult.Extent);
                    }
                }
            };


        }

        private async Task<IEnumerable<Graphic>> BuildFullExtentGraphicsAsync()
        {
            var graphicsList = new List<Graphic>();

            foreach (var layer in Map.OperationalLayers)
            {
                await layer.LoadAsync();

                var fullExtent = layer.FullExtent;
                
                var fullExtentGraphic = new Graphic(fullExtent);
                graphicsList.Add(fullExtentGraphic);
            }

            return graphicsList;
        }

        private Bookmark _selectedBookmark;
        public Bookmark SelectedBookmark
        {
            get => _selectedBookmark;
            set => Set(ref _selectedBookmark, value);
        }

        private int _allLayersCount;
        public int AllLayersCount
        {
            get => _allLayersCount;
            set => Set(ref _allLayersCount, value);
        }

        private int _operationalLayersCount;
        public int OperationalLayersCount
        {
            get => _operationalLayersCount;
            set => Set(ref _operationalLayersCount, value);
        }
    }
}
