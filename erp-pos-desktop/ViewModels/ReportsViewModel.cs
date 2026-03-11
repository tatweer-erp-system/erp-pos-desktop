namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

public partial class ReportsViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    // ─── Observable Properties ─────────────────────────────────────

    [ObservableProperty] private string selectedReportType = "Sales";
    [ObservableProperty] private DateTime dateFrom = DateTime.Today.AddDays(-30);
    [ObservableProperty] private DateTime dateTo = DateTime.Today;
    [ObservableProperty] private bool isLoading;

    // Sales stats
    [ObservableProperty] private decimal totalSales;
    [ObservableProperty] private int totalOrders;
    [ObservableProperty] private decimal averageOrderValue;

    // Top products: (Product, Quantity, Revenue)
    [ObservableProperty] private List<(Product Product, int Quantity, decimal Revenue)> topProducts = new();

    // Top customers: (Customer, Orders, Spent)
    [ObservableProperty] private List<(Customer Customer, int Orders, decimal Spent)> topCustomers = new();

    // Breakdowns
    [ObservableProperty] private Dictionary<PaymentMethod, decimal> salesByPaymentMethod = new();
    [ObservableProperty] private Dictionary<DateTime, decimal> salesByDay = new();

    // Audit
    [ObservableProperty] private ObservableCollection<AuditLog> auditLogs = new();
    [ObservableProperty] private AuditAction? selectedAuditAction;

    // Static data
    public List<string> ReportTypes { get; } = new() { "Sales", "Products", "Customers", "Employees", "Audit" };

    // ─── Constructor ───────────────────────────────────────────────

    public ReportsViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        GenerateReport();
    }

    // ─── Commands ──────────────────────────────────────────────────

    [RelayCommand]
    private async Task GenerateReportAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(500); // Simulate processing
            GenerateReport();
            _notificationService.ShowSuccess($"{SelectedReportType} report generated successfully.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void FilterAuditLogs()
    {
        var allLogs = _auditService.GetAll();

        var filtered = allLogs
            .Where(l => l.Timestamp >= DateFrom && l.Timestamp <= DateTo.AddDays(1));

        if (SelectedAuditAction.HasValue)
        {
            filtered = filtered.Where(l => l.Action == SelectedAuditAction.Value);
        }

        AuditLogs = new ObservableCollection<AuditLog>(
            filtered.OrderByDescending(l => l.Timestamp));
    }

    [RelayCommand]
    private async Task ExportToPdfAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(800); // Simulate PDF generation
            LogAudit(AuditAction.Sale, $"Exported {SelectedReportType} report to PDF (Date range: {DateFrom:yyyy-MM-dd} to {DateTo:yyyy-MM-dd})");
            _notificationService.ShowSuccess("Report exported to PDF successfully.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task PrintReportAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(500); // Simulate print job
            _notificationService.ShowSuccess("Report sent to printer.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await GenerateReportAsync();
    }

    [RelayCommand]
    private void SelectReportType(string reportType)
    {
        SelectedReportType = reportType;
        GenerateReport();
    }

    // ─── Private Helpers ───────────────────────────────────────────

    private void GenerateReport()
    {
        switch (SelectedReportType)
        {
            case "Sales":
                GenerateSalesReport();
                break;
            case "Products":
                GenerateProductsReport();
                break;
            case "Customers":
                GenerateCustomersReport();
                break;
            case "Employees":
                GenerateEmployeesReport();
                break;
            case "Audit":
                FilterAuditLogs();
                break;
        }
    }

    private void GenerateSalesReport()
    {
        var orders = MockOrders.All
            .Where(o => o.Status == OrderStatus.Completed &&
                        o.CreatedAt >= DateFrom &&
                        o.CreatedAt <= DateTo.AddDays(1))
            .ToList();

        TotalOrders = orders.Count;
        TotalSales = orders.Sum(o => o.GrandTotal);
        AverageOrderValue = TotalOrders > 0 ? TotalSales / TotalOrders : 0;

        // Sales by payment method
        SalesByPaymentMethod = orders
            .GroupBy(o => o.PaymentMethod)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.GrandTotal));

        // Sales by day
        SalesByDay = orders
            .GroupBy(o => o.CreatedAt.Date)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.GrandTotal));
    }

    private void GenerateProductsReport()
    {
        var orders = MockOrders.All
            .Where(o => o.Status == OrderStatus.Completed &&
                        o.CreatedAt >= DateFrom &&
                        o.CreatedAt <= DateTo.AddDays(1))
            .ToList();

        var allProducts = MockProducts.All;

        // Aggregate product sales from order items
        var productSales = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => i.ProductId)
            .Select(g =>
            {
                var product = allProducts.FirstOrDefault(p => p.Id == g.Key);
                return (
                    Product: product ?? new Product { Name = g.First().ProductName },
                    Quantity: g.Sum(i => i.Quantity),
                    Revenue: g.Sum(i => i.UnitPrice * i.Quantity)
                );
            })
            .OrderByDescending(x => x.Revenue)
            .Take(10)
            .ToList();

        TopProducts = productSales;

        // Update overall stats from orders
        TotalOrders = orders.Count;
        TotalSales = orders.Sum(o => o.GrandTotal);
        AverageOrderValue = TotalOrders > 0 ? TotalSales / TotalOrders : 0;
    }

    private void GenerateCustomersReport()
    {
        var orders = MockOrders.All
            .Where(o => o.Status == OrderStatus.Completed &&
                        o.CreatedAt >= DateFrom &&
                        o.CreatedAt <= DateTo.AddDays(1))
            .ToList();

        var allCustomers = MockCustomers.All;

        // Aggregate customer stats from orders
        var customerStats = orders
            .Where(o => !string.IsNullOrEmpty(o.CustomerId))
            .GroupBy(o => o.CustomerId)
            .Select(g =>
            {
                var customer = allCustomers.FirstOrDefault(c => c.Id == g.Key);
                return (
                    Customer: customer ?? new Customer { Name = g.First().CustomerName },
                    Orders: g.Count(),
                    Spent: g.Sum(o => o.GrandTotal)
                );
            })
            .OrderByDescending(x => x.Spent)
            .Take(10)
            .ToList();

        TopCustomers = customerStats;

        // Update overall stats
        TotalOrders = orders.Count;
        TotalSales = orders.Sum(o => o.GrandTotal);
        AverageOrderValue = TotalOrders > 0 ? TotalSales / TotalOrders : 0;
    }

    private void GenerateEmployeesReport()
    {
        var orders = MockOrders.All
            .Where(o => o.Status == OrderStatus.Completed &&
                        o.CreatedAt >= DateFrom &&
                        o.CreatedAt <= DateTo.AddDays(1))
            .ToList();

        // Employee performance is reflected via sales stats
        TotalOrders = orders.Count;
        TotalSales = orders.Sum(o => o.GrandTotal);
        AverageOrderValue = TotalOrders > 0 ? TotalSales / TotalOrders : 0;

        // Sales by day for chart
        SalesByDay = orders
            .GroupBy(o => o.CreatedAt.Date)
            .OrderBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.GrandTotal));
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
