using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
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

    private void OnThemeToggleClick(object sender, RoutedEventArgs e)
    {
        // Toggle between Dark and Light theme
    }

    private void OnLanguageToggleClick(object sender, RoutedEventArgs e)
    {
        // Toggle EN/AR — ensure mutual exclusion
        if (sender == LangEnBtn)
        {
            LangEnBtn.IsChecked = true;
            LangArBtn.IsChecked = false;
        }
        else
        {
            LangArBtn.IsChecked = true;
            LangEnBtn.IsChecked = false;
        }
    }

    private void OnSavePosSettingsClick(object sender, RoutedEventArgs e)
    {
        // Save receipt header/footer, timeout, printer settings
    }

    private void OnSyncNowClick(object sender, RoutedEventArgs e)
    {
        // Trigger manual sync
    }

    private void OnValidateLicenseClick(object sender, RoutedEventArgs e)
    {
        // Validate license key against fingerprint
    }

    private void OnClearCacheClick(object sender, RoutedEventArgs e)
    {
        // Clear application cache
    }

    private void OnBackupClick(object sender, RoutedEventArgs e)
    {
        // Create data backup
    }

    private void OnRestoreClick(object sender, RoutedEventArgs e)
    {
        // Restore from backup
    }
}
