namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

/// <summary>
/// ViewModel for the Tables / Floor-plan management screen.
/// Provides table listing with floor and section filtering,
/// status counts, and table operations (status change, waiter assignment, order creation).
/// </summary>
public partial class TablesViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    private List<Table> _allTables = new();

    // ─── Collections ─────────────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Table> tables = new();

    // ─── Filters ─────────────────────────────────────────────────

    [ObservableProperty] private int selectedFloor = 1;
    [ObservableProperty] private TableSection? selectedSection;

    // ─── Selection ───────────────────────────────────────────────

    [ObservableProperty] private Table? selectedTable;
    [ObservableProperty] private bool isDetailOpen;

    // ─── State ───────────────────────────────────────────────────

    [ObservableProperty] private bool isLoading;

    // ─── Summary Stats ───────────────────────────────────────────

    [ObservableProperty] private int availableCount;
    [ObservableProperty] private int occupiedCount;
    [ObservableProperty] private int reservedCount;

    public TablesViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        Initialize();
    }

    private void Initialize()
    {
        _allTables = MockTables.All.ToList();
        ApplyFilters();
        UpdateStats();
    }

    // ─── Property-Changed Hooks ──────────────────────────────────

    partial void OnSelectedFloorChanged(int value)
    {
        ApplyFilters();
        UpdateStats();
    }

    partial void OnSelectedSectionChanged(TableSection? value)
    {
        ApplyFilters();
        UpdateStats();
    }

    // ─── Filtering ───────────────────────────────────────────────

    private void ApplyFilters()
    {
        var query = _allTables.Where(t => t.Floor == SelectedFloor);

        if (SelectedSection.HasValue)
        {
            query = query.Where(t => t.Section == SelectedSection.Value);
        }

        Tables = new ObservableCollection<Table>(query.OrderBy(t => t.Number));
    }

    private void UpdateStats()
    {
        var currentFloorTables = _allTables.Where(t => t.Floor == SelectedFloor);

        if (SelectedSection.HasValue)
        {
            currentFloorTables = currentFloorTables.Where(t => t.Section == SelectedSection.Value);
        }

        var list = currentFloorTables.ToList();
        AvailableCount = list.Count(t => t.Status == TableStatus.Available);
        OccupiedCount = list.Count(t => t.Status == TableStatus.Occupied);
        ReservedCount = list.Count(t => t.Status == TableStatus.Reserved);
    }

    // ─── Commands ────────────────────────────────────────────────

    [RelayCommand]
    private void SelectTable(Table table)
    {
        SelectedTable = table;
        IsDetailOpen = table != null;
    }

    [RelayCommand]
    private void ChangeStatus((Table Table, TableStatus NewStatus) args)
    {
        if (args.Table == null) return;

        var previousStatus = args.Table.Status;
        args.Table.Status = args.NewStatus;

        // Clear order and waiter when table becomes available
        if (args.NewStatus == TableStatus.Available)
        {
            args.Table.CurrentOrderId = null;
            args.Table.WaiterName = null;
        }

        var session = _authService.CurrentSession;
        if (session != null)
        {
            _auditService.Log(
                AuditAction.SettingsChange,
                session.EmployeeId,
                session.EmployeeName,
                session.BranchId,
                $"Table {args.Table.Number} status changed: {previousStatus} -> {args.NewStatus}");
        }

        ApplyFilters();
        UpdateStats();

        _notificationService.ShowSuccess(
            $"Table {args.Table.Number} is now {args.NewStatus}");
    }

    [RelayCommand]
    private void AssignWaiter((Table Table, string WaiterName) args)
    {
        if (args.Table == null) return;

        if (string.IsNullOrWhiteSpace(args.WaiterName))
        {
            _notificationService.ShowWarning("Please provide a waiter name");
            return;
        }

        args.Table.WaiterName = args.WaiterName.Trim();

        _notificationService.ShowSuccess(
            $"Waiter '{args.Table.WaiterName}' assigned to Table {args.Table.Number}");
    }

    [RelayCommand]
    private void FilterByFloor(int floor)
    {
        SelectedFloor = floor;
    }

    [RelayCommand]
    private void FilterBySection(TableSection? section)
    {
        SelectedSection = section;
    }

    [RelayCommand]
    private void OpenOrderForTable(Table table)
    {
        if (table == null) return;

        if (table.Status == TableStatus.Occupied && !string.IsNullOrEmpty(table.CurrentOrderId))
        {
            _notificationService.ShowInfo(
                $"Table {table.Number} already has an active order: {table.CurrentOrderId}");
            return;
        }

        // Mark the table as occupied with a new placeholder order
        var orderId = $"ORD-{Guid.NewGuid().ToString("N")[..8]}";
        table.Status = TableStatus.Occupied;
        table.CurrentOrderId = orderId;

        var session = _authService.CurrentSession;
        if (session != null)
        {
            table.WaiterName ??= session.EmployeeName;

            _auditService.Log(
                AuditAction.Sale,
                session.EmployeeId,
                session.EmployeeName,
                session.BranchId,
                $"New order opened for Table {table.Number} (Order: {orderId})");
        }

        ApplyFilters();
        UpdateStats();

        _notificationService.ShowSuccess(
            $"Order opened for Table {table.Number} — switch to POS to add items");
    }

    [RelayCommand]
    private void Refresh()
    {
        IsLoading = true;

        _allTables = MockTables.All.ToList();
        ApplyFilters();
        UpdateStats();

        IsLoading = false;
        _notificationService.ShowInfo("Tables refreshed");
    }
}
