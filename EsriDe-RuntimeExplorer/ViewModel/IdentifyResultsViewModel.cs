using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Data;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class IdentifyResultsViewModel : ViewModelBase
    {
        public IdentifyResultsViewModel(IEnumerable<IdentifyLayerResult> identifyLayerResults)
        {
            foreach (var result in identifyLayerResults)
            {
                IdentifyLayerResults.Add(result);
            }

            SelectedGeoElement = IdentifyLayerResults.FirstOrDefault()?.GeoElements.FirstOrDefault();
        }

        public ObservableCollection<IdentifyLayerResult> IdentifyLayerResults { get; } = new ObservableCollection<IdentifyLayerResult>();

        private GeoElement _selectedGeoElement;

        public GeoElement SelectedGeoElement
        {
            get => _selectedGeoElement;
            set => Set(ref _selectedGeoElement, value);
        }
    }
}
