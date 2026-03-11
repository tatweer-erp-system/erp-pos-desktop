using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

public partial class EmployeesPage : UserControl
{
    public EmployeesPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AnimationHelper.StaggeredEntrance(new UIElement[]
        {
            HeaderSection,
            FiltersRow,
            ContentArea,
        });
    }

    private void OnAddEmployeeClick(object sender, RoutedEventArgs e)
    {
        // Open add employee dialog
    }

    private void OnRoleFilterChanged(object sender, SelectionChangedEventArgs e)
    {
        // Filter employees by role
    }

    private void OnEmployeeCardClick(object sender, MouseButtonEventArgs e)
    {
        // Show detail panel for selected employee
        if (sender is Border card && card.Tag is string employeeId)
        {
            DetailPanel.Visibility = Visibility.Visible;
            DetailColumn.Width = new GridLength(320);
        }
    }

    private void OnCloseDetailClick(object sender, RoutedEventArgs e)
    {
        DetailPanel.Visibility = Visibility.Collapsed;
        DetailColumn.Width = new GridLength(0);
    }

    private void OnEditEmployeeClick(object sender, RoutedEventArgs e)
    {
        // Open edit employee dialog
    }

    private void OnToggleActiveClick(object sender, RoutedEventArgs e)
    {
        // Toggle employee active status
    }

    private void OnResetPinClick(object sender, RoutedEventArgs e)
    {
        // Reset employee PIN
    }
}
