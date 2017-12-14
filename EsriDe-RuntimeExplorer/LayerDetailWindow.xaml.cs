using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using MahApps.Metro.Controls;

namespace EsriDe.RuntimeExplorer
{
    /// <summary>
    /// Interaction logic for LayerDetailWindow.xaml
    /// </summary>
    public partial class LayerDetailWindow : MetroWindow
    {
        private const double ScaleFactor = 0.95;

        public LayerDetailWindow()
        {
            InitializeComponent();

            FeatureLayerGrid.AutoGeneratingColumn += FeatureLayerGrid_AutoGeneratingColumn;
            Height = SystemParameters.PrimaryScreenHeight * ScaleFactor;
            Width = SystemParameters.PrimaryScreenWidth * ScaleFactor;
        }

        private void FeatureLayerGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(FeatureTable))
            {
                e.Column = new DataGridTemplateColumn
                {
                    Header = e.PropertyName,
                    CellTemplate = (DataTemplate)Resources["FeatureTableTemplate"]
                };
            }
            if (e.PropertyType == typeof(Renderer))
            {
                e.Column = new DataGridTemplateColumn
                {
                    Header = e.PropertyName,
                    CellTemplate = (DataTemplate)Resources["RendererTemplate"]
                };
            }
            if (e.PropertyType == typeof(SpatialReference))
            {
                e.Column = new DataGridTemplateColumn
                {
                    Header = e.PropertyName,
                    CellTemplate = (DataTemplate) Resources["SpatRefTemplate"]
                };
            }
            if (e.PropertyType == typeof(Color))
            {
                var templateColumn = new DataGridTemplateColumn
                {
                    Header = e.PropertyName,
                    CellTemplate = (DataTemplate) Resources["ColorTemplate"]
                };
                e.Column = templateColumn;
            }
        }
    }
}