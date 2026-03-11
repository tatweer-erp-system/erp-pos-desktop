using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TatweerPOS.Models.Enums;

namespace TatweerPOS.Converters;

public class RoleToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not EmployeeRole currentRole || parameter is not string requiredRoles)
            return Visibility.Collapsed;

        string currentRoleStr = currentRole.ToString();
        string[] roles = requiredRoles.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (string role in roles)
        {
            if (role.Equals(currentRoleStr, StringComparison.OrdinalIgnoreCase))
                return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => DependencyProperty.UnsetValue;
}
