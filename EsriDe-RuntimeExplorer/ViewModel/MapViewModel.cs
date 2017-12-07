using System.Collections.ObjectModel;
using System.Linq;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks.Geocoding;
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
