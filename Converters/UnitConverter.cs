using System;
using System.Globalization;
using System.Windows.Data;

namespace ModernWPFApp.Converters
{
    public class UnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("F2", CultureInfo.InvariantCulture);
            }
            else if (value is double doubleValue)
            {
                return doubleValue.ToString("F2", CultureInfo.InvariantCulture);
            }
            else if (value is float floatValue)
            {
                return floatValue.ToString("F2", CultureInfo.InvariantCulture);
            }
            
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            
            return 0m;
        }
    }
}


