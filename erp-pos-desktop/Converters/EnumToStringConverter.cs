using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TatweerPOS.Converters;

public partial class EnumToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            return string.Empty;

        string enumString = value.ToString()!;
        return InsertSpacesRegex().Replace(enumString, " $1");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;

    [GeneratedRegex(@"(?<!^)([A-Z])")]
    private static partial Regex InsertSpacesRegex();
}
