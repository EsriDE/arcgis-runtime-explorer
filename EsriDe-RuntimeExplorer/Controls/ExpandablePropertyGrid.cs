using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace EsriDe.RuntimeExplorer.Controls
{
    public class ExpandablePropertyGrid : PropertyGrid
    {
        public ExpandablePropertyGrid()
        {
            SelectedObjectChanged += PropertyGrid_OnSelectedObjectChanged;
        }

        private void PropertyGrid_OnSelectedObjectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is PropertyGrid grid)
            {
                foreach (PropertyItem prop in grid.Properties)
                {
                    prop.MinHeight = 22;
                    if (!prop.PropertyType.IsValueType && prop.PropertyType != typeof(string))
                    {
                        prop.IsExpandable = true;
                    }
                }
            }
        }
    }
}
