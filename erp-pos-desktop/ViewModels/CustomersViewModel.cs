namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

/// <summary>
/// ViewModel for the Customers management screen.
/// Provides customer listing with search, tier filtering,
/// summary statistics, and CRUD operations with permission checks.
/// </summary>
public partial class CustomersViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    private List<Customer> _allCustomers = new();

    // ─── Collections ─────────────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Customer> customers = new();
    [ObservableProperty] private ObservableCollection<Customer> filteredCustomers = new();

    // ─── Filters ─────────────────────────────────────────────────

    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private CustomerTier? selectedTier;

    // ─── Selection ───────────────────────────────────────────────

    [ObservableProperty] private Customer? selectedCustomer;
    [ObservableProperty] private bool isDetailOpen;
    [ObservableProperty] private bool isEditDialogOpen;

    // ─── State ───────────────────────────────────────────────────

    [ObservableProperty] private bool isLoading;

    // ─── Summary Stats ───────────────────────────────────────────

    [ObservableProperty] private int totalCustomers;
    [ObservableProperty] private int activeCustomers;
    [ObservableProperty] private int totalLoyaltyPoints;

    public CustomersViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        Initialize();
    }

    private void Initialize()
    {
        _allCustomers = MockCustomers.All.ToList();
        Customers = new ObservableCollection<Customer>(_allCustomers);
        ApplyFilters();
        UpdateStats();
    }

    // ─── Property-Changed Hooks ──────────────────────────────────

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedTierChanged(CustomerTier? value)
    {
        ApplyFilters();
    }

    // ─── Filtering ───────────────────────────────────────────────

    private void ApplyFilters()
    {
        var query = _allCustomers.AsEnumerable();

        // Tier filter
        if (SelectedTier.HasValue)
        {
            query = query.Where(c => c.Tier == SelectedTier.Value);
        }

        // Search text filter (name, email, phone)
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var lower = SearchText.Trim().ToLowerInvariant();
            query = query.Where(c =>
                c.Name.ToLowerInvariant().Contains(lower) ||
                c.Email.ToLowerInvariant().Contains(lower) ||
                c.Phone.Contains(lower));
        }

        FilteredCustomers = new ObservableCollection<Customer>(query);
    }

    private void UpdateStats()
    {
        TotalCustomers = _allCustomers.Count;
        ActiveCustomers = _allCustomers.Count(c => c.IsActive);
        TotalLoyaltyPoints = _allCustomers.Sum(c => c.LoyaltyPoints);
    }

    // ─── Commands ────────────────────────────────────────────────

    [RelayCommand]
    private void Search()
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void FilterByTier(CustomerTier? tier)
    {
        SelectedTier = tier;
    }

    [RelayCommand]
    private void SelectCustomer(Customer customer)
    {
        SelectedCustomer = customer;
        IsDetailOpen = customer != null;
    }

    [RelayCommand]
    private void EditCustomer(Customer customer)
    {
        if (!_authService.HasPermission("customers.edit"))
        {
            _notificationService.ShowError("Permission denied — you do not have customer edit access");
            return;
        }

        if (customer == null) return;

        SelectedCustomer = customer;
        IsEditDialogOpen = true;
    }

    [RelayCommand]
    private void AddCustomer()
    {
        if (!_authService.HasPermission("customers.edit"))
        {
            _notificationService.ShowError("Permission denied — you do not have customer creation access");
            return;
        }

        var newCustomer = new Customer
        {
            Id = $"C{(_allCustomers.Count + 1):D3}",
            Name = "New Customer",
            Email = "",
            Phone = "",
            Address = "",
            Tier = CustomerTier.Bronze,
            LoyaltyPoints = 0,
            TotalSpent = 0m,
            TotalVisits = 0,
            JoinDate = DateTime.UtcNow,
            IsActive = true
        };

        _allCustomers.Add(newCustomer);
        Customers.Add(newCustomer);
        ApplyFilters();
        UpdateStats();

        SelectedCustomer = newCustomer;
        IsEditDialogOpen = true;

        _notificationService.ShowSuccess("New customer created — please fill in the details");
    }

    [RelayCommand]
    private void Refresh()
    {
        IsLoading = true;

        _allCustomers = MockCustomers.All.ToList();
        Customers = new ObservableCollection<Customer>(_allCustomers);
        ApplyFilters();
        UpdateStats();

        IsLoading = false;
        _notificationService.ShowInfo("Customers refreshed");
    }
}
