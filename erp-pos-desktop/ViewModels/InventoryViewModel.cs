namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

/// <summary>
/// ViewModel for the Inventory management screen.
/// Provides product listing with search, category and business-mode filtering,
/// stock counts, and stock adjustment operations.
/// </summary>
public partial class InventoryViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    private List<Product> _allProducts = new();

    // ─── Collections ─────────────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Product> products = new();
    [ObservableProperty] private ObservableCollection<Product> filteredProducts = new();

    // ─── Filters ─────────────────────────────────────────────────

    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private string selectedCategory = "All";
    [ObservableProperty] private List<string> categories = new();
    [ObservableProperty] private BusinessMode? selectedBusinessMode;

    // ─── Selection ───────────────────────────────────────────────

    [ObservableProperty] private Product? selectedProduct;
    [ObservableProperty] private bool isEditDialogOpen;

    // ─── State ───────────────────────────────────────────────────

    [ObservableProperty] private bool isLoading;

    // ─── Summary Stats ───────────────────────────────────────────

    [ObservableProperty] private int lowStockCount;
    [ObservableProperty] private int outOfStockCount;
    [ObservableProperty] private int totalProducts;

    public InventoryViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        Initialize();
    }

    private void Initialize()
    {
        _allProducts = MockProducts.All.ToList();

        var cats = new List<string> { "All" };
        cats.AddRange(_allProducts.Select(p => p.CategoryName).Distinct().OrderBy(c => c));
        Categories = cats;

        Products = new ObservableCollection<Product>(_allProducts);
        ApplyFilters();
        UpdateStats();
    }

    // ─── Property-Changed Hooks ──────────────────────────────────

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedBusinessModeChanged(BusinessMode? value)
    {
        ApplyFilters();
    }

    // ─── Filtering ───────────────────────────────────────────────

    private void ApplyFilters()
    {
        var query = _allProducts.AsEnumerable();

        // Category filter
        if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
        {
            query = query.Where(p => p.CategoryName == SelectedCategory);
        }

        // Business mode filter
        if (SelectedBusinessMode.HasValue)
        {
            query = query.Where(p => p.BusinessMode == SelectedBusinessMode.Value);
        }

        // Search text filter
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var lower = SearchText.Trim().ToLowerInvariant();
            query = query.Where(p =>
                p.Name.ToLowerInvariant().Contains(lower) ||
                p.Barcode.Contains(lower) ||
                p.Sku.ToLowerInvariant().Contains(lower));
        }

        FilteredProducts = new ObservableCollection<Product>(query);
        TotalProducts = FilteredProducts.Count;
    }

    private void UpdateStats()
    {
        LowStockCount = _allProducts.Count(p => p.Stock > 0 && p.Stock <= p.MinStock);
        OutOfStockCount = _allProducts.Count(p => p.Stock == 0);
        TotalProducts = _allProducts.Count;
    }

    // ─── Commands ────────────────────────────────────────────────

    [RelayCommand]
    private void Search()
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void FilterByCategory(string category)
    {
        SelectedCategory = category ?? "All";
    }

    [RelayCommand]
    private void FilterByBusinessMode(BusinessMode? mode)
    {
        SelectedBusinessMode = mode;
    }

    [RelayCommand]
    private void SelectProduct(Product product)
    {
        SelectedProduct = product;
    }

    [RelayCommand]
    private void EditProduct(Product product)
    {
        if (!_authService.HasPermission("inventory.edit"))
        {
            _notificationService.ShowError("Permission denied — you do not have inventory edit access");
            return;
        }

        if (product == null) return;

        SelectedProduct = product;
        IsEditDialogOpen = true;
    }

    [RelayCommand]
    private void AdjustStock((Product Product, int Adjustment) args)
    {
        if (!_authService.HasPermission("inventory.edit"))
        {
            _notificationService.ShowError("Permission denied — you do not have inventory edit access");
            return;
        }

        if (args.Product == null) return;

        var product = args.Product;
        var newStock = product.Stock + args.Adjustment;

        if (newStock < 0)
        {
            _notificationService.ShowError($"Cannot reduce stock below 0 (current: {product.Stock})");
            return;
        }

        var previousStock = product.Stock;
        product.Stock = newStock;

        var session = _authService.CurrentSession;
        if (session != null)
        {
            _auditService.Log(
                AuditAction.StockAdjustment,
                session.EmployeeId,
                session.EmployeeName,
                session.BranchId,
                $"Stock adjusted for '{product.Name}': {previousStock} -> {newStock} (adjustment: {args.Adjustment:+#;-#;0})");
        }

        UpdateStats();
        ApplyFilters();

        _notificationService.ShowSuccess(
            $"Stock for '{product.Name}' updated: {previousStock} -> {newStock}");
    }

    [RelayCommand]
    private void Refresh()
    {
        IsLoading = true;

        _allProducts = MockProducts.All.ToList();
        Products = new ObservableCollection<Product>(_allProducts);
        ApplyFilters();
        UpdateStats();

        IsLoading = false;
        _notificationService.ShowInfo("Inventory refreshed");
    }
}
