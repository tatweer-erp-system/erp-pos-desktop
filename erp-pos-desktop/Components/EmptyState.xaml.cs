using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;

namespace TatweerPOS.Components;

/// <summary>
/// Centered empty state for lists with no data.
/// Shows a large faded icon, title, subtitle, and an optional action button.
/// Entrance animation: scale + fade in.
/// </summary>
public partial class EmptyState : UserControl
{
    // ── Dependency Properties ──

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(PackIconKind), typeof(EmptyState),
            new PropertyMetadata(PackIconKind.InboxOutline, OnIconChanged));

    public PackIconKind Icon
    {
        get => (PackIconKind)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(EmptyState),
            new PropertyMetadata(string.Empty, OnTitleChanged));

    public new string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly DependencyProperty SubtitleProperty =
        DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(EmptyState),
            new PropertyMetadata(string.Empty, OnSubtitleChanged));

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public static readonly DependencyProperty ActionTextProperty =
        DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(EmptyState),
            new PropertyMetadata(string.Empty, OnActionTextChanged));

    public string ActionText
    {
        get => (string)GetValue(ActionTextProperty);
        set => SetValue(ActionTextProperty, value);
    }

    public static readonly DependencyProperty ShowActionProperty =
        DependencyProperty.Register(nameof(ShowAction), typeof(bool), typeof(EmptyState),
            new PropertyMetadata(false, OnShowActionChanged));

    public bool ShowAction
    {
        get => (bool)GetValue(ShowActionProperty);
        set => SetValue(ShowActionProperty, value);
    }

    // ── Events ──

    public event EventHandler? ActionClicked;

    public EmptyState()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ApplyProperties();
        PlayEntranceAnimation();
    }

    private void ApplyProperties()
    {
        EmptyIcon.Kind = Icon;
        TitleText.Text = Title;
        SubtitleText.Text = Subtitle;
        ActionBtn.Content = ActionText;
        ActionBtn.Visibility = ShowAction ? Visibility.Visible : Visibility.Collapsed;
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EmptyState c && c.EmptyIcon != null)
            c.EmptyIcon.Kind = (PackIconKind)e.NewValue;
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EmptyState c && c.TitleText != null)
            c.TitleText.Text = (string)e.NewValue;
    }

    private static void OnSubtitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EmptyState c && c.SubtitleText != null)
            c.SubtitleText.Text = (string)e.NewValue;
    }

    private static void OnActionTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EmptyState c && c.ActionBtn != null)
            c.ActionBtn.Content = (string)e.NewValue;
    }

    private static void OnShowActionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is EmptyState c && c.ActionBtn != null)
            c.ActionBtn.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
    }

    private void PlayEntranceAnimation()
    {
        var storyboard = new Storyboard();

        // Scale X: 0.85 -> 1
        var scaleX = new DoubleAnimation
        {
            From = 0.85, To = 1,
            Duration = TimeSpan.FromMilliseconds(400),
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 }
        };
        Storyboard.SetTarget(scaleX, ContentPanel);
        Storyboard.SetTargetName(scaleX, "EntranceScale");
        Storyboard.SetTargetProperty(scaleX,
            new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

        // Scale Y: 0.85 -> 1
        var scaleY = new DoubleAnimation
        {
            From = 0.85, To = 1,
            Duration = TimeSpan.FromMilliseconds(400),
            EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 }
        };
        Storyboard.SetTarget(scaleY, ContentPanel);
        Storyboard.SetTargetProperty(scaleY,
            new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

        // Fade in
        var fade = new DoubleAnimation
        {
            From = 0, To = 1,
            Duration = TimeSpan.FromMilliseconds(350),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        Storyboard.SetTarget(fade, ContentPanel);
        Storyboard.SetTargetProperty(fade, new PropertyPath(OpacityProperty));

        storyboard.Children.Add(scaleX);
        storyboard.Children.Add(scaleY);
        storyboard.Children.Add(fade);

        // Use the parent Grid's transform for scale
        var parentGrid = ContentPanel.Parent as System.Windows.Controls.Grid;
        if (parentGrid != null)
        {
            var gridScaleX = new DoubleAnimation
            {
                From = 0.85, To = 1,
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 }
            };
            Storyboard.SetTarget(gridScaleX, parentGrid);
            Storyboard.SetTargetProperty(gridScaleX,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));

            var gridScaleY = new DoubleAnimation
            {
                From = 0.85, To = 1,
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 }
            };
            Storyboard.SetTarget(gridScaleY, parentGrid);
            Storyboard.SetTargetProperty(gridScaleY,
                new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(gridScaleX);
            storyboard.Children.Add(gridScaleY);
        }

        storyboard.Begin(this);
    }

    private void OnActionClick(object sender, RoutedEventArgs e)
    {
        ActionClicked?.Invoke(this, EventArgs.Empty);
    }
}
