using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TatweerPOS.Helpers;
using TatweerPOS.ViewModels;

namespace TatweerPOS.Views;

/// <summary>
/// Main application shell window. Manages title bar, activity tracking,
/// entrance animations, and PIN lock overlay interactions.
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    // ══════════════════════════════════════════════════════════════════
    // Initialization
    // ══════════════════════════════════════════════════════════════════

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Wire up ViewModel events
        ViewModel.PinShakeRequested += OnPinShakeRequested;
        ViewModel.LogoutRequested += OnLogoutRequested;
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;

        // Play entrance animations per CLAUDE.md Rule #4
        PlayEntranceAnimations();
    }

    // ══════════════════════════════════════════════════════════════════
    // Title Bar
    // ══════════════════════════════════════════════════════════════════

    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
            }
            else
            {
                DragMove();
            }
        }
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        => ToggleMaximize();

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => Close();

    private void ToggleMaximize()
    {
        if (WindowState == WindowState.Maximized)
        {
            WindowState = WindowState.Normal;
            MaximizeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
        }
        else
        {
            WindowState = WindowState.Maximized;
            MaximizeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
        }
    }

    // ══════════════════════════════════════════════════════════════════
    // Activity Tracking (resets inactivity timer)
    // ══════════════════════════════════════════════════════════════════

    private void Window_Activity(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.UpdateActivity();
        }
    }

    // ══════════════════════════════════════════════════════════════════
    // PIN Lock Overlay
    // ══════════════════════════════════════════════════════════════════

    private void PinInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        // Auto-submit when 4 digits entered
        if (sender is PasswordBox pb && pb.Password.Length == 4)
        {
            ViewModel.UnlockCommand.Execute(pb.Password);
            pb.Clear();
        }
    }

    private void UnlockButton_Click(object sender, RoutedEventArgs e)
    {
        if (PinInput.Password.Length > 0)
        {
            ViewModel.UnlockCommand.Execute(PinInput.Password);
            PinInput.Clear();
        }
    }

    private void OnPinShakeRequested()
    {
        AnimationHelper.Shake(PinInput);
    }

    // ══════════════════════════════════════════════════════════════════
    // Logout
    // ══════════════════════════════════════════════════════════════════

    private void OnLogoutRequested()
    {
        // Fade out, then open LoginWindow and close this window
        var fadeOut = AnimationHelper.CreateFadeOut(delayMs: 200, durationMs: 300);
        fadeOut.Completed += (_, _) =>
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        };
        BeginAnimation(OpacityProperty, fadeOut);
    }

    // ══════════════════════════════════════════════════════════════════
    // ViewModel Property Changes
    // ══════════════════════════════════════════════════════════════════

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.IsSidebarExpanded))
        {
            AnimateSidebarToggle();
        }

        if (e.PropertyName == nameof(MainViewModel.IsPinLocked))
        {
            if (ViewModel.IsPinLocked)
            {
                // Focus the PIN input when lock overlay appears
                Dispatcher.BeginInvoke(() => PinInput.Focus(),
                    System.Windows.Threading.DispatcherPriority.Input);
            }
        }
    }

    // ══════════════════════════════════════════════════════════════════
    // Sidebar Collapse/Expand Animation
    // ══════════════════════════════════════════════════════════════════

    private void AnimateSidebarToggle()
    {
        double targetWidth = ViewModel.IsSidebarExpanded ? 240 : 60;
        double targetAngle = ViewModel.IsSidebarExpanded ? 0 : 180;

        // Animate sidebar width
        var widthAnim = new GridLengthAnimation
        {
            To = new GridLength(targetWidth),
            Duration = TimeSpan.FromMilliseconds(200),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        SidebarColumn.BeginAnimation(ColumnDefinition.WidthProperty, widthAnim);

        // Animate collapse icon rotation
        var rotateAnim = new DoubleAnimation(targetAngle, TimeSpan.FromMilliseconds(200))
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        CollapseIconRotation.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, rotateAnim);
    }

    // ══════════════════════════════════════════════════════════════════
    // Entrance Animations (CLAUDE.md Rule #4)
    // ══════════════════════════════════════════════════════════════════

    private void PlayEntranceAnimations()
    {
        // Staggered entrance for sidebar nav items
        Dispatcher.BeginInvoke(() =>
        {
            var navItems = new List<UIElement>();
            for (int i = 0; i < NavItemsControl.Items.Count; i++)
            {
                var container = NavItemsControl.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
                if (container != null)
                {
                    navItems.Add(container);
                }
            }

            if (navItems.Count > 0)
            {
                AnimationHelper.StaggeredEntrance(navItems.ToArray(), slideDistance: 20, fadeDurationMs: 300, staggerMs: 60);
            }
        }, System.Windows.Threading.DispatcherPriority.Loaded);

        // Fade in the content area
        ContentArea.Opacity = 0;
        var contentFade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(400))
        {
            BeginTime = TimeSpan.FromMilliseconds(200),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        ContentArea.BeginAnimation(OpacityProperty, contentFade);

        // Fade in the top bar page title
        PageTitleText.Opacity = 0;
        PageTitleText.RenderTransform = new System.Windows.Media.TranslateTransform(0, 10);
        var titleFade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300))
        {
            BeginTime = TimeSpan.FromMilliseconds(100),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        var titleSlide = new DoubleAnimation(10, 0, TimeSpan.FromMilliseconds(300))
        {
            BeginTime = TimeSpan.FromMilliseconds(100),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        PageTitleText.BeginAnimation(OpacityProperty, titleFade);
        PageTitleText.RenderTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, titleSlide);
    }
}

// ══════════════════════════════════════════════════════════════════════
// GridLength Animation Support
// ══════════════════════════════════════════════════════════════════════

/// <summary>
/// Custom animation for animating GridLength values (used for sidebar collapse).
/// </summary>
public class GridLengthAnimation : AnimationTimeline
{
    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register(nameof(To), typeof(GridLength), typeof(GridLengthAnimation));

    public GridLength To
    {
        get => (GridLength)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public IEasingFunction? EasingFunction { get; set; }

    public override Type TargetPropertyType => typeof(GridLength);

    protected override Freezable CreateInstanceCore() => new GridLengthAnimation();

    public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        double progress = animationClock.CurrentProgress ?? 0;

        if (EasingFunction != null)
        {
            progress = EasingFunction.Ease(progress);
        }

        var from = (GridLength)defaultOriginValue;
        double fromVal = from.Value;
        double toVal = To.Value;

        double current = fromVal + (toVal - fromVal) * progress;
        return new GridLength(current, GridUnitType.Pixel);
    }
}
