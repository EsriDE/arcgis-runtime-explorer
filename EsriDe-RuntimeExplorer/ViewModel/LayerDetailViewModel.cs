using System.Collections.Generic;
using System.Linq;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Mapping;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace EsriDe.RuntimeExplorer.ViewModel
{
	public class LayerDetailViewModel : ViewModelBase
	{
		public Map Map { get; set; }

		public LayerDetailViewModel()
		{
			var instance = SimpleIoc.Default.GetInstance<MainDataViewModel>();
			Map = instance.SelectedMapView.Map;

			//der Wechsel eines Tabs interessiert mich
			instance.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(MainDataViewModel.SelectedMapView))
				{
					Map = instance.SelectedMapView.Map;
					//der Wechsel einer Map innerhalb eines Tabs interessiert mich auch
					instance.SelectedMapView.PropertyChanged += (sender1, args1) =>
					{
						if (args1.PropertyName == nameof(MapViewModel.Map))
						{
							Map = instance.SelectedMapView.Map;
						}
					};
				}
			};
		}
		

		public IEnumerable<FeatureLayer> FeatureLayers
		{
			get { return Map.OperationalLayers.OfType<FeatureLayer>(); }
		}

		public IEnumerable<RasterLayer> RasterLayers
		{
			get { return Map.OperationalLayers.OfType<RasterLayer>(); }
		}
	}
}