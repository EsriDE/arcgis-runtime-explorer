using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Esri.ArcGISRuntime.UI;

namespace EsriDe.RuntimeExplorer.Converter
{
    public class DrawStatusToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var drawStatus = (DrawStatus?) value;
            return drawStatus == DrawStatus.InProgress ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}