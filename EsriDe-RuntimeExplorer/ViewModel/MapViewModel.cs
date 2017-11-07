using System.Windows;
using Esri.ArcGISRuntime.Mapping;

namespace EsriDe.RuntimeExplorer.ViewModel
{
    public class MapViewModel : DependencyObject
    {
        public Map Map { get; } = new Map(Basemap.CreateStreetsVector());
    }
}
