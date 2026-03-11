using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

/// <summary>
/// Main POS screen — 3-column layout: Categories | Products | Cart.
/// DataContext is set via DataTemplate in MainWindow.
/// </summary>
public partial class POSPage : UserControl
{
    public POSPage()
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
