using System;
using System.Globalization;
using System.Windows.Data;
using Esri.ArcGISRuntime.Geometry;

namespace EsriDe.RuntimeExplorer
{
	public class SpatialReferenceToWkidConverter : BaseConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as SpatialReference)?.Wkid;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}