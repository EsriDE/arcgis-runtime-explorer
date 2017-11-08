using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EsriDe.RuntimeExplorer
{
	public class StringToPenConverter : BaseConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				var color = (Color) ColorConverter.ConvertFromString(value?.ToString());
				return new Pen(new SolidColorBrush(color), 2);
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