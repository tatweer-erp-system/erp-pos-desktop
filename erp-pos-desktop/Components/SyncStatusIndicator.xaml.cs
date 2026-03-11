using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;

namespace TatweerPOS.Components;

/// <summary>
/// Small circular indicator showing sync status: Synced, Pending, Error, Offline.
/// Pending state includes a subtle pulse animation.
/// </summary>
public partial class SyncStatusIndicator : UserControl
{
    private Storyboard? _pulseStoryboard;

    public static readonly DependencyProperty StatusProperty =
        DependencyProperty.Register(
            nameof(Status),
            typeof(string),
            typeof(SyncStatusIndicator),
            new PropertyMetadata("Synced", OnStatusChanged));

    public string Status
    {
        get => (string)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public SyncStatusIndicator()
    {
        InitializeComponent();
        Loaded += (_, _) => ApplyStatus();
    }

    private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SyncStatusIndicator control)
        {
            control.ApplyStatus();
        }
    }

    private void ApplyStatus()
    {
        if (StatusDot == null) return;

        // Stop any existing pulse
        StopPulse();

        Brush dotBrush;
        Brush ringBrush;
        PackIconKind iconKind;
        string textKey;

        switch (Status)
        {
            case "Synced":
                dotBrush = (Brush)FindResource("SuccessBrush");
                ringBrush = dotBrush;
                iconKind = PackIconKind.CloudCheck;
                textKey = "str_Synced";
                break;

            case "Pending":
                dotBrush = (Brush)FindResource("WarningBrush");
                ringBrush = dotBrush;
                iconKind = PackIconKind.CloudSync;
                textKey = "str_SyncPending";
                StartPulse();
                break;

            case "Error":
                dotBrush = (Brush)FindResource("ErrorBrush");
                ringBrush = dotBrush;
                iconKind = PackIconKind.CloudAlert;
                textKey = "str_SyncError";
                break;

            case "Offline":
            default:
                dotBrush = (Brush)FindResource("TextMuted");
                ringBrush = dotBrush;
                iconKind = PackIconKind.CloudOff;
                textKey = "str_Offline";
                break;
        }

        StatusDot.Fill = dotBrush;
        PulseRing.Fill = ringBrush;
        StatusIcon.Kind = iconKind;
        StatusIcon.Foreground = dotBrush;

        var text = TryFindResource(textKey) as string ?? Status;
        StatusText.Text = text;
        TooltipText.Text = text;
    }

    private void StartPulse()
    {
        _pulseStoryboard = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };

        var opacityAnim = new DoubleAnimation
        {
            From = 0.6,
            To = 0,
            Duration = TimeSpan.FromMilliseconds(1200),
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(opacityAnim, PulseRing);
        Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(OpacityProperty));

        var scaleXAnim = new DoubleAnimation
        {
            From = 1,
            To = 2.2,
            Duration = TimeSpan.FromMilliseconds(1200),
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(scaleXAnim, PulseRing);
        Storyboard.SetTargetProperty(scaleXAnim,
            new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));

        var scaleYAnim = new DoubleAnimation
        {
            From = 1,
            To = 2.2,
            Duration = TimeSpan.FromMilliseconds(1200),
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(scaleYAnim, PulseRing);
        Storyboard.SetTargetProperty(scaleYAnim,
            new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

        _pulseStoryboard.Children.Add(opacityAnim);
        _pulseStoryboard.Children.Add(scaleXAnim);
        _pulseStoryboard.Children.Add(scaleYAnim);

        _pulseStoryboard.Begin();
    }

    private void StopPulse()
    {
        _pulseStoryboard?.Stop();
        _pulseStoryboard = null;
        PulseRing.Opacity = 0;
    }
}
