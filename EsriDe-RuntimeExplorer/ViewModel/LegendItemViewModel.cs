using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class LegendItemViewModel : ViewModelBase
    {
        private readonly Layer _layer;

        public Layer Layer => _layer;

        public LegendItemViewModel(MapViewModel mapViewModel, Layer layer)
        {
            _mapViewModel = mapViewModel;
            mapViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MapViewModel.ViewScale))
                {
                    IsVisibleAtCurrentScale = _layer.IsVisibleAtScale(_mapViewModel.ViewScale);
                }
            };
            _layer = layer;
            _layer.Loaded += (sender, args) =>
            {
                RaisePropertyChanged(null);
            };
            _layer.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(Layer.Name):
                        RaisePropertyChanged(nameof(Name));
                        break;
                    case nameof(Layer.IsVisible):
                        RaisePropertyChanged(nameof(IsVisible));
                        break;
                    case nameof(Layer.MinScale):
                        RaisePropertyChanged(nameof(MinScale));
                        break;
                }
            };
        }

        public string Name
        {
            get => _layer.Name;
            set
            {
                _layer.Name = value;
                RaisePropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => _layer.IsVisible;
            set
            {
                _layer.IsVisible = value;
                RaisePropertyChanged();
            }
        }

        public double MinScale
        {
            get => _layer.MinScale;
            set
            {
                _layer.MinScale = value;
                RaisePropertyChanged();
            }
        }

        private bool _isVisibleAtCurrentScale;
        private readonly MapViewModel _mapViewModel;

        public bool IsVisibleAtCurrentScale
        {
            get => _isVisibleAtCurrentScale;
            set => Set(ref _isVisibleAtCurrentScale, value);
        }
    }
}
