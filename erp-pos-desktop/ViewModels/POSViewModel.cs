namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;
using TatweerPOS.Helpers;

/// <summary>
/// Nested class representing a single order tab in the multi-tab POS interface.
/// </summary>
public class OrderTab : ObservableObject
{
    public int TabIndex { get; set; }
    public string Name { get; set; } = "";
    public ObservableCollection<OrderItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.LineTotal);
}

/// <summary>
/// Main POS screen ViewModel. Manages the product grid, multi-tab cart,
/// payment flow, coupon application, and order completion.
/// </summary>
public partial class POSViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    private List<Product> _allProducts = new();
    private int _orderCounter;

    // ─── Product Grid ────────────────────────────────────────────

    [ObservableProperty] private ObservableCollection<Product> products = new();
    [ObservableProperty] private List<string> categories = new();
    [ObservableProperty] private string selectedCategory = "All";
    [ObservableProperty] private string searchText = "";

    // ─── Cart / Order Tabs ───────────────────────────────────────

    [ObservableProperty] private ObservableCollection<OrderItem> cartItems = new();
    [ObservableProperty] private int currentOrderTab;
    [ObservableProperty] private ObservableCollection<OrderTab> orderTabs = new();

    // ─── Totals (computed) ───────────────────────────────────────

    [ObservableProperty] private decimal subtotal;
    [ObservableProperty] private decimal discountAmount;
    [ObservableProperty] private decimal taxTotal;
    [ObservableProperty] private decimal grandTotal;

    // ─── Customer & Payment ──────────────────────────────────────

    [ObservableProperty] private Customer? selectedCustomer;
    [ObservableProperty] private PaymentMethod selectedPaymentMethod = PaymentMethod.Cash;
    [ObservableProperty] private OrderType selectedOrderType = OrderType.DineIn;

    // ─── Coupon ──────────────────────────────────────────────────

    [ObservableProperty] private string couponCode = "";
    [ObservableProperty] private bool couponApplied;
    [ObservableProperty] private decimal couponDiscount;

    // ─── Payment Dialog ──────────────────────────────────────────

    [ObservableProperty] private decimal amountTendered;
    [ObservableProperty] private decimal changeAmount;
    [ObservableProperty] private bool isPaymentDialogOpen;

    // ─── Table ───────────────────────────────────────────────────

    [ObservableProperty] private int? selectedTable;

    // ─── State ───────────────────────────────────────────────────

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool hasItems;

    public POSViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        Initialize();
    }

    // ─── Initialization ──────────────────────────────────────────

    private void Initialize()
    {
        _allProducts = MockProducts.All.Where(p => p.IsActive).ToList();

        var cats = new List<string> { "All" };
        cats.AddRange(_allProducts.Select(p => p.CategoryName).Distinct().OrderBy(c => c));
        Categories = cats;

        ApplyProductFilters();

        // Create the first order tab
        var firstTab = new OrderTab { TabIndex = 0, Name = "Order #1" };
        OrderTabs.Add(firstTab);
        CurrentOrderTab = 0;
        CartItems = firstTab.Items;

        _orderCounter = MockOrders.All.Count;
    }

    // ─── Partial Property-Changed Hooks ──────────────────────────

    partial void OnSearchTextChanged(string value)
    {
        ApplyProductFilters();
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        ApplyProductFilters();
    }

    partial void OnAmountTenderedChanged(decimal value)
    {
        ChangeAmount = value > GrandTotal ? value - GrandTotal : 0m;
    }

    partial void OnCurrentOrderTabChanged(int value)
    {
        if (value >= 0 && value < OrderTabs.Count)
        {
            CartItems = OrderTabs[value].Items;
            RecalculateTotals();
        }
    }

    // ─── Product Filtering ───────────────────────────────────────

    private void ApplyProductFilters()
    {
        var query = _allProducts.AsEnumerable();

        if (!string.IsNullOrEmpty(SelectedCategory) && SelectedCategory != "All")
        {
            query = query.Where(p => p.CategoryName == SelectedCategory);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim();

            // Barcode detection: 8+ digits treated as barcode scan
            if (term.Length >= 8 && term.All(char.IsDigit))
            {
                query = query.Where(p => p.Barcode == term);
            }
            else
            {
                var lower = term.ToLowerInvariant();
                query = query.Where(p =>
                    p.Name.ToLowerInvariant().Contains(lower) ||
                    p.Barcode.Contains(lower) ||
                    p.Sku.ToLowerInvariant().Contains(lower));
            }
        }

        Products = new ObservableCollection<Product>(query);
    }

    // ─── Cart Calculation (per CLAUDE.md order) ──────────────────

    private void RecalculateTotals()
    {
        var items = CartItems;

        // 1. Sum line items (qty x unit_price)
        var lineSubtotal = items.Sum(i => i.UnitPrice * i.Quantity);

        // 2. Apply line-level discounts
        var lineDiscounts = items.Sum(i => i.DiscountAmount);
        var afterLineDiscounts = lineSubtotal - lineDiscounts;

        // 3. Apply order-level discount (none in this mock — reserved for future)
        var orderDiscount = 0m;

        // 4. Apply coupon
        var afterCoupon = afterLineDiscounts - orderDiscount - CouponDiscount;
        if (afterCoupon < 0) afterCoupon = 0;

        // 5. Calculate tax per product
        var tax = items.Sum(i => i.TaxAmount);

        // 6. Grand total
        Subtotal = lineSubtotal;
        DiscountAmount = lineDiscounts + orderDiscount + CouponDiscount;
        TaxTotal = tax;
        GrandTotal = afterCoupon + tax;

        HasItems = items.Count > 0;

        // Update change when totals shift
        ChangeAmount = AmountTendered > GrandTotal ? AmountTendered - GrandTotal : 0m;
    }

    // ─── Commands ────────────────────────────────────────────────

    [RelayCommand]
    private void AddToCart(Product product)
    {
        if (product == null) return;

        var existing = CartItems.FirstOrDefault(i => i.ProductId == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            CartItems.Add(new OrderItem
            {
                Id = Guid.NewGuid().ToString("N")[..8],
                ProductId = product.Id,
                ProductName = product.Name,
                Emoji = product.Emoji,
                Quantity = 1,
                UnitPrice = product.Price,
                TaxRate = product.TaxRate
            });
        }

        RecalculateTotals();
    }

    [RelayCommand]
    private void RemoveFromCart(OrderItem item)
    {
        if (item == null) return;
        CartItems.Remove(item);
        RecalculateTotals();
    }

    [RelayCommand]
    private void UpdateQuantity((OrderItem Item, int NewQty) args)
    {
        if (args.Item == null) return;

        if (args.NewQty <= 0)
        {
            CartItems.Remove(args.Item);
        }
        else
        {
            args.Item.Quantity = args.NewQty;
        }

        RecalculateTotals();
    }

    [RelayCommand]
    private void ClearCart()
    {
        if (CartItems.Count == 0) return;

        CartItems.Clear();
        RemoveCouponInternal();
        RecalculateTotals();

        _notificationService.ShowInfo("Cart cleared");
    }

    [RelayCommand]
    private void ApplyCoupon()
    {
        if (string.IsNullOrWhiteSpace(CouponCode))
        {
            _notificationService.ShowWarning("Please enter a coupon code");
            return;
        }

        var coupon = MockCoupons.All.FirstOrDefault(c =>
            c.Code.Equals(CouponCode.Trim(), StringComparison.OrdinalIgnoreCase));

        if (coupon == null)
        {
            _notificationService.ShowError("Invalid coupon code");
            return;
        }

        if (!coupon.IsActive)
        {
            _notificationService.ShowError("This coupon is no longer active");
            return;
        }

        if (coupon.ExpiresAt.HasValue && coupon.ExpiresAt.Value < DateTime.UtcNow)
        {
            _notificationService.ShowError("This coupon has expired");
            return;
        }

        var currentSubtotal = CartItems.Sum(i => i.UnitPrice * i.Quantity - i.DiscountAmount);
        if (currentSubtotal < coupon.MinOrderAmount)
        {
            _notificationService.ShowError($"Minimum order amount of {coupon.MinOrderAmount:F2} SAR required");
            return;
        }

        if (coupon.DiscountType == DiscountType.Percent)
        {
            CouponDiscount = currentSubtotal * coupon.Value / 100m;
        }
        else
        {
            CouponDiscount = coupon.Value;
        }

        CouponApplied = true;
        RecalculateTotals();
        _notificationService.ShowSuccess($"Coupon '{coupon.Code}' applied — {CouponDiscount:F2} SAR off");
    }

    [RelayCommand]
    private void RemoveCoupon()
    {
        RemoveCouponInternal();
        RecalculateTotals();
        _notificationService.ShowInfo("Coupon removed");
    }

    private void RemoveCouponInternal()
    {
        CouponCode = "";
        CouponApplied = false;
        CouponDiscount = 0m;
    }

    [RelayCommand]
    private void NewTab()
    {
        if (OrderTabs.Count >= 10)
        {
            _notificationService.ShowWarning("Maximum 10 order tabs allowed");
            return;
        }

        var tab = new OrderTab
        {
            TabIndex = OrderTabs.Count,
            Name = $"Order #{OrderTabs.Count + 1}"
        };
        OrderTabs.Add(tab);
        CurrentOrderTab = tab.TabIndex;

        _notificationService.ShowInfo($"New tab '{tab.Name}' created");
    }

    [RelayCommand]
    private void CloseTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex >= OrderTabs.Count) return;

        var tab = OrderTabs[tabIndex];
        if (tab.Items.Count > 0)
        {
            _notificationService.ShowWarning($"Tab '{tab.Name}' has items — clear cart first or complete the order");
            return;
        }

        if (OrderTabs.Count == 1)
        {
            _notificationService.ShowWarning("Cannot close the last tab");
            return;
        }

        OrderTabs.RemoveAt(tabIndex);

        // Re-index remaining tabs
        for (int i = 0; i < OrderTabs.Count; i++)
        {
            OrderTabs[i].TabIndex = i;
            OrderTabs[i].Name = $"Order #{i + 1}";
        }

        CurrentOrderTab = Math.Min(tabIndex, OrderTabs.Count - 1);
    }

    [RelayCommand]
    private void SwitchTab(int tabIndex)
    {
        if (tabIndex >= 0 && tabIndex < OrderTabs.Count)
        {
            CurrentOrderTab = tabIndex;
        }
    }

    [RelayCommand]
    private void OpenPayment()
    {
        if (!HasItems)
        {
            _notificationService.ShowWarning("Cart is empty");
            return;
        }

        AmountTendered = 0m;
        ChangeAmount = 0m;
        IsPaymentDialogOpen = true;
    }

    [RelayCommand]
    private void CompleteOrder()
    {
        if (!HasItems)
        {
            _notificationService.ShowWarning("Cart is empty");
            return;
        }

        if (SelectedPaymentMethod == PaymentMethod.Cash && AmountTendered < GrandTotal)
        {
            _notificationService.ShowError("Insufficient amount tendered");
            return;
        }

        var session = _authService.CurrentSession;
        if (session == null)
        {
            _notificationService.ShowError("No active session");
            return;
        }

        _orderCounter++;
        var orderNumber = $"POS-{_orderCounter:D4}";

        var order = new Order
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            Number = orderNumber,
            BranchId = session.BranchId,
            EmployeeId = session.EmployeeId,
            EmployeeName = session.EmployeeName,
            CustomerId = SelectedCustomer?.Id ?? "",
            CustomerName = SelectedCustomer?.Name ?? "Walk-in Customer",
            Items = CartItems.ToList(),
            Status = OrderStatus.Completed,
            PaymentMethod = SelectedPaymentMethod,
            OrderType = SelectedOrderType,
            Subtotal = Subtotal,
            DiscountAmount = DiscountAmount,
            CouponCode = CouponApplied ? CouponCode : null,
            TaxTotal = TaxTotal,
            GrandTotal = GrandTotal,
            AmountPaid = SelectedPaymentMethod == PaymentMethod.Cash ? AmountTendered : GrandTotal,
            ChangeAmount = ChangeAmount,
            CreatedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            TableNumber = SelectedTable
        };

        // Assign order IDs to items
        foreach (var item in order.Items)
        {
            item.OrderId = order.Id;
        }

        // Queue for sync
        SyncQueue.Enqueue(new SyncRecord
        {
            Id = Guid.NewGuid().ToString("N"),
            Type = SyncType.Upsert,
            Entity = "Order",
            EntityId = order.Id,
            Payload = JsonSerializer.Serialize(order),
            Timestamp = DateTime.UtcNow,
            DeviceId = HardwareFingerprint.Generate()
        });

        // Audit log
        _auditService.Log(
            AuditAction.Sale,
            session.EmployeeId,
            session.EmployeeName,
            session.BranchId,
            $"Order {orderNumber} completed — {GrandTotal:F2} SAR via {SelectedPaymentMethod}");

        // Reset cart state
        CartItems.Clear();
        RemoveCouponInternal();
        SelectedCustomer = null;
        SelectedTable = null;
        AmountTendered = 0m;
        IsPaymentDialogOpen = false;
        RecalculateTotals();

        _notificationService.ShowSuccess($"Order {orderNumber} completed — {order.GrandTotal:F2} SAR");
    }

    [RelayCommand]
    private void HoldOrder()
    {
        if (!HasItems)
        {
            _notificationService.ShowWarning("Cart is empty — nothing to hold");
            return;
        }

        var session = _authService.CurrentSession;
        if (session == null) return;

        _orderCounter++;
        var orderNumber = $"POS-{_orderCounter:D4}";

        var order = new Order
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            Number = orderNumber,
            BranchId = session.BranchId,
            EmployeeId = session.EmployeeId,
            EmployeeName = session.EmployeeName,
            CustomerId = SelectedCustomer?.Id ?? "",
            CustomerName = SelectedCustomer?.Name ?? "Walk-in Customer",
            Items = CartItems.ToList(),
            Status = OrderStatus.OnHold,
            OrderType = SelectedOrderType,
            Subtotal = Subtotal,
            DiscountAmount = DiscountAmount,
            CouponCode = CouponApplied ? CouponCode : null,
            TaxTotal = TaxTotal,
            GrandTotal = GrandTotal,
            CreatedAt = DateTime.UtcNow,
            TableNumber = SelectedTable
        };

        foreach (var item in order.Items)
        {
            item.OrderId = order.Id;
        }

        _auditService.Log(
            AuditAction.HoldOrder,
            session.EmployeeId,
            session.EmployeeName,
            session.BranchId,
            $"Order {orderNumber} placed on hold — {GrandTotal:F2} SAR");

        CartItems.Clear();
        RemoveCouponInternal();
        RecalculateTotals();

        _notificationService.ShowInfo($"Order {orderNumber} placed on hold");
    }

    [RelayCommand]
    private void VoidOrder()
    {
        if (!_authService.HasPermission("pos.void"))
        {
            _notificationService.ShowError("Permission denied — manager authorization required to void orders");
            return;
        }

        if (!HasItems)
        {
            _notificationService.ShowWarning("Cart is empty — nothing to void");
            return;
        }

        var session = _authService.CurrentSession;
        if (session == null) return;

        _auditService.Log(
            AuditAction.Void,
            session.EmployeeId,
            session.EmployeeName,
            session.BranchId,
            $"Current cart voided — {GrandTotal:F2} SAR");

        CartItems.Clear();
        RemoveCouponInternal();
        RecalculateTotals();

        _notificationService.ShowWarning("Order voided");
    }

    [RelayCommand]
    private void SelectCategory(string category)
    {
        SelectedCategory = category ?? "All";
    }

    [RelayCommand]
    private void SearchProducts()
    {
        ApplyProductFilters();
    }
}
