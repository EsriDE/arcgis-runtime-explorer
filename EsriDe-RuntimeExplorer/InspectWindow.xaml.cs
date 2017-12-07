using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace EsriDe.RuntimeExplorer
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
