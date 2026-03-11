using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TatweerPOS.Models.Enums;

namespace TatweerPOS.Converters;

public class ThemeToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppTheme theme)
        {
            return theme switch
            {
                AppTheme.Dark => "WeatherNight",
                AppTheme.Light => "WhiteBalanceSunny",
                _ => "WhiteBalanceSunny"
            };
        }

        return "WhiteBalanceSunny";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}
