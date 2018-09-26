using System;
using Esri.ArcGISRuntime.Mapping;

namespace EsriDe.RuntimeExplorer.Events
{
    public class MapEventArgs : EventArgs
    {
        public Map Map { get; set; }

        public MapEventArgs(Map map)
        {
            Map = map;
        }
    }
}
