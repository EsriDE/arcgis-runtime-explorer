﻿using System;
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

namespace EsriDe.RuntimeExplorer
{
	/// <summary>
	/// Interaction logic for LayerDetailWindow.xaml
	/// </summary>
	public partial class LayerDetailWindow : Window
	{
		public LayerDetailWindow()
		{
			InitializeComponent();

			FeatureLayerGrid.AutoGeneratingColumn += FeatureLayerGrid_AutoGeneratingColumn;
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
