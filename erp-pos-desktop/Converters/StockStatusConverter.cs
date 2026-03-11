using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TatweerPOS.Converters;

public class StockStatusConverter : IValueConverter
{
    private static readonly SolidColorBrush OutOfStockBrush = new((Color)ColorConverter.ConvertFromString("#EF4444"));
    private static readonly SolidColorBrush LowStockBrush = new((Color)ColorConverter.ConvertFromString("#F59E0B"));
    private static readonly SolidColorBrush InStockBrush = new((Color)ColorConverter.ConvertFromString("#10B981"));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int stock = System.Convert.ToInt32(value);
        int minStock = 5;

        if (parameter is string paramStr && int.TryParse(paramStr, out int parsed))
            minStock = parsed;

        if (stock <= 0)
            return OutOfStockBrush;
        if (stock <= minStock)
            return LowStockBrush;
        return InStockBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}
