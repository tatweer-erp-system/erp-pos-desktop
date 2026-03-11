using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TatweerPOS.Converters;

public class StatusToColorConverter : IValueConverter
{
    private static readonly SolidColorBrush GreenBrush = new((Color)ColorConverter.ConvertFromString("#10B981"));
    private static readonly SolidColorBrush AmberBrush = new((Color)ColorConverter.ConvertFromString("#F59E0B"));
    private static readonly SolidColorBrush RedBrush = new((Color)ColorConverter.ConvertFromString("#EF4444"));
    private static readonly SolidColorBrush BlueBrush = new((Color)ColorConverter.ConvertFromString("#0EA5E9"));
    private static readonly SolidColorBrush GrayBrush = new((Color)ColorConverter.ConvertFromString("#94A3B8"));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = value?.ToString() ?? string.Empty;

        return status switch
        {
            "Available" or "Completed" or "Ready" or "Active" or "Synced" => GreenBrush,
            "Occupied" or "Pending" or "New" or "Preparing" or "InProgress" => AmberBrush,
            "Reserved" or "Cancelled" or "Voided" or "Error" => RedBrush,
            "Cleaning" or "Served" or "OnHold" => BlueBrush,
            "Offline" or "Refunded" => GrayBrush,
            _ => GrayBrush
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}
