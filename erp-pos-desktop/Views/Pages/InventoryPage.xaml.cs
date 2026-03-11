using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

/// <summary>
/// Inventory management page — product listing with filters, stats, and detail panel.
/// DataContext is set via DataTemplate in MainWindow.
/// </summary>
public partial class InventoryPage : UserControl
{
    public InventoryPage()
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
