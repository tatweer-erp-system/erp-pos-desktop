using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TatweerPOS.Components;

/// <summary>
/// Shimmer/skeleton loading placeholder with an animated gradient
/// that moves left to right continuously.
/// </summary>
public partial class LoadingSkeleton : UserControl
{
    private Storyboard? _shimmerStoryboard;

    public static readonly DependencyProperty SkeletonHeightProperty =
        DependencyProperty.Register(nameof(SkeletonHeight), typeof(double), typeof(LoadingSkeleton),
            new PropertyMetadata(20.0));

    public double SkeletonHeight
    {
        get => (double)GetValue(SkeletonHeightProperty);
        set => SetValue(SkeletonHeightProperty, value);
    }

    public static readonly DependencyProperty SkeletonWidthProperty =
        DependencyProperty.Register(nameof(SkeletonWidth), typeof(double), typeof(LoadingSkeleton),
            new PropertyMetadata(double.NaN));

    public double SkeletonWidth
    {
        get => (double)GetValue(SkeletonWidthProperty);
        set => SetValue(SkeletonWidthProperty, value);
    }

    public static readonly DependencyProperty SkeletonCornerRadiusProperty =
        DependencyProperty.Register(nameof(SkeletonCornerRadius), typeof(CornerRadius), typeof(LoadingSkeleton),
            new PropertyMetadata(new CornerRadius(6)));

    public CornerRadius SkeletonCornerRadius
    {
        get => (CornerRadius)GetValue(SkeletonCornerRadiusProperty);
        set => SetValue(SkeletonCornerRadiusProperty, value);
    }

    public LoadingSkeleton()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        StartShimmer();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        StopShimmer();
    }

    private void StartShimmer()
    {
        _shimmerStoryboard = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };

        // Animate all three gradient stops from left to right
        var anim0 = new DoubleAnimation
        {
            From = -1.0,
            To = 1.0,
            Duration = TimeSpan.FromMilliseconds(1500),
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        };
        Storyboard.SetTarget(anim0, ShimmerOverlay);
        Storyboard.SetTargetProperty(anim0,
            new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[0].(GradientStop.Offset)"));

        var anim1 = new DoubleAnimation
        {
            From = -0.5,
            To = 1.5,
            Duration = TimeSpan.FromMilliseconds(1500),
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        };
        Storyboard.SetTarget(anim1, ShimmerOverlay);
        Storyboard.SetTargetProperty(anim1,
            new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[1].(GradientStop.Offset)"));

        var anim2 = new DoubleAnimation
        {
            From = 0.0,
            To = 2.0,
            Duration = TimeSpan.FromMilliseconds(1500),
            EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
        };
        Storyboard.SetTarget(anim2, ShimmerOverlay);
        Storyboard.SetTargetProperty(anim2,
            new PropertyPath("(Border.Background).(LinearGradientBrush.GradientStops)[2].(GradientStop.Offset)"));

        _shimmerStoryboard.Children.Add(anim0);
        _shimmerStoryboard.Children.Add(anim1);
        _shimmerStoryboard.Children.Add(anim2);

        _shimmerStoryboard.Begin();
    }

    private void StopShimmer()
    {
        _shimmerStoryboard?.Stop();
        _shimmerStoryboard = null;
    }
}
