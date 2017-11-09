using System.Collections.ObjectModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows;
using Esri.ArcGISRuntime.Mapping;
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

        public MapViewModel()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Map))
                {
                    LegendItems.Clear();
                    foreach (var layer in Map.OperationalLayers.Reverse())
                    {
                        LegendItems.Add(new LegendItemViewModel(this, layer));
                    }
                }
                if (args.PropertyName == nameof(SelectedBookmark))
                {
                    MapView.SetViewpoint(SelectedBookmark.Viewpoint);
                }
            };
        }

        private Bookmark _selectedBookmark;

        public Bookmark SelectedBookmark
        {
            get => _selectedBookmark;
            set => Set(ref _selectedBookmark, value);
        }
    }
}
