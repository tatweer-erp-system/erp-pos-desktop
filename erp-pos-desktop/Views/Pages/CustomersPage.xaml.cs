using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

/// <summary>
/// Customers management page — customer cards with filters, tier badges, and detail panel.
/// DataContext is set via DataTemplate in MainWindow.
/// </summary>
public partial class CustomersPage : UserControl
{
    public CustomersPage()
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
