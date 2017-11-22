using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace EsriDe.RuntimeExplorer
{
	/// <summary>
	/// Interaction logic for LayerDetailWindow.xaml
	/// </summary>
	public partial class LayerDetailWindow : MetroWindow
	{
		public LayerDetailWindow()
		{
			InitializeComponent();

			FeatureLayerGrid.AutoGeneratingColumn += FeatureLayerGrid_AutoGeneratingColumn;
		    this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.9);
		    this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.9);
        }

		private void FeatureLayerGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			if (e.PropertyName == "SpatialReference")
			{
				var templateColumn = new DataGridTemplateColumn
				{
					Header = e.PropertyName,
					CellTemplate = (DataTemplate) Resources["SpatRefTemplate"],
					SortMemberPath = e.PropertyName
				};
				e.Column = templateColumn;
			}
			if (e.PropertyName == "SelectionColor")
			{
				var templateColumn = new DataGridTemplateColumn
				{
					Header = e.PropertyName,
					CellTemplate = (DataTemplate)Resources["ColorTemplate"],
					SortMemberPath = e.PropertyName
				};
				e.Column = templateColumn;
			}
		}
	}
}
