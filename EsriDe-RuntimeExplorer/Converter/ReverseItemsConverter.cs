using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace EsriDe.RuntimeExplorer.Converter
{
    public class ReverseItemsConverter : BaseConverter, IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object>)
            {
                var list = new List<object>();
                list.AddRange((IEnumerable<object>)value);
                list.Reverse();
                return list;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}