using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MLClassifierStation.Converters
{
    public class LogicalOrConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is bool) || !(values[1] is bool))
                return Visibility.Collapsed;

            bool a = (bool)values[0];
            bool b = (bool)values[1];

            return a || b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}