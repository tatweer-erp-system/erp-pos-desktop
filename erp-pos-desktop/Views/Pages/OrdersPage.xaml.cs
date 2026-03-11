using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

/// <summary>
/// Orders management page — order listing with status filters, date range, and detail panel.
/// DataContext is set via DataTemplate in MainWindow.
/// </summary>
public partial class OrdersPage : UserControl
{
    public OrdersPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Staggered entrance animation per CLAUDE.md Rule #4
        AnimationHelper.StaggeredEntrance(new UIElement[]
        {
            HeaderSection,
            FiltersRow,
            ContentArea,
        });
    }
}
