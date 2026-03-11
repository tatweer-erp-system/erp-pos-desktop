namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

public partial class SuppliersViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    // ─── Observable Properties ─────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Supplier> suppliers = new();
    [ObservableProperty] private ObservableCollection<Supplier> filteredSuppliers = new();
    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private Supplier? selectedSupplier;
    [ObservableProperty] private bool isDetailOpen;
    [ObservableProperty] private bool isEditDialogOpen;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private int totalSuppliers;
    [ObservableProperty] private int activeSuppliers;

    // ─── Constructor ───────────────────────────────────────────────

    public SuppliersViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        LoadSuppliers();
    }

    // ─── Partial property change handlers ──────────────────────────

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    // ─── Commands ──────────────────────────────────────────────────

    [RelayCommand]
    private void Search()
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void SelectSupplier(Supplier supplier)
    {
        SelectedSupplier = supplier;
        IsDetailOpen = true;
    }

    [RelayCommand]
    private void AddSupplier()
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to add suppliers.");
            return;
        }

        IsEditDialogOpen = true;
        SelectedSupplier = null;
    }

    [RelayCommand]
    private void EditSupplier(Supplier supplier)
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to edit suppliers.");
            return;
        }

        SelectedSupplier = supplier;
        IsEditDialogOpen = true;
    }

    [RelayCommand]
    private void ToggleActive(Supplier supplier)
    {
        if (!_authService.HasPermission("settings"))
        {
            _notificationService.ShowError("You do not have permission to modify suppliers.");
            return;
        }

        supplier.IsActive = !supplier.IsActive;
        var status = supplier.IsActive ? "activated" : "deactivated";

        LogAudit(AuditAction.SettingsChange, $"Supplier {supplier.Name} {status}");
        _notificationService.ShowSuccess($"Supplier {supplier.Name} has been {status}.");
        UpdateStats();
        ApplyFilters();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(300); // Simulate data fetch
            LoadSuppliers();
            _notificationService.ShowInfo("Supplier list refreshed.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ─── Private Helpers ───────────────────────────────────────────

    private void LoadSuppliers()
    {
        var all = MockSuppliers.All;

        Suppliers = new ObservableCollection<Supplier>(all);
        UpdateStats();
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = Suppliers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.Trim().ToLowerInvariant();
            filtered = filtered.Where(s =>
                s.Name.ToLowerInvariant().Contains(search) ||
                s.Contact.ToLowerInvariant().Contains(search) ||
                s.Email.ToLowerInvariant().Contains(search) ||
                s.Phone.Contains(search) ||
                s.Address.ToLowerInvariant().Contains(search));
        }

        FilteredSuppliers = new ObservableCollection<Supplier>(filtered);
    }

    private void UpdateStats()
    {
        TotalSuppliers = Suppliers.Count;
        ActiveSuppliers = Suppliers.Count(s => s.IsActive);
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
