using MLClassifierStation.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MLClassifierStation.Converters
{
    public class TypeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string typeName = value.GetType().Name;
            string resource = Resources.ResourceManager.GetString(typeName);
            return string.IsNullOrWhiteSpace(resource)
                ? typeName
                : resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}