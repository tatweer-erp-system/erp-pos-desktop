using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

public partial class KitchenPage : UserControl
{
    public KitchenPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AnimationHelper.StaggeredEntrance(new UIElement[]
        {
            HeaderSection,
            ContentArea,
        });
    }

    private void OnStartClick(object sender, RoutedEventArgs e)
    {
        // Move ticket from New → Preparing
        if (sender is Button btn && btn.Tag is string ticketId)
        {
            // ViewModel handles status transition
        }
    }

    private void OnDoneClick(object sender, RoutedEventArgs e)
    {
        // Move ticket from Preparing → Ready
        if (sender is Button btn && btn.Tag is string ticketId)
        {
            // ViewModel handles status transition
        }
    }

    private void OnServeClick(object sender, RoutedEventArgs e)
    {
        // Move ticket from Ready → Served
        if (sender is Button btn && btn.Tag is string ticketId)
        {
            // ViewModel handles status transition
        }
    }
}
