using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace TatweerPOS.Components;

/// <summary>
/// Full-screen PIN lock overlay for session security.
/// Supports 4-digit PIN entry, shake animation on error,
/// and lockout after 3 failed attempts with a 30-second countdown.
/// </summary>
public partial class PinLockOverlay : UserControl
{
    private readonly char[] _pinDigits = new char[4];
    private int _currentIndex;
    private int _failedAttempts;
    private bool _isLockedOut;
    private DispatcherTimer? _countdownTimer;
    private int _countdownSeconds;

    // ── Dependency Properties ──

    public static readonly DependencyProperty UserNameProperty =
        DependencyProperty.Register(nameof(UserName), typeof(string), typeof(PinLockOverlay),
            new PropertyMetadata(string.Empty));

    public string UserName
    {
        get => (string)GetValue(UserNameProperty);
        set => SetValue(UserNameProperty, value);
    }

    public static readonly DependencyProperty UserRoleProperty =
        DependencyProperty.Register(nameof(UserRole), typeof(string), typeof(PinLockOverlay),
            new PropertyMetadata(string.Empty));

    public string UserRole
    {
        get => (string)GetValue(UserRoleProperty);
        set => SetValue(UserRoleProperty, value);
    }

    public static readonly DependencyProperty UserInitialsProperty =
        DependencyProperty.Register(nameof(UserInitials), typeof(string), typeof(PinLockOverlay),
            new PropertyMetadata(string.Empty));

    public string UserInitials
    {
        get => (string)GetValue(UserInitialsProperty);
        set => SetValue(UserInitialsProperty, value);
    }

    // ── Events ──

    public event EventHandler<string>? PinSubmitted;
    public event EventHandler? UnlockRequested;

    public PinLockOverlay()
    {
        InitializeComponent();
        Loaded += (_, _) => Focus();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_isLockedOut) return;

        // Handle numeric keys (both main keyboard and numpad)
        char? digit = e.Key switch
        {
            Key.D0 or Key.NumPad0 => '0',
            Key.D1 or Key.NumPad1 => '1',
            Key.D2 or Key.NumPad2 => '2',
            Key.D3 or Key.NumPad3 => '3',
            Key.D4 or Key.NumPad4 => '4',
            Key.D5 or Key.NumPad5 => '5',
            Key.D6 or Key.NumPad6 => '6',
            Key.D7 or Key.NumPad7 => '7',
            Key.D8 or Key.NumPad8 => '8',
            Key.D9 or Key.NumPad9 => '9',
            _ => null
        };

        if (digit.HasValue && _currentIndex < 4)
        {
            _pinDigits[_currentIndex] = digit.Value;
            SetDigitDisplay(_currentIndex, "\u2022"); // bullet character
            HighlightBox(_currentIndex, true);
            _currentIndex++;

            if (_currentIndex == 4)
            {
                var pin = new string(_pinDigits);
                PinSubmitted?.Invoke(this, pin);
            }

            e.Handled = true;
            return;
        }

        // Backspace to delete last digit
        if (e.Key == Key.Back && _currentIndex > 0)
        {
            _currentIndex--;
            _pinDigits[_currentIndex] = '\0';
            SetDigitDisplay(_currentIndex, "");
            HighlightBox(_currentIndex, false);
            HideError();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Call this from the parent when the entered PIN is wrong.
    /// Triggers shake animation and increments the fail counter.
    /// </summary>
    public void ShowWrongPin()
    {
        _failedAttempts++;

        // Show error message
        var wrongPinText = TryFindResource("str_WrongPin") as string ?? "Wrong PIN";
        ErrorMessage.Text = wrongPinText;
        ErrorMessage.Visibility = Visibility.Visible;

        // Shake animation on the PIN boxes
        PlayShakeAnimation();

        // Reset PIN entry
        ResetPinEntry();

        // Check for lockout
        if (_failedAttempts >= 3)
        {
            LockOut();
        }
    }

    /// <summary>
    /// Resets the overlay to its initial state.
    /// </summary>
    public void Reset()
    {
        _failedAttempts = 0;
        _isLockedOut = false;
        _countdownTimer?.Stop();
        ResetPinEntry();
        HideError();
        LockedPanel.Visibility = Visibility.Collapsed;
        Focus();
    }

    private void ResetPinEntry()
    {
        _currentIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            _pinDigits[i] = '\0';
            SetDigitDisplay(i, "");
            HighlightBox(i, false);
        }
    }

    private void SetDigitDisplay(int index, string text)
    {
        var digitBlock = index switch
        {
            0 => PinDigit0,
            1 => PinDigit1,
            2 => PinDigit2,
            3 => PinDigit3,
            _ => null
        };

        if (digitBlock != null)
            digitBlock.Text = text;
    }

    private void HighlightBox(int index, bool filled)
    {
        var box = index switch
        {
            0 => PinBox0,
            1 => PinBox1,
            2 => PinBox2,
            3 => PinBox3,
            _ => null
        };

        if (box != null)
        {
            box.BorderBrush = filled
                ? (System.Windows.Media.Brush)FindResource("PrimaryBrush")
                : (System.Windows.Media.Brush)FindResource("InputBorder");
        }
    }

    private void HideError()
    {
        ErrorMessage.Visibility = Visibility.Collapsed;
    }

    private void PlayShakeAnimation()
    {
        var shake = new DoubleAnimationUsingKeyFrames
        {
            Duration = TimeSpan.FromMilliseconds(400)
        };

        shake.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
        shake.KeyFrames.Add(new LinearDoubleKeyFrame(-12, KeyTime.FromPercent(0.1)));
        shake.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromPercent(0.25)));
        shake.KeyFrames.Add(new LinearDoubleKeyFrame(-8, KeyTime.FromPercent(0.4)));
        shake.KeyFrames.Add(new LinearDoubleKeyFrame(6, KeyTime.FromPercent(0.55)));
        shake.KeyFrames.Add(new LinearDoubleKeyFrame(-3, KeyTime.FromPercent(0.7)));
        shake.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1.0)));

        PinShakeTransform.BeginAnimation(
            System.Windows.Media.TranslateTransform.XProperty, shake);
    }

    private void LockOut()
    {
        _isLockedOut = true;
        _countdownSeconds = 30;
        LockedPanel.Visibility = Visibility.Visible;
        ErrorMessage.Visibility = Visibility.Collapsed;
        UpdateCountdownText();

        _countdownTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _countdownTimer.Tick += (_, _) =>
        {
            _countdownSeconds--;
            UpdateCountdownText();

            if (_countdownSeconds <= 0)
            {
                _countdownTimer.Stop();
                _isLockedOut = false;
                _failedAttempts = 0;
                LockedPanel.Visibility = Visibility.Collapsed;
                Focus();
            }
        };
        _countdownTimer.Start();
    }

    private void UpdateCountdownText()
    {
        var lockedFor = TryFindResource("str_LockedFor") as string ?? "Locked for";
        var seconds = TryFindResource("str_Seconds") as string ?? "seconds";
        CountdownText.Text = $"{lockedFor} {_countdownSeconds} {seconds}";
    }

    private void OnSwitchUserClick(object sender, RoutedEventArgs e)
    {
        UnlockRequested?.Invoke(this, EventArgs.Empty);
    }
}
