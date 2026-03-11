namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

public partial class EmployeesViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    // ─── Observable Properties ─────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Employee> employees = new();
    [ObservableProperty] private ObservableCollection<Employee> filteredEmployees = new();
    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private EmployeeRole? selectedRole;
    [ObservableProperty] private Employee? selectedEmployee;
    [ObservableProperty] private bool isDetailOpen;
    [ObservableProperty] private bool isEditDialogOpen;
    [ObservableProperty] private bool isAddDialogOpen;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private int totalEmployees;
    [ObservableProperty] private int activeEmployees;
    [ObservableProperty] private Dictionary<EmployeeRole, int> roleCounts = new();

    // ─── Constructor ───────────────────────────────────────────────

    public EmployeesViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        LoadEmployees();
    }

    // ─── Partial property change handlers ──────────────────────────

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedRoleChanged(EmployeeRole? value)
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
    private void FilterByRole(EmployeeRole? role)
    {
        SelectedRole = role;
    }

    [RelayCommand]
    private void SelectEmployee(Employee employee)
    {
        SelectedEmployee = employee;
        IsDetailOpen = true;
    }

    [RelayCommand]
    private void AddEmployee()
    {
        if (!_authService.HasPermission("employees.edit"))
        {
            _notificationService.ShowError("You do not have permission to add employees.");
            return;
        }

        IsAddDialogOpen = true;
    }

    [RelayCommand]
    private void EditEmployee(Employee employee)
    {
        if (!_authService.HasPermission("employees.edit"))
        {
            _notificationService.ShowError("You do not have permission to edit employees.");
            return;
        }

        SelectedEmployee = employee;
        IsEditDialogOpen = true;
    }

    [RelayCommand]
    private void ToggleActive(Employee employee)
    {
        if (!_authService.HasPermission("employees.edit"))
        {
            _notificationService.ShowError("You do not have permission to modify employees.");
            return;
        }

        employee.IsActive = !employee.IsActive;
        var status = employee.IsActive ? "activated" : "deactivated";

        LogAudit(AuditAction.EmployeeEdit, $"Employee {employee.Name} {status}");
        _notificationService.ShowSuccess($"Employee {employee.Name} has been {status}.");
        UpdateStats();
        ApplyFilters();
    }

    [RelayCommand]
    private void ResetPin(Employee employee)
    {
        if (!_authService.HasPermission("employees.edit"))
        {
            _notificationService.ShowError("You do not have permission to reset PINs.");
            return;
        }

        employee.Pin = "0000";
        LogAudit(AuditAction.EmployeeEdit, $"PIN reset to 0000 for employee {employee.Name} (ID: {employee.Id})");
        _notificationService.ShowSuccess($"PIN for {employee.Name} has been reset to 0000.");
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(300); // Simulate data fetch
            LoadEmployees();
            _notificationService.ShowInfo("Employee list refreshed.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ─── Private Helpers ───────────────────────────────────────────

    private void LoadEmployees()
    {
        var all = MockEmployees.All;

        Employees = new ObservableCollection<Employee>(all);
        UpdateStats();
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = Employees.AsEnumerable();

        // Filter by search text
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.Trim().ToLowerInvariant();
            filtered = filtered.Where(e =>
                e.Name.ToLowerInvariant().Contains(search) ||
                e.Email.ToLowerInvariant().Contains(search) ||
                e.Phone.Contains(search));
        }

        // Filter by role
        if (SelectedRole.HasValue)
        {
            filtered = filtered.Where(e => e.Role == SelectedRole.Value);
        }

        FilteredEmployees = new ObservableCollection<Employee>(filtered);
    }

    private void UpdateStats()
    {
        TotalEmployees = Employees.Count;
        ActiveEmployees = Employees.Count(e => e.IsActive);

        RoleCounts = Employees
            .GroupBy(e => e.Role)
            .ToDictionary(g => g.Key, g => g.Count());
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
