using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Data;
using TatweerPOS.Models;

namespace TatweerPOS.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    // Form Fields
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private bool rememberMe;
    [ObservableProperty] private Branch? selectedBranch;
    [ObservableProperty] private string selectedLanguage = "EN";

    // UI State
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool isPasswordVisible;
    [ObservableProperty] private string errorMessage = "";
    [ObservableProperty] private bool hasError;
    [ObservableProperty] private bool isEmailValid;
    [ObservableProperty] private bool isSuccess;
    [ObservableProperty] private bool isLocked;
    [ObservableProperty] private int lockCountdown;
    [ObservableProperty] private bool showForgotDialog;

    // Data
    public List<Branch> Branches { get; } = MockData.Branches;

    // Events for view-level animations
    public event Action? ShakeRequested;
    public event Action? LoginSucceeded;

    private int _failedAttempts;
    private DispatcherTimer? _lockTimer;

    partial void OnEmailChanged(string value)
    {
        IsEmailValid = Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (HasError) HasError = false;
    }

    partial void OnPasswordChanged(string value)
    {
        if (HasError) HasError = false;
    }

    partial void OnSelectedLanguageChanged(string value)
    {
        if (Application.Current?.MainWindow != null)
        {
            Application.Current.MainWindow.FlowDirection =
                value == "AR" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }
    }

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    [RelayCommand]
    private void ForgotPassword()
    {
        ShowForgotDialog = true;
    }

    [RelayCommand]
    private void CloseForgotDialog()
    {
        ShowForgotDialog = false;
    }

    [RelayCommand]
    private void SwitchLanguage(string lang)
    {
        SelectedLanguage = lang;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsLoading || IsLocked) return;

        // Validation
        if (SelectedBranch is null)
        {
            ErrorMessage = "Please select a branch";
            HasError = true;
            ShakeRequested?.Invoke();
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Please enter your email address";
            HasError = true;
            ShakeRequested?.Invoke();
            return;
        }

        if (!IsEmailValid)
        {
            ErrorMessage = "Please enter a valid email address";
            HasError = true;
            ShakeRequested?.Invoke();
            return;
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
        {
            ErrorMessage = "Password must be at least 6 characters";
            HasError = true;
            ShakeRequested?.Invoke();
            return;
        }

        // Loading state
        IsLoading = true;
        HasError = false;

        await Task.Delay(1500); // Simulate API call

        // Mock Auth — per CLAUDE.md credentials
        bool authenticated = (Email.ToLower(), Password) switch
        {
            ("admin@tatweer.com", "Admin@123") => true,
            ("manager@tatweer.com", "Manager@123") => true,
            ("supervisor@tatweer.com", "Super@123") => true,
            ("cashier@tatweer.com", "Cash@123") => true,
            ("pharma@tatweer.com", "Pharma@123") => true,
            _ => false
        };

        if (authenticated)
        {
            IsLoading = false;
            IsSuccess = true;
            LoginSucceeded?.Invoke();
        }
        else
        {
            IsLoading = false;
            _failedAttempts++;

            if (_failedAttempts >= 3)
            {
                IsLocked = true;
                LockCountdown = 30;
                ErrorMessage = $"Too many attempts. Try again in {LockCountdown} seconds";
                HasError = true;
                ShakeRequested?.Invoke();

                _lockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                _lockTimer.Tick += (_, _) =>
                {
                    LockCountdown--;
                    ErrorMessage = $"Too many attempts. Try again in {LockCountdown} seconds";
                    if (LockCountdown <= 0)
                    {
                        _lockTimer.Stop();
                        IsLocked = false;
                        _failedAttempts = 0;
                        HasError = false;
                    }
                };
                _lockTimer.Start();
            }
            else
            {
                ErrorMessage = $"Invalid email or password ({3 - _failedAttempts} attempts remaining)";
                HasError = true;
                ShakeRequested?.Invoke();
            }
        }
    }
}
