using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

/// <summary>
/// Tables management page — visual table layout with floor/section filters and detail panel.
/// DataContext is set via DataTemplate in MainWindow.
/// </summary>
public partial class TablesPage : UserControl
{
    public TablesPage()
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
