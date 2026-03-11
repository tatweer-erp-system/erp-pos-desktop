namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;
using TatweerPOS.Resources.Themes;
using TatweerPOS.Helpers;

public partial class SettingsViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    // ─── Observable Properties ─────────────────────────────────────

    // Theme & Language
    [ObservableProperty] private AppTheme currentTheme = ThemeManager.CurrentTheme;
    [ObservableProperty] private string currentLanguage = "EN";
    [ObservableProperty] private BusinessMode currentBusinessMode = BusinessMode.Restaurant;

    // Static lists
    public List<BusinessMode> BusinessModes { get; } = new()
    {
        BusinessMode.Retail,
        BusinessMode.Restaurant,
        BusinessMode.Pharmacy,
        BusinessMode.Supermarket
    };

    // Branch info (from session)
    [ObservableProperty] private string branchName = "";
    [ObservableProperty] private string branchAddress = "";
    [ObservableProperty] private string branchPhone = "";
    [ObservableProperty] private string branchTaxNumber = "";

    // Receipt
    [ObservableProperty] private string receiptHeader = "Tatweer POS";
    [ObservableProperty] private string receiptFooter = "Thank you for your visit!";

    // Security
    [ObservableProperty] private int autoLockMinutes = 5;

    // Printing
    [ObservableProperty] private string defaultPrinter = "Default Printer";
    [ObservableProperty] private List<string> availablePrinters = new()
    {
        "Default Printer",
        "Receipt Printer (USB)",
        "Kitchen Printer (Network)",
        "Label Printer",
        "Microsoft Print to PDF"
    };

    // Sync
    [ObservableProperty] private bool isSyncEnabled = true;
    [ObservableProperty] private int syncIntervalMinutes = 5;

    // System
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string appVersion = "1.0.0";
    [ObservableProperty] private string deviceFingerprint = "";
    [ObservableProperty] private string licenseKey = "";
    [ObservableProperty] private string licenseStatus = "Not Validated";

    // ─── Constructor ───────────────────────────────────────────────

    public SettingsViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        LoadSettings();
    }

    // ─── Commands ──────────────────────────────────────────────────

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to change settings.");
            return;
        }

        IsLoading = true;

        try
        {
            await Task.Delay(500); // Simulate save
            LogAudit(AuditAction.SettingsChange,
                $"Settings saved: Theme={CurrentTheme}, Language={CurrentLanguage}, BusinessMode={CurrentBusinessMode}, AutoLock={AutoLockMinutes}min");
            _notificationService.ShowSuccess("Settings saved successfully.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeManager.Toggle();
        CurrentTheme = ThemeManager.CurrentTheme;
        LogAudit(AuditAction.SettingsChange, $"Theme changed to {CurrentTheme}");
        _notificationService.ShowInfo($"Theme switched to {CurrentTheme}.");
    }

    [RelayCommand]
    private void SwitchLanguage(string language)
    {
        if (language != "EN" && language != "AR") return;

        CurrentLanguage = language;
        LogAudit(AuditAction.SettingsChange, $"Language changed to {CurrentLanguage}");
        _notificationService.ShowInfo($"Language switched to {(CurrentLanguage == "AR" ? "Arabic" : "English")}.");
    }

    [RelayCommand]
    private void ChangeBusinessMode(BusinessMode mode)
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to change business mode.");
            return;
        }

        CurrentBusinessMode = mode;
        LogAudit(AuditAction.SettingsChange, $"Business mode changed to {mode}");
        _notificationService.ShowSuccess($"Business mode changed to {mode}.");
    }

    [RelayCommand]
    private async Task TestPrinterAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(1000); // Simulate print test page

            _notificationService.ShowSuccess($"Test page sent to {DefaultPrinter}.");
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Printer test failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ClearCacheAsync()
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to clear cache.");
            return;
        }

        IsLoading = true;

        try
        {
            await Task.Delay(300); // Simulate cache clear
            CacheManager.Clear();
            LogAudit(AuditAction.CacheClear, "Application cache cleared");
            _notificationService.ShowSuccess("Cache cleared successfully.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task BackupDataAsync()
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to create backups.");
            return;
        }

        IsLoading = true;

        try
        {
            await Task.Delay(1500); // Simulate backup
            var backupFile = $"tatweer_backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            LogAudit(AuditAction.BackupCreate, $"Backup created: {backupFile}");
            _notificationService.ShowSuccess($"Backup created successfully: {backupFile}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RestoreBackupAsync()
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to restore backups.");
            return;
        }

        IsLoading = true;

        try
        {
            await Task.Delay(2000); // Simulate restore
            LogAudit(AuditAction.BackupRestore, "Data restored from backup");
            _notificationService.ShowSuccess("Data restored from backup successfully.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task ValidateLicenseAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(800); // Simulate license validation

            var fingerprint = HardwareFingerprint.Generate();
            var result = LicenseManager.Validate(LicenseKey, fingerprint);

            if (result.IsValid)
            {
                LicenseStatus = $"Valid (expires {result.ExpiresAt:yyyy-MM-dd})";
                LogAudit(AuditAction.LicenseValidation, $"License validated successfully: {LicenseKey}");
                _notificationService.ShowSuccess("License is valid.");
            }
            else
            {
                LicenseStatus = $"Invalid: {result.ErrorMessage}";
                LogAudit(AuditAction.LicenseFailure, $"License validation failed: {result.ErrorMessage}");
                _notificationService.ShowError($"License validation failed: {result.ErrorMessage}");
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(300);
            LoadSettings();
            _notificationService.ShowInfo("Settings refreshed.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ─── Private Helpers ───────────────────────────────────────────

    private void LoadSettings()
    {
        // Load branch info from session
        var session = _authService.CurrentSession;
        if (session != null)
        {
            var branch = MockData.Branches.FirstOrDefault(b => b.Id == session.BranchId);
            if (branch != null)
            {
                BranchName = branch.Name;
                BranchAddress = branch.Address;
                BranchPhone = branch.Phone;
                BranchTaxNumber = branch.TaxNumber;
            }
            else
            {
                BranchName = session.BranchName;
            }
        }

        // Load device info
        DeviceFingerprint = HardwareFingerprint.Generate();
        CurrentTheme = ThemeManager.CurrentTheme;

        // Try loading saved license
        var savedKey = LicenseManager.LoadLicense();
        if (!string.IsNullOrWhiteSpace(savedKey))
        {
            LicenseKey = savedKey;
            var result = LicenseManager.Validate(savedKey, DeviceFingerprint);
            LicenseStatus = result.IsValid
                ? $"Valid (expires {result.ExpiresAt:yyyy-MM-dd})"
                : $"Invalid: {result.ErrorMessage}";
        }
        else
        {
            LicenseKey = "";
            LicenseStatus = "No license found";
        }
    }

    private void LogAudit(AuditAction action, string description)
    {
        var session = _authService.CurrentSession;
        _auditService.Log(
            action,
            session?.EmployeeId ?? "unknown",
            session?.EmployeeName ?? "Unknown",
            session?.BranchId ?? "",
            description);
    }
}
