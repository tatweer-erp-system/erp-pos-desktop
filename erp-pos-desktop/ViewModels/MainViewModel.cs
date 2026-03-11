using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using TatweerPOS.Models.Enums;
using TatweerPOS.Services;

namespace TatweerPOS.ViewModels;

/// <summary>
/// Primary application shell ViewModel. Manages sidebar navigation, top bar state,
/// inactivity-based PIN lock, theme/language toggling, and the content area.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;
    private readonly AuthService _authService;
    private readonly AuditService _auditService;
    private readonly NotificationService _notificationService;

    private readonly DispatcherTimer _inactivityTimer;
    private readonly DispatcherTimer? _pinLockoutTimer;

    // ── Current Page ──────────────────────────────────────────────────
    [ObservableProperty] private object? currentPage;
    [ObservableProperty] private string currentPageTitle = "";

    // ── User Info ─────────────────────────────────────────────────────
    [ObservableProperty] private string userName = "";
    [ObservableProperty] private string userRole = "";
    [ObservableProperty] private string userInitials = "";
    [ObservableProperty] private string branchName = "";

    // ── PIN Lock ──────────────────────────────────────────────────────
    [ObservableProperty] private bool isPinLocked;
    [ObservableProperty] private int pinFailCount;
    [ObservableProperty] private bool isPinLockout;
    [ObservableProperty] private int pinLockoutCountdown;
    [ObservableProperty] private string pinErrorMessage = "";

    // ── Theme / Language ──────────────────────────────────────────────
    [ObservableProperty] private bool isDarkTheme = true;
    [ObservableProperty] private string selectedLanguage = "EN";

    // ── Sync / Notifications ──────────────────────────────────────────
    [ObservableProperty] private string syncStatus = "Synced";
    [ObservableProperty] private int notificationCount;

    // ── Sidebar ───────────────────────────────────────────────────────
    [ObservableProperty] private bool isSidebarExpanded = true;
    [ObservableProperty] private string selectedPageName = "";

    /// <summary>
    /// Navigation items displayed in the sidebar.
    /// </summary>
    public ObservableCollection<NavigationItem> NavigationItems { get; } = new();

    // ── Events for view-level animations ──────────────────────────────
    public event Action? PinShakeRequested;
    public event Action? LogoutRequested;

    public MainViewModel(
        NavigationService navigationService,
        AuthService authService,
        AuditService auditService,
        NotificationService notificationService)
    {
        _navigationService = navigationService;
        _authService = authService;
        _auditService = auditService;
        _notificationService = notificationService;

        // Wire up navigation service page changes
        _navigationService.PageChanged += OnPageChanged;

        // Inactivity timer: 5 minutes → auto PIN lock
        _inactivityTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(5)
        };
        _inactivityTimer.Tick += (_, _) =>
        {
            if (!IsPinLocked)
            {
                LockScreen();
            }
        };

        // Initialize from session
        InitializeFromSession();

        // Register all pages
        RegisterPages();

        // Build sidebar navigation items
        BuildNavigationItems();

        // Navigate to POS by default
        Navigate("POS");

        // Start inactivity timer
        _inactivityTimer.Start();
    }

    // ══════════════════════════════════════════════════════════════════
    // Initialization
    // ══════════════════════════════════════════════════════════════════

    private void InitializeFromSession()
    {
        var session = _authService.CurrentSession;
        if (session != null)
        {
            UserName = session.EmployeeName;
            UserRole = session.Role.ToString();
            BranchName = session.BranchName;

            // Compute initials: first letter of each word in the name
            var parts = session.EmployeeName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            UserInitials = parts.Length >= 2
                ? $"{parts[0][0]}{parts[1][0]}".ToUpper()
                : session.EmployeeName.Length >= 2
                    ? session.EmployeeName[..2].ToUpper()
                    : session.EmployeeName.ToUpper();
        }
    }

    private void RegisterPages()
    {
        var pages = new Dictionary<string, Type>
        {
            ["POS"] = typeof(POSViewModel),
            ["Orders"] = typeof(OrdersViewModel),
            ["Inventory"] = typeof(InventoryViewModel),
            ["Customers"] = typeof(CustomersViewModel),
            ["Tables"] = typeof(TablesViewModel),
            ["Kitchen"] = typeof(KitchenViewModel),
            ["Employees"] = typeof(EmployeesViewModel),
            ["Suppliers"] = typeof(SuppliersViewModel),
            ["Reports"] = typeof(ReportsViewModel),
            ["Settings"] = typeof(SettingsViewModel),
        };

        _navigationService.RegisterPages(pages);
    }

    private void BuildNavigationItems()
    {
        NavigationItems.Add(new NavigationItem("POS", PackIconKind.CartOutline, "POS", "pos.sell"));
        NavigationItems.Add(new NavigationItem("Orders", PackIconKind.ClipboardTextOutline, "Orders", "orders.view"));
        NavigationItems.Add(new NavigationItem("Inventory", PackIconKind.PackageVariant, "Inventory", "inventory.view"));
        NavigationItems.Add(new NavigationItem("Customers", PackIconKind.AccountGroupOutline, "Customers", "customers.view"));
        NavigationItems.Add(new NavigationItem("Tables", PackIconKind.TableChair, "Tables", "pos.sell"));
        NavigationItems.Add(new NavigationItem("Kitchen", PackIconKind.Stove, "Kitchen", "pos.sell"));
        NavigationItems.Add(new NavigationItem("Employees", PackIconKind.BadgeAccountOutline, "Employees", "employees.view"));
        NavigationItems.Add(new NavigationItem("Suppliers", PackIconKind.TruckDeliveryOutline, "Suppliers", "inventory.view"));
        NavigationItems.Add(new NavigationItem("Reports", PackIconKind.ChartBar, "Reports", "reports"));
        NavigationItems.Add(new NavigationItem("Settings", PackIconKind.CogOutline, "Settings", "settings"));
    }

    // ══════════════════════════════════════════════════════════════════
    // Navigation
    // ══════════════════════════════════════════════════════════════════

    [RelayCommand]
    private void Navigate(string pageName)
    {
        // Find the navigation item to check permission
        var navItem = NavigationItems.FirstOrDefault(n => n.PageName == pageName);
        if (navItem != null && !string.IsNullOrEmpty(navItem.RequiredPermission))
        {
            if (!_authService.HasPermission(navItem.RequiredPermission))
            {
                _notificationService.ShowWarning("Access denied. You do not have permission to access this page.");
                return;
            }
        }

        _navigationService.NavigateTo(pageName);
        SelectedPageName = pageName;
    }

    private void OnPageChanged(object page)
    {
        // Set the page name on placeholder ViewModels so the UI can display it
        if (page is PlaceholderPageViewModel placeholder)
        {
            placeholder.PageName = _navigationService.CurrentPageTitle;
        }

        CurrentPage = page;
        CurrentPageTitle = _navigationService.CurrentPageTitle;
    }

    // ══════════════════════════════════════════════════════════════════
    // Sidebar
    // ══════════════════════════════════════════════════════════════════

    [RelayCommand]
    private void ToggleSidebar()
    {
        IsSidebarExpanded = !IsSidebarExpanded;
    }

    // ══════════════════════════════════════════════════════════════════
    // Theme
    // ══════════════════════════════════════════════════════════════════

    [RelayCommand]
    private void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
    }

    // ══════════════════════════════════════════════════════════════════
    // Language
    // ══════════════════════════════════════════════════════════════════

    [RelayCommand]
    private void SwitchLanguage(string lang)
    {
        SelectedLanguage = lang;

        if (Application.Current?.MainWindow != null)
        {
            Application.Current.MainWindow.FlowDirection =
                lang == "AR" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }
    }

    // ══════════════════════════════════════════════════════════════════
    // PIN Lock / Unlock
    // ══════════════════════════════════════════════════════════════════

    [RelayCommand]
    private void LockScreen()
    {
        IsPinLocked = true;
        PinFailCount = 0;
        PinErrorMessage = "";

        if (_authService.CurrentSession != null)
        {
            _auditService.Log(
                AuditAction.PinLock,
                _authService.CurrentSession.EmployeeId,
                _authService.CurrentSession.EmployeeName,
                _authService.CurrentSession.BranchId,
                "Screen locked");
        }

        _inactivityTimer.Stop();
    }

    [RelayCommand]
    private void Unlock(string pin)
    {
        if (IsPinLockout) return;

        if (_authService.ValidatePin(pin))
        {
            IsPinLocked = false;
            PinFailCount = 0;
            PinErrorMessage = "";

            if (_authService.CurrentSession != null)
            {
                _auditService.Log(
                    AuditAction.PinUnlock,
                    _authService.CurrentSession.EmployeeId,
                    _authService.CurrentSession.EmployeeName,
                    _authService.CurrentSession.BranchId,
                    "Screen unlocked via PIN");
            }

            // Restart inactivity timer
            _inactivityTimer.Start();
        }
        else
        {
            PinFailCount++;
            PinErrorMessage = "Wrong PIN";

            // Request shake animation
            PinShakeRequested?.Invoke();

            if (PinFailCount >= 3)
            {
                // 30-second lockout
                IsPinLockout = true;
                PinLockoutCountdown = 30;
                PinErrorMessage = "Too many attempts. Locked for 30 seconds.";

                var lockoutTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                lockoutTimer.Tick += (_, _) =>
                {
                    PinLockoutCountdown--;
                    if (PinLockoutCountdown <= 0)
                    {
                        lockoutTimer.Stop();
                        IsPinLockout = false;
                        PinFailCount = 0;
                        PinErrorMessage = "";
                    }
                };
                lockoutTimer.Start();
            }
        }
    }

    // ══════════════════════════════════════════════════════════════════
    // Logout
    // ══════════════════════════════════════════════════════════════════

    [RelayCommand]
    private void Logout()
    {
        _inactivityTimer.Stop();
        _authService.Logout();
        LogoutRequested?.Invoke();
    }

    // ══════════════════════════════════════════════════════════════════
    // Inactivity
    // ══════════════════════════════════════════════════════════════════

    /// <summary>
    /// Resets the inactivity timer. Should be called on any mouse/keyboard activity.
    /// </summary>
    public void UpdateActivity()
    {
        if (IsPinLocked) return;

        _authService.UpdateActivity();
        _inactivityTimer.Stop();
        _inactivityTimer.Start();
    }
}

// ══════════════════════════════════════════════════════════════════════
// Supporting Types
// ══════════════════════════════════════════════════════════════════════

/// <summary>
/// Represents a navigation item in the sidebar.
/// </summary>
public class NavigationItem
{
    public string Name { get; }
    public PackIconKind Icon { get; }
    public string PageName { get; }
    public string RequiredPermission { get; }

    /// <summary>
    /// The string resource key for the display name (e.g., "str_POS").
    /// </summary>
    public string StringResourceKey => $"str_{Name}";

    public NavigationItem(string name, PackIconKind icon, string pageName, string requiredPermission)
    {
        Name = name;
        Icon = icon;
        PageName = pageName;
        RequiredPermission = requiredPermission;
    }
}

/// <summary>
/// Placeholder ViewModel used until actual page ViewModels are created.
/// Registered in DI as transient so each navigation creates a new instance.
/// </summary>
public partial class PlaceholderPageViewModel : ObservableObject
{
    [ObservableProperty] private string pageName = "";

    public PlaceholderPageViewModel()
    {
        PageName = "Placeholder";
    }
}
