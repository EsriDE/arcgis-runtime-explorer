using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace EsriDe.RuntimeExplorer.Views
{
    /// <summary>
    /// Interaction logic for InspectWindow.xaml
    /// </summary>
    public partial class InspectWindow : Window
    {
        public InspectWindow()
        {
            InitializeComponent();
        }

        private void PropertyGrid_OnSelectedObjectChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is PropertyGrid grid)
            {
                foreach (PropertyItem prop in grid.Properties)
                {
                    prop.IsExpandable = true;
                }
            }
        }
    }
}
