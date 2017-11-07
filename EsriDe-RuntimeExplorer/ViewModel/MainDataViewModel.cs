using System.Collections.ObjectModel;
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
                    var mmpk = await MobileMapPackage.OpenAsync(FilePath);
                    foreach (var map in mmpk.Maps)
                    {
                        MapViews.Add(new MapViewModel{Map = map});
                    }
                }
            };

        }
    }
}