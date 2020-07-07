using MLClassifierStation.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MLClassifierStation.Converters
{
    public class TypeNameTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string resourceName = string.Format("{0}Tooltip", value.GetType().Name);
            string resource = Resources.ResourceManager.GetString(resourceName);
            return string.IsNullOrWhiteSpace(resource)
                ? resourceName
                : resource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}