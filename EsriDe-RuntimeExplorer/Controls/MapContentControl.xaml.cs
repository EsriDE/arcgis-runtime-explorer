using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ServiceLocation;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using EsriDe.RuntimeExplorer.ViewModel;

namespace EsriDe.RuntimeExplorer.Controls
{
    /// <summary>
    /// Interaction logic for MapContentControl.xaml
    /// </summary>
    public partial class MapContentControl : UserControl
    {
        public MapContentControl()
        {
            InitializeComponent();
        }

        private void GeoView_OnDrawStatusChanged(object sender, DrawStatusChangedEventArgs e)
        {
            // TODO: Accessing the ServiceLocator here is not good practice..
            var mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>();
            mainViewModel.MapDrawStatus = e.Status;
            if (e.Status == DrawStatus.Completed)
            {
                var mapView = sender as MapView;
                var mapViewModel = ServiceLocator.Current.GetInstance<MainDataViewModel>().SelectedMapView;
                mapViewModel.ViewScale = (double)mapView?.MapScale;
            }
        }

        private void MapView_OnNavigationCompleted(object sender, EventArgs e)
        {
            var mapView = sender as MapView;
            var mapViewModel = ServiceLocator.Current.GetInstance<MainDataViewModel>().SelectedMapView;
            mapViewModel.ViewScale = (double) mapView?.MapScale;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = e.NewValue;
            var mapViewModel = dataContext as MapViewModel;
            if (mapViewModel != null)
            {
                mapViewModel.MapView = MapView;
            }
        }
    }
}
