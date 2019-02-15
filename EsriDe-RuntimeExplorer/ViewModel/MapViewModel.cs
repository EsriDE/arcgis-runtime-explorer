using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using EsriDe.RuntimeExplorer.Controls;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        private MapView _mapView;

        public MapView MapView
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

        private bool _layerExtentGraphicsVisible;

        public bool LayerExtentGraphicsVisible
        {
            get => _layerExtentGraphicsVisible;
            set => Set(ref _layerExtentGraphicsVisible, value);
        }

        private bool _bookmarkExtentGraphicsVisible = true;

        public bool BookmarkExtentGraphicsVisible
        {
            get => _bookmarkExtentGraphicsVisible;
            set => Set(ref _bookmarkExtentGraphicsVisible, value);
        }

        private bool _identifyModeEnabled;
        public bool IdentifyModeEnabled
        {
            get => _identifyModeEnabled;
            set => Set(ref _identifyModeEnabled, value);
        }

        public MapViewModel()
        {
            var fullExtentOverlay = new GraphicsOverlay();
            fullExtentOverlay.IsVisible = LayerExtentGraphicsVisible;
            var fullExtentRenderer = new SimpleRenderer();
            var outlineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 1.0);
            var fillSymbol = new SimpleFillSymbol(SimpleFillSymbolStyle.Null, System.Drawing.Color.Black, outlineSymbol);
            fullExtentRenderer.Symbol = fillSymbol;
            fullExtentOverlay.Renderer = fullExtentRenderer;
            GraphicsOverlays.Add(fullExtentOverlay);

            var bookmarkExtentOverlay = new GraphicsOverlay();
            bookmarkExtentOverlay.IsVisible = BookmarkExtentGraphicsVisible;
            var bookmarkExtentRenderer = new SimpleRenderer();
            var outlineSymbol2 = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Orange, 1.0);
            var fillSymbol2 = new SimpleFillSymbol(SimpleFillSymbolStyle.Null, System.Drawing.Color.Black, outlineSymbol2);
            bookmarkExtentRenderer.Symbol = fillSymbol2;
            bookmarkExtentOverlay.Renderer = bookmarkExtentRenderer;
            GraphicsOverlays.Add(bookmarkExtentOverlay);

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
                    fullExtentOverlay.Graphics.Clear();
                    await BuildFullExtentGraphicsAsync(fullExtentOverlay.Graphics);
                    bookmarkExtentOverlay.Graphics.Clear();
                    await BuildBookmarkExtentGraphicsAsync(bookmarkExtentOverlay.Graphics);
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
                if (args.PropertyName == nameof(LayerExtentGraphicsVisible))
                {
                    fullExtentOverlay.IsVisible = LayerExtentGraphicsVisible;
                }
                if (args.PropertyName == nameof(BookmarkExtentGraphicsVisible))
                {
                    bookmarkExtentOverlay.IsVisible = BookmarkExtentGraphicsVisible;
                }
                if (args.PropertyName == nameof(MapView))
                {
                    MapView.GeoViewTapped += MapViewOnGeoViewTapped;
                }
            };
        }

        private async void MapViewOnGeoViewTapped(object s, GeoViewInputEventArgs e)
        {
            if (IdentifyModeEnabled)
            {
                // get the tap location in screen units
                System.Windows.Point tapScreenPoint = e.Position;
                

                var pixelTolerance = 20;
                var returnPopupsOnly = false;
                var maxResultsPerLayer = 100;


                // identify all layers in the MapView, passing the tap point, tolerance, types to return, and max results
                var identifyLayerResults = await MapView.IdentifyLayersAsync(tapScreenPoint, pixelTolerance, returnPopupsOnly, maxResultsPerLayer);
                if (identifyLayerResults.Any())
                {
                    var identifyResultsControl = new IdentifyResultsControl
                    {
                        DataContext = new IdentifyResultsViewModel(identifyLayerResults)
                    };
                    MapView.ShowCalloutAt(e.Location, identifyResultsControl);
                }
                else
                {
                    MapView.DismissCallout();
                }
            }
        }

        private async Task BuildFullExtentGraphicsAsync(GraphicCollection graphicCollection)
        {
            foreach (var layer in Map.OperationalLayers)
            {
                await layer.LoadAsync();

                var fullExtentGraphic = new Graphic(layer.FullExtent);
                graphicCollection.Add(fullExtentGraphic);
            }
        }

        private async Task BuildBookmarkExtentGraphicsAsync(GraphicCollection graphicCollection)
        {
            foreach (var bookmark in Map.Bookmarks)
            {
                var bookmarkExtentGraphic = new Graphic(bookmark.Viewpoint.TargetGeometry);
                graphicCollection.Add(bookmarkExtentGraphic);

                var textSym = new TextSymbol
                {
                    Text = bookmark.Name,
                    FontFamily = "Tahoma", Size = 12,
                    Color = Color.Orange,
                    HaloColor = Color.White, HaloWidth = 2,
                    HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom
                };
                var bottomLeftPoint = new MapPoint(bookmark.Viewpoint.TargetGeometry.Extent.XMin, bookmark.Viewpoint.TargetGeometry.Extent.YMin);
                var labelGraphic = new Graphic(bottomLeftPoint, textSym);
                graphicCollection.Add(labelGraphic);
            }
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
