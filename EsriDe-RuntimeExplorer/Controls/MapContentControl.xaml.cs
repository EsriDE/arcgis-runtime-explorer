using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonServiceLocator;
using Esri.ArcGISRuntime.Toolkit.UI.Controls;
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

        private void Compass_OnTouchDown(object sender, TouchEventArgs e)
        {
            if (sender is Compass compass)
            {
                compass.Heading = 0;
            }
        }

        private async void MapView_OnKeyDown(object sender, KeyEventArgs e)
        {
            var mapView = (MapView)sender;
            
            if (e.Key == Key.PageDown && !mapView.IsNavigating)
            {
                await mapView.SetViewpointRotationAsync(mapView.MapRotation + 10);
            }

            if (e.Key == Key.PageUp && !mapView.IsNavigating)
            {
                await mapView.SetViewpointRotationAsync(mapView.MapRotation - 10);
            }
        }
    }
}
