using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TatweerPOS.Components;

/// <summary>
/// Toggle control for switching between EN and AR languages.
/// Active language gets PrimaryBrush background, inactive gets transparent.
/// </summary>
public partial class LanguageToggle : UserControl
{
    public static readonly DependencyProperty SelectedLanguageProperty =
        DependencyProperty.Register(
            nameof(SelectedLanguage),
            typeof(string),
            typeof(LanguageToggle),
            new FrameworkPropertyMetadata("EN",
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedLanguageChanged));

    public string SelectedLanguage
    {
        get => (string)GetValue(SelectedLanguageProperty);
        set => SetValue(SelectedLanguageProperty, value);
    }

    public event EventHandler<string>? LanguageChanged;

    public LanguageToggle()
    {
        InitializeComponent();
        Loaded += (_, _) => UpdateButtonStates();
    }

    private static void OnSelectedLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LanguageToggle control)
        {
            control.UpdateButtonStates();
        }
    }

    private void OnEnClick(object sender, RoutedEventArgs e)
    {
        if (SelectedLanguage == "EN") return;
        SelectedLanguage = "EN";
        LanguageChanged?.Invoke(this, "EN");
    }

    private void OnArClick(object sender, RoutedEventArgs e)
    {
        if (SelectedLanguage == "AR") return;
        SelectedLanguage = "AR";
        LanguageChanged?.Invoke(this, "AR");
    }

    private void UpdateButtonStates()
    {
        if (EnBtn == null || ArBtn == null) return;

        var primaryBrush = TryFindResource("PrimaryBrush") as Brush ?? Brushes.Indigo;
        var transparentBrush = Brushes.Transparent;
        var whiteBrush = Brushes.White;
        var mutedBrush = TryFindResource("TextMuted") as Brush ?? Brushes.Gray;

        if (SelectedLanguage == "EN")
        {
            EnBtn.Background = primaryBrush;
            EnBtn.Foreground = whiteBrush;
            ArBtn.Background = transparentBrush;
            ArBtn.Foreground = mutedBrush;
        }
        else
        {
            ArBtn.Background = primaryBrush;
            ArBtn.Foreground = whiteBrush;
            EnBtn.Background = transparentBrush;
            EnBtn.Foreground = mutedBrush;
        }
    }
}
