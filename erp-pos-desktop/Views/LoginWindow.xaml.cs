using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TatweerPOS.Helpers;
using TatweerPOS.ViewModels;

namespace TatweerPOS.Views;

public partial class LoginWindow : Window
{
    private LoginViewModel ViewModel => (LoginViewModel)DataContext;

    public LoginWindow()
    {
        InitializeComponent();
        Loaded += LoginWindow_Loaded;
    }

    private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Wire up ViewModel events for view-level animations
        ViewModel.ShakeRequested += OnShakeRequested;
        ViewModel.LoginSucceeded += OnLoginSucceeded;
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;

        // Generate decorative elements
        GenerateDotGrid();

        // Start all animations per CLAUDE.md rules
        StartOrbAnimations();
        AnimationHelper.ElasticScaleIn(this, LogoScale, delayMs: 300);
        PlayEntranceAnimations();
    }

    // ===== Title Bar Drag =====
    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        => WindowState = WindowState.Minimized;

    private void CloseButton_Click(object sender, RoutedEventArgs e)
        => Close();

    // ===== Keyboard shortcuts =====
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            WindowState = WindowState.Minimized;
        else if (e.Key == Key.Enter && !ViewModel.IsLoading)
            ViewModel.LoginCommand.Execute(null);
    }

    // ===== Language Toggles (EN/AR with RTL support per CLAUDE.md) =====
    private void EnToggle_Click(object sender, RoutedEventArgs e)
    {
        EnToggle.IsChecked = true;
        ArToggle.IsChecked = false;
        ViewModel.SelectedLanguage = "EN";
    }

    private void ArToggle_Click(object sender, RoutedEventArgs e)
    {
        ArToggle.IsChecked = true;
        EnToggle.IsChecked = false;
        ViewModel.SelectedLanguage = "AR";
    }

    // ===== Password Binding (WPF PasswordBox doesn't support data binding) =====
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }

    // ===== Spinner for loading state =====
    private Storyboard? _spinnerStoryboard;

    private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.IsLoading))
        {
            if (ViewModel.IsLoading)
            {
                _spinnerStoryboard = AnimationHelper.CreateSpinnerStoryboard("SpinnerRotate");
                _spinnerStoryboard.Begin(this, true);
            }
            else
            {
                _spinnerStoryboard?.Stop(this);
                _spinnerStoryboard = null;
            }
        }
    }

    // ===== Shake Animation (CLAUDE.md Rule #5: error feedback) =====
    private void OnShakeRequested()
    {
        AnimationHelper.Shake(EmailTextBox);
        AnimationHelper.Shake(PasswordField);
        AnimationHelper.Shake(PasswordVisibleField);
        AnimationHelper.Shake(ErrorBanner);
    }

    // ===== Success Animation =====
    private async void OnLoginSucceeded()
    {
        // Authenticate via AuthService to populate the session for MainViewModel
        var authService = (TatweerPOS.Services.AuthService)TatweerPOS.App.Services.GetService(typeof(TatweerPOS.Services.AuthService))!;
        var branchId = ViewModel.SelectedBranch?.Id ?? "";
        await authService.LoginAsync(ViewModel.Email, ViewModel.Password, branchId);

        var fadeOut = AnimationHelper.CreateFadeOut(delayMs: 600, durationMs: 400);
        fadeOut.Completed += (_, _) =>
        {
            // Open MainWindow via DI and close LoginWindow
            var mainViewModel = TatweerPOS.App.Services.GetService(typeof(TatweerPOS.ViewModels.MainViewModel));
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
            Close();
        };
        BeginAnimation(OpacityProperty, fadeOut);
    }

    // ===== Staggered Entrance (CLAUDE.md Rule #4: 60-80ms per element, CubicEaseOut) =====
    private void PlayEntranceAnimations()
    {
        var elements = new UIElement[FormGrid.Children.Count];
        for (int i = 0; i < FormGrid.Children.Count; i++)
            elements[i] = FormGrid.Children[i];

        AnimationHelper.StaggeredEntrance(elements, slideDistance: 30, fadeDurationMs: 400, staggerMs: 70);
    }

    // ===== Orb Animations (SineEase, organic movement per CLAUDE.md) =====
    private void StartOrbAnimations()
    {
        AnimationHelper.FloatAnimation(Orb1Transform, vertical: true, distance: -20, durationSec: 6);
        AnimationHelper.FloatAnimation(Orb2Transform, vertical: false, distance: 15, durationSec: 8);
        AnimationHelper.ScalePulse(Orb3Transform, maxScale: 1.15, durationSec: 5);
    }

    // ===== Dot Grid Pattern =====
    private void GenerateDotGrid()
    {
        DotCanvas.Children.Clear();
        double spacing = 40;
        double width = ActualWidth > 420 ? ActualWidth - 420 : 680;
        double height = ActualHeight > 40 ? ActualHeight - 40 : 640;

        for (double x = 20; x < width; x += spacing)
        {
            for (double y = 20; y < height; y += spacing)
            {
                var dot = new Ellipse
                {
                    Width = 2,
                    Height = 2,
                    Fill = new SolidColorBrush(Color.FromArgb(0x10, 0x63, 0x66, 0xF1)),
                };
                Canvas.SetLeft(dot, x);
                Canvas.SetTop(dot, y);
                DotCanvas.Children.Add(dot);
            }
        }
    }
}
