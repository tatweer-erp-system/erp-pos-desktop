using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

public partial class SuppliersPage : UserControl
{
    public SuppliersPage()
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

    private void OnAddSupplierClick(object sender, RoutedEventArgs e)
    {
        // Open add supplier dialog
    }

    private void OnSupplierSelected(object sender, SelectionChangedEventArgs e)
    {
        // Show detail panel for selected supplier
        if (SuppliersGrid.SelectedItem != null)
        {
            DetailPanel.Visibility = Visibility.Visible;
            DetailColumn.Width = new GridLength(320);
        }
    }

    private void OnCloseDetailClick(object sender, RoutedEventArgs e)
    {
        DetailPanel.Visibility = Visibility.Collapsed;
        DetailColumn.Width = new GridLength(0);
        SuppliersGrid.SelectedItem = null;
    }

    private void OnEditSupplierClick(object sender, RoutedEventArgs e)
    {
        // Open edit supplier dialog
    }

    private void OnToggleActiveClick(object sender, RoutedEventArgs e)
    {
        // Toggle supplier active status
    }
}
