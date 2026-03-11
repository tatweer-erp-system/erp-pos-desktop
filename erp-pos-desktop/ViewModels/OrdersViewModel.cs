namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

/// <summary>
/// ViewModel for the Orders history screen.
/// Provides order listing with search, status filtering, date range filtering,
/// computed daily stats, and order operations (refund, void, print).
/// </summary>
public partial class OrdersViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    private List<Order> _allOrders = new();

    // ─── Collections ─────────────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Order> orders = new();
    [ObservableProperty] private ObservableCollection<Order> filteredOrders = new();

    // ─── Filters ─────────────────────────────────────────────────

    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private OrderStatus? selectedStatus;
    [ObservableProperty] private DateTime? dateFrom;
    [ObservableProperty] private DateTime? dateTo;

    // ─── Selection ───────────────────────────────────────────────

    [ObservableProperty] private Order? selectedOrder;
    [ObservableProperty] private bool isDetailOpen;

    // ─── State ───────────────────────────────────────────────────

    [ObservableProperty] private bool isLoading;

    // ─── Summary Stats ───────────────────────────────────────────

    [ObservableProperty] private int todayOrders;
    [ObservableProperty] private decimal todayRevenue;
    [ObservableProperty] private Dictionary<OrderStatus, int> statusCounts = new();

    public OrdersViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        Initialize();
    }

    private void Initialize()
    {
        _allOrders = MockOrders.All.ToList();
        Orders = new ObservableCollection<Order>(_allOrders);
        ApplyFilters();
        UpdateStats();
    }

    // ─── Property-Changed Hooks ──────────────────────────────────

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedStatusChanged(OrderStatus? value)
    {
        ApplyFilters();
    }

    // ─── Filtering ───────────────────────────────────────────────

    private void ApplyFilters()
    {
        var query = _allOrders.AsEnumerable();

        // Status filter
        if (SelectedStatus.HasValue)
        {
            query = query.Where(o => o.Status == SelectedStatus.Value);
        }

        // Date range filter
        if (DateFrom.HasValue)
        {
            query = query.Where(o => o.CreatedAt.Date >= DateFrom.Value.Date);
        }
        if (DateTo.HasValue)
        {
            query = query.Where(o => o.CreatedAt.Date <= DateTo.Value.Date);
        }

        // Search text filter (order number, customer name, employee name)
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var lower = SearchText.Trim().ToLowerInvariant();
            query = query.Where(o =>
                o.Number.ToLowerInvariant().Contains(lower) ||
                o.CustomerName.ToLowerInvariant().Contains(lower) ||
                o.EmployeeName.ToLowerInvariant().Contains(lower));
        }

        // Most recent first
        FilteredOrders = new ObservableCollection<Order>(query.OrderByDescending(o => o.CreatedAt));
    }

    private void UpdateStats()
    {
        var today = DateTime.Today;
        var todayList = _allOrders.Where(o => o.CreatedAt.Date == today).ToList();

        TodayOrders = todayList.Count;
        TodayRevenue = todayList
            .Where(o => o.Status == OrderStatus.Completed)
            .Sum(o => o.GrandTotal);

        StatusCounts = _allOrders
            .GroupBy(o => o.Status)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    // ─── Commands ────────────────────────────────────────────────

    [RelayCommand]
    private void Search()
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void FilterByStatus(OrderStatus? status)
    {
        SelectedStatus = status;
    }

    [RelayCommand]
    private void FilterByDate((DateTime From, DateTime To) range)
    {
        DateFrom = range.From;
        DateTo = range.To;
        ApplyFilters();
    }

    [RelayCommand]
    private void SelectOrder(Order order)
    {
        SelectedOrder = order;
        IsDetailOpen = order != null;
    }

    [RelayCommand]
    private void RefundOrder(Order order)
    {
        if (!_authService.HasPermission("pos.refund"))
        {
            _notificationService.ShowError("Permission denied — supervisor or manager authorization required for refunds");
            return;
        }

        if (order == null) return;

        if (order.Status != OrderStatus.Completed)
        {
            _notificationService.ShowError($"Cannot refund order with status '{order.Status}'");
            return;
        }

        order.Status = OrderStatus.Refunded;

        var session = _authService.CurrentSession;
        if (session != null)
        {
            _auditService.Log(
                AuditAction.Refund,
                session.EmployeeId,
                session.EmployeeName,
                session.BranchId,
                $"Order {order.Number} refunded — {order.GrandTotal:F2} SAR");
        }

        ApplyFilters();
        UpdateStats();
        _notificationService.ShowSuccess($"Order {order.Number} has been refunded");
    }

    [RelayCommand]
    private void VoidOrder(Order order)
    {
        if (!_authService.HasPermission("pos.void"))
        {
            _notificationService.ShowError("Permission denied — manager authorization required to void orders");
            return;
        }

        if (order == null) return;

        if (order.Status == OrderStatus.Voided || order.Status == OrderStatus.Refunded)
        {
            _notificationService.ShowError($"Order is already {order.Status}");
            return;
        }

        order.Status = OrderStatus.Voided;

        var session = _authService.CurrentSession;
        if (session != null)
        {
            _auditService.Log(
                AuditAction.Void,
                session.EmployeeId,
                session.EmployeeName,
                session.BranchId,
                $"Order {order.Number} voided — {order.GrandTotal:F2} SAR");
        }

        ApplyFilters();
        UpdateStats();
        _notificationService.ShowWarning($"Order {order.Number} has been voided");
    }

    [RelayCommand]
    private void PrintReceipt(Order order)
    {
        if (order == null) return;

        // Mock print — in production this would invoke PrintService
        _notificationService.ShowSuccess($"Receipt for order {order.Number} sent to printer");
    }

    [RelayCommand]
    private void Refresh()
    {
        IsLoading = true;

        _allOrders = MockOrders.All.ToList();
        Orders = new ObservableCollection<Order>(_allOrders);
        ApplyFilters();
        UpdateStats();

        IsLoading = false;
        _notificationService.ShowInfo("Orders refreshed");
    }
}
