using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Esri.ArcGISRuntime.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class IdentifyResultsViewModel : ViewModelBase
    {
        public IdentifyResultsViewModel()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SelectedLayerIndex))
                {
                    SelectedLayerDisplayIndex = SelectedLayerIndex + 1;
                }
                if (args.PropertyName == nameof(SelectedElementIndex))
                {
                    SelectedElementDisplayIndex = SelectedElementIndex + 1;
                }
            };
            LayerLeftCommand = new RelayCommand(() =>
            {
                SelectedLayerIndex -= 1;
                SelectedElementIndex = 0;
                SelectedGeoElement = IdentifyLayerResults[SelectedLayerIndex].GeoElements[SelectedElementIndex];
            },
            () => SelectedLayerIndex > 0);
            LayerRightCommand = new RelayCommand(() =>
            {
                SelectedLayerIndex += 1;
                SelectedElementIndex = 0;
                SelectedGeoElement = IdentifyLayerResults[SelectedLayerIndex].GeoElements[SelectedElementIndex];
            },
            () => SelectedLayerDisplayIndex < LayerCount);
            ElementLeftCommand = new RelayCommand(() =>
            {
                SelectedElementIndex -= 1;
                SelectedGeoElement = IdentifyLayerResults[SelectedLayerIndex].GeoElements[SelectedElementIndex];
            },
            () => SelectedElementIndex > 0);
            ElementRightCommand = new RelayCommand(() =>
            {
                SelectedElementIndex += 1;
                SelectedGeoElement = IdentifyLayerResults[SelectedLayerIndex].GeoElements[SelectedElementIndex];
            },
            () => SelectedElementDisplayIndex < ElementCount);
        }

        public IdentifyResultsViewModel(IEnumerable<IdentifyLayerResult> identifyLayerResults) : this()
        {
            foreach (var result in identifyLayerResults)
            {
                IdentifyLayerResults.Add(result);
            }

            SelectedLayerIndex = 0;
            SelectedLayerDisplayIndex = 1;
            LayerCount = IdentifyLayerResults.Count;
            SelectedElementIndex = 0;
            SelectedElementDisplayIndex = 1;
            ElementCount = IdentifyLayerResults.First().GeoElements.Count;
            SelectedGeoElement = IdentifyLayerResults[SelectedLayerIndex].GeoElements[SelectedElementIndex];
        }

        public ObservableCollection<IdentifyLayerResult> IdentifyLayerResults { get; } = new ObservableCollection<IdentifyLayerResult>();

        private GeoElement _selectedGeoElement;

        public GeoElement SelectedGeoElement
        {
            get => _selectedGeoElement;
            set => Set(ref _selectedGeoElement, value);
        }

        private int _selectedLayerIndex;

        public int SelectedLayerIndex
        {
            get => _selectedLayerIndex;
            set => Set(ref _selectedLayerIndex, value);
        }

        private int _selectedLayerDisplayIndex;

        public int SelectedLayerDisplayIndex
        {
            get => _selectedLayerDisplayIndex;
            set => Set(ref _selectedLayerDisplayIndex, value);
        }

        private int _layerCount;

        public int LayerCount
        {
            get => _layerCount;
            set => Set(ref _layerCount, value);
        }

        private int _selectedElementIndex;

        public int SelectedElementIndex
        {
            get => _selectedElementIndex;
            set => Set(ref _selectedElementIndex, value);
        }

        private int _selectedElementDisplayIndex;

        public int SelectedElementDisplayIndex
        {
            get => _selectedElementDisplayIndex;
            set => Set(ref _selectedElementDisplayIndex, value);
        }

        private int _elementCount;

        public int ElementCount
        {
            get => _elementCount;
            set => Set(ref _elementCount, value);
        }

        public ICommand LayerLeftCommand { get; private set; }
        public ICommand LayerRightCommand { get; private set; }
        public ICommand ElementLeftCommand { get; private set; }
        public ICommand ElementRightCommand { get; private set; }
    }
}
