using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TatweerPOS.Components;

/// <summary>
/// Custom search input with icon, clear button, and barcode detection.
/// Fires BarcodeDetected when 8+ digits are typed rapidly (within ~100ms per char).
/// </summary>
public partial class SearchBox : UserControl
{
    private readonly Stopwatch _inputStopwatch = new();
    private int _consecutiveDigitCount;
    private string _digitBuffer = string.Empty;

    // Threshold: if chars arrive faster than this, it's likely a barcode scan
    private const int BarcodeCharIntervalMs = 80;
    private const int BarcodeMinDigits = 8;

    // ── Dependency Properties ──

    public static readonly DependencyProperty SearchTextProperty =
        DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(SearchBox),
            new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSearchTextChanged));

    public string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(SearchBox),
            new PropertyMetadata(null, OnPlaceholderChanged));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    private static readonly DependencyPropertyKey IsBarcodeScanModePropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(IsBarcodeScanMode), typeof(bool), typeof(SearchBox),
            new PropertyMetadata(false));

    public static readonly DependencyProperty IsBarcodeScanModeProperty =
        IsBarcodeScanModePropertyKey.DependencyProperty;

    public bool IsBarcodeScanMode
    {
        get => (bool)GetValue(IsBarcodeScanModeProperty);
        private set => SetValue(IsBarcodeScanModePropertyKey, value);
    }

    // ── Events ──

    public event EventHandler<string>? SearchTextChanged;
    public event EventHandler<string>? BarcodeDetected;

    public SearchBox()
    {
        InitializeComponent();
    }

    private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SearchBox box && box.SearchInput != null)
        {
            if (box.SearchInput.Text != (string)e.NewValue)
            {
                box.SearchInput.Text = (string)e.NewValue;
            }
        }
    }

    private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SearchBox box && box.PlaceholderText != null && e.NewValue is string text)
        {
            box.PlaceholderText.Text = text;
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var text = SearchInput.Text;

        // Update dependency property
        SearchText = text;

        // Show/hide clear button and placeholder
        ClearBtn.Visibility = string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
        PlaceholderText.Visibility = string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;

        // Fire event
        SearchTextChanged?.Invoke(this, text);
    }

    private void OnInputPreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Track rapid digit input for barcode detection
        bool isDigit = (e.Key >= Key.D0 && e.Key <= Key.D9) ||
                       (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);

        if (isDigit)
        {
            long elapsed = _inputStopwatch.ElapsedMilliseconds;
            _inputStopwatch.Restart();

            char digit = e.Key switch
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
                _ => '0'
            };

            if (_consecutiveDigitCount == 0 || elapsed < BarcodeCharIntervalMs)
            {
                _consecutiveDigitCount++;
                _digitBuffer += digit;
            }
            else
            {
                // Too slow, reset
                _consecutiveDigitCount = 1;
                _digitBuffer = digit.ToString();
            }

            // Check if we have a barcode
            if (_consecutiveDigitCount >= BarcodeMinDigits)
            {
                IsBarcodeScanMode = true;
                BarcodeIcon.Visibility = Visibility.Visible;
            }
        }
        else if (e.Key == Key.Enter && _consecutiveDigitCount >= BarcodeMinDigits)
        {
            // Enter after rapid digits = barcode scan complete
            BarcodeDetected?.Invoke(this, _digitBuffer);

            // Reset
            _consecutiveDigitCount = 0;
            _digitBuffer = string.Empty;
            IsBarcodeScanMode = false;
            BarcodeIcon.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }
        else
        {
            // Non-digit key resets barcode tracking
            _consecutiveDigitCount = 0;
            _digitBuffer = string.Empty;
            IsBarcodeScanMode = false;
            BarcodeIcon.Visibility = Visibility.Collapsed;
        }
    }

    private void OnInputGotFocus(object sender, RoutedEventArgs e)
    {
        // Animate border to focus color
        var colorAnim = new ColorAnimation
        {
            To = ((SolidColorBrush)FindResource("InputBorderFocus")).Color,
            Duration = TimeSpan.FromMilliseconds(200),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        SearchBorder.BorderBrush = SearchBorder.BorderBrush.Clone() as SolidColorBrush
                                    ?? new SolidColorBrush();
        ((SolidColorBrush)SearchBorder.BorderBrush).BeginAnimation(
            SolidColorBrush.ColorProperty, colorAnim);

        // Highlight search icon
        SearchIcon.Foreground = (Brush)FindResource("PrimaryBrush");
    }

    private void OnInputLostFocus(object sender, RoutedEventArgs e)
    {
        // Animate border back to default color
        var colorAnim = new ColorAnimation
        {
            To = ((SolidColorBrush)FindResource("InputBorder")).Color,
            Duration = TimeSpan.FromMilliseconds(200),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        if (SearchBorder.BorderBrush is SolidColorBrush brush)
        {
            var cloned = brush.Clone();
            SearchBorder.BorderBrush = cloned;
            cloned.BeginAnimation(SolidColorBrush.ColorProperty, colorAnim);
        }

        // Reset search icon
        SearchIcon.Foreground = (Brush)FindResource("TextMuted");

        // Reset barcode tracking
        _consecutiveDigitCount = 0;
        _digitBuffer = string.Empty;
        IsBarcodeScanMode = false;
        BarcodeIcon.Visibility = Visibility.Collapsed;
    }

    private void OnClearClick(object sender, RoutedEventArgs e)
    {
        SearchInput.Text = string.Empty;
        SearchInput.Focus();
    }
}
