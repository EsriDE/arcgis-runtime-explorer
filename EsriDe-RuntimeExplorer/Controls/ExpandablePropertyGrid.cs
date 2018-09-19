using Xceed.Wpf.Toolkit.PropertyGrid;

namespace EsriDe.RuntimeExplorer.Controls
{
    public class ExpandablePropertyGrid : PropertyGrid
    {
        public ExpandablePropertyGrid()
        {
            PreparePropertyItem += LetsPreparePropertyItem;
        }

        private void LetsPreparePropertyItem(object sender, PropertyItemEventArgs e)
        {
            var prop = e.PropertyItem as PropertyItem;
            if (prop == null) return;

            prop.MinHeight = 22;
            if (!prop.PropertyType.IsValueType && prop.PropertyType != typeof(string))
            {
                prop.IsExpandable = true;
            }
        }
    }
}
