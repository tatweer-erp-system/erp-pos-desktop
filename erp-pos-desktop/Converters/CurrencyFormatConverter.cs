using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TatweerPOS.Converters;

public class CurrencyFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        decimal amount = System.Convert.ToDecimal(value);

        if (parameter is string currency && currency.Equals("SAR", StringComparison.OrdinalIgnoreCase))
            return $"{amount:F2} ر.س";

        return $"${amount:F2}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}
