using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EsriDe.RuntimeExplorer.Converter
{
	public class CountToVisibiltyConverter : BaseConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				return System.Convert.ToInt32(value) > 0 ? Visibility.Visible : Visibility.Hidden;
			}
			catch (Exception)
			{
				return value;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}