using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Esri.ArcGISRuntime.Mapping;

namespace EsriDe_RuntimeExplorer
{
    public class MapViewModel : DependencyObject
    {
        public Map Map { get; } = new Map(Basemap.CreateStreetsVector());
    }
}
