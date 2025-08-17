using System;
using System.Globalization;
using System.Windows.Data;

namespace ModernWPFApp.Converters
{
    public class EuroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C2", new CultureInfo("de-DE"));
            }
            else if (value is double doubleValue)
            {
                return doubleValue.ToString("C2", new CultureInfo("de-DE"));
            }
            else if (value is float floatValue)
            {
                return floatValue.ToString("C2", new CultureInfo("de-DE"));
            }
            
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                // Remove currency symbol and parse
                string cleanValue = stringValue.Replace("€", "").Replace(" ", "").Trim();
                if (decimal.TryParse(cleanValue, NumberStyles.Currency, new CultureInfo("de-DE"), out decimal result))
                {
                    return result;
                }
            }
            
            return 0m;
        }
    }
}


