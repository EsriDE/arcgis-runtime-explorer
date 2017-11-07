using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace EsriDe.RuntimeExplorer.ViewModel
{
	public class LayerDetailViewModel : ViewModelBase
	{
		public Map Map { get; }

		public LayerDetailViewModel()
		{
			var instance = SimpleIoc.Default.GetInstance<MainDataViewModel>();
			Map = instance.SelectedMapView.Map;
		}

		public LayerCollection Layers
		{
			get { return Map.OperationalLayers; }
		}
	}
}