using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TatweerPOS.Resources.Themes;

namespace TatweerPOS.Components;

/// <summary>
/// Toggle button that switches between Dark and Light themes
/// with a smooth rotation animation.
/// </summary>
public partial class ThemeToggleButton : UserControl
{
    public static readonly DependencyProperty IsDarkThemeProperty =
        DependencyProperty.Register(
            nameof(IsDarkTheme),
            typeof(bool),
            typeof(ThemeToggleButton),
            new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIsDarkThemeChanged));

    public bool IsDarkTheme
    {
        get => (bool)GetValue(IsDarkThemeProperty);
        set => SetValue(IsDarkThemeProperty, value);
    }

    public ThemeToggleButton()
    {
        InitializeComponent();
        UpdateIconVisibility();
    }

    private static void OnIsDarkThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ThemeToggleButton control)
        {
            control.UpdateIconVisibility();
        }
    }

    private void OnToggleClick(object sender, RoutedEventArgs e)
    {
        // Animate rotation (150ms)
        var rotateAnimation = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = TimeSpan.FromMilliseconds(150),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        IconRotation.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, rotateAnimation);

        // Toggle the theme
        ThemeManager.Toggle();
        IsDarkTheme = ThemeManager.CurrentTheme == Models.Enums.AppTheme.Dark;
    }

    private void UpdateIconVisibility()
    {
        if (SunIcon == null || MoonIcon == null) return;

        if (IsDarkTheme)
        {
            SunIcon.Visibility = Visibility.Visible;
            MoonIcon.Visibility = Visibility.Collapsed;
        }
        else
        {
            SunIcon.Visibility = Visibility.Collapsed;
            MoonIcon.Visibility = Visibility.Visible;
        }
    }
}
