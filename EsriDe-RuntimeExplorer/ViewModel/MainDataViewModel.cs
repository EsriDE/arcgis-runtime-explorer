using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class MainDataViewModel : ViewModelBase
    {
        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            set => Set(ref _filePath, value);
        }

        public ObservableCollection<MapViewModel> MapViews { get; } = new ObservableCollection<MapViewModel>();

        public MainDataViewModel()
        {
            PropertyChanged += async (sender, args) =>
            {
                if (args.PropertyName == nameof(FilePath))
                {
                    MapViews.Clear();
                    if (FilePath.EndsWith(".geodatabase"))
                    {
                        await OpenGeodatabaseAsync();
                    }
                    else if (FilePath.EndsWith(".mmpk"))
                    {
                        await OpenMmpkAsync();
                    }
                }
            };

        }

        private async Task OpenGeodatabaseAsync()
        {
            var geodatabase = await Geodatabase.OpenAsync(FilePath);
            var map = new Map(Basemap.CreateStreetsVector());
            foreach (var table in geodatabase.GeodatabaseFeatureTables)
            {
                var layer = new FeatureLayer(table);
                map.OperationalLayers.Add(layer);
            }
            MapViews.Add(new MapViewModel {Map = map});
        }

        private async Task OpenMmpkAsync()
        {
            var mmpk = await MobileMapPackage.OpenAsync(FilePath);
            foreach (var map in mmpk.Maps)
            {
                await map.LoadAsync();
                MapViews.Add(new MapViewModel {Map = map});
            }
        }
    }
}