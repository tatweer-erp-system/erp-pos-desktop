# CLAUDE.md — Tatweer POS Development Guide

> This file tells Claude Code everything it needs to know about this project.
> Read this file completely before writing any code.

---

## 🧠 Project Identity

**App Name**: Tatweer POS
**Type**: Desktop Point of Sale — WPF + .NET 8
**Platform**: Windows 10/11 only
**Architecture**: MVVM (CommunityToolkit.Mvvm)
**Language**: C# 12 + XAML
**Supported Locales**: English (LTR) + Arabic (RTL)
**Business Modes**: Retail, Restaurant/Café, Pharmacy, Supermarket

---

## 🎨 Design System — CRITICAL RULES

### Rule #1 — Never Use Default WPF Styling
This app uses **MaterialDesignInXAML + HandyControl** for ALL controls.
Never use plain WPF Button, TextBox, ComboBox without applying a style.
Every control must look modern, polished, and intentional.

### Rule #2 — Never Hardcode Colors
```xml
<!-- ❌ WRONG — never do this -->
<Border Background="#111827">
<TextBlock Foreground="#F8FAFC">

<!-- ✅ CORRECT — always use resource keys -->
<Border Background="{StaticResource CardBg}">
<TextBlock Foreground="{StaticResource TextPrimary}">
```

### Rule #3 — Never Hardcode Strings
```xml
<!-- ❌ WRONG -->
<TextBlock Text="Point of Sale"/>

<!-- ✅ CORRECT -->
<TextBlock Text="{StaticResource str_PointOfSale}"/>
```

### Rule #4 — Every Screen Has Entrance Animations
Every page/window must animate in on load using staggered entrance:
- Elements slide up + fade in sequentially
- Each element delayed by 60-80ms from the previous
- Use `AnimationHelper.StaggeredEntrance(elements)` helper

### Rule #5 — Every Interaction Has Feedback
- Buttons: ripple (MaterialDesign) + hover glow + press scale
- Inputs: border color animates on focus + icon color changes
- Cards: lift on hover (shadow + translateY -2px)
- Destructive actions: shake animation on error

---

## 🎨 Color Palette

### Dark Theme
```
WindowBg:          #080A12   (deepest background)
SurfaceBg:         #0F1219   (slightly lighter)
CardBg:            #111827   (cards & panels)
CardHoverBg:       #1A2235   (card hover state)
InputBg:           #0F1523   (form inputs)
InputBorder:       #1E2D45   (input default border)
InputBorderFocus:  #6366F1   (input focused border)
DividerBrush:      #1A2236   (separators)
SidebarBg:         #080A12
TopBarBg:          #0F1219
```

### Light Theme
```
WindowBg:          #F1F5F9
SurfaceBg:         #FFFFFF
CardBg:            #FFFFFF
CardHoverBg:       #F8FAFF
InputBg:           #FFFFFF
InputBorder:       #CBD5E1
InputBorderFocus:  #6366F1
DividerBrush:      #E2E8F0
SidebarBg:         #1E1B4B   (stays dark in light mode)
TopBarBg:          #FFFFFF
```

### Semantic Colors (same in both themes)
```
PrimaryBrush:      #6366F1   (Indigo — main accent)
PrimaryDarkBrush:  #4F46E5
TealAccent:        #0EA5E9
SuccessBrush:      #10B981
ErrorBrush:        #EF4444
WarningBrush:      #F59E0B
InfoBrush:         #0EA5E9
TextPrimary:       #F8FAFC  / #0F172A
TextSecondary:     #94A3B8  / #475569
TextMuted:         #475569  / #94A3B8
```

### Gradients
```
PrimaryGradient:   #6366F1 → #8B5CF6 → #0EA5E9  (diagonal)
```

---

## 📦 Tech Stack & Libraries

```xml
MaterialDesignThemes        5.1.0    <!-- Core UI components -->
MaterialDesignColors         3.1.0    <!-- Color system -->
HandyControl                3.5.1    <!-- Extended controls -->
LiveChartsCore.SkiaSharpView.WPF 2.0.0 <!-- Charts -->
CommunityToolkit.Mvvm       8.3.2    <!-- MVVM framework -->
Microsoft.Extensions.DependencyInjection 8.0.0
Newtonsoft.Json             13.0.3   <!-- Serialization -->
ESCPOS.Net                  3.0.0    <!-- Thermal printing -->
PdfSharp                    6.0.0    <!-- PDF export -->
Serilog + Serilog.Sinks.File         <!-- Logging -->
```

---

## 🏗️ Architecture Rules

### ViewModel Pattern
```csharp
// ✅ Always use CommunityToolkit.Mvvm source generators
public partial class ExampleViewModel : ObservableObject
{
    [ObservableProperty] private string title = "";
    [ObservableProperty] private bool isLoading = false;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsLoading = true;
        // ... work
        IsLoading = false;
    }
}
```

### Dependency Injection
```csharp
// Register in App.xaml.cs
services.AddSingleton<AuthService>();
services.AddSingleton<SyncService>();
services.AddSingleton<ThemeManager>();
services.AddTransient<POSViewModel>();
```

### Navigation
- Use a `NavigationService` that swaps `ContentControl` in `MainWindow`
- Never instantiate ViewModels directly in Views
- Always resolve via DI container

---

## 🔐 Authentication & Security

### Login Flow
1. User enters email + password + selects branch
2. `AuthService.LoginAsync(email, password, branchId)`
3. Mock validation against `MockData.Employees`
4. On success → store `CurrentSession` in `AuthService`
5. Navigate to `MainWindow`, close `LoginWindow`

### PIN Lock System
- Auto-lock after **5 minutes** of inactivity (no mouse/keyboard events)
- Manual lock via lock button in top bar or sidebar
- PIN lock is an **overlay** on top of MainWindow (not a new window)
- Correct PIN (0000 for all users in mock) → dismiss overlay
- Wrong PIN → shake animation + increment fail counter
- 3 wrong PINs → show "Contact manager" + lock for 30 seconds

### Mock Credentials
```csharp
// ALL users have PIN: 0000
public static class MockCredentials
{
    public static readonly List<MockUser> Users = new()
    {
        new("admin@tatweer.com",      "Admin@123",   Role.Admin,        pin: "0000"),
        new("manager@tatweer.com",    "Manager@123", Role.Manager,      pin: "0000"),
        new("supervisor@tatweer.com", "Super@123",   Role.Supervisor,   pin: "0000"),
        new("cashier@tatweer.com",    "Cash@123",    Role.Cashier,      pin: "0000"),
        new("pharma@tatweer.com",     "Pharma@123",  Role.Pharmacist,   pin: "0000"),
    };
}
```

### Role Permissions Matrix
```
Page/Action              Admin  Manager  Supervisor  Cashier  Pharmacist
─────────────────────────────────────────────────────────────────────────
POS - Sell               ✅     ✅        ✅          ✅        ✅
POS - Discount >20%      ✅     ✅        ⚠️override  ❌        ❌
POS - Void Order         ✅     ✅        ❌          ❌        ❌
POS - Refund             ✅     ✅        ✅          ❌        ❌
Orders - View            ✅     ✅        ✅          ✅        ✅
Inventory - View         ✅     ✅        ✅          ✅        ✅
Inventory - Edit         ✅     ✅        ✅          ❌        ✅
Customers - View         ✅     ✅        ✅          ✅        ✅
Customers - Edit         ✅     ✅        ✅          ❌        ❌
Employees - View         ✅     ✅        ❌          ❌        ❌
Employees - Edit         ✅     ✅        ❌          ❌        ❌
Reports                  ✅     ✅        ✅          ❌        ❌
Settings                 ✅     ✅        ❌          ❌        ❌
Audit Log                ✅     ✅        ❌          ❌        ❌
⚠️ = requires manager override PIN
```

---

## 🛒 POS Screen Rules

### Multi-Order Tabs
- Max 10 simultaneous open orders
- Each tab = independent cart state
- Switching tabs saves current + loads selected
- Tab shows: "Order #N  $XX.XX"
- Closing tab → confirm dialog if cart not empty

### Cart Calculation Order
```
1. Sum all line items (qty × unit_price)
2. Apply line-level discounts
3. Apply order-level discount (% or flat)
4. Apply coupon/voucher if valid
5. Calculate tax per product tax group
6. Grand Total = subtotal - discounts + tax
```

### Mock Coupons
```csharp
public static class MockCoupons
{
    // Code       Type       Value    MinOrder
    // SAVE10     Percent    10%      $0
    // FLAT20     Flat       $20      $50
    // GC-2025-001 GiftCard  $50      $0
    // BUNDLE5    Percent    5%       $100
}
```

### Barcode Simulation
- Typing 8+ consecutive digits in search → treat as barcode scan
- Find matching product → add to cart → show toast "Added via barcode"
- If not found → show error toast "Product not found"

---

## 🖨️ Printing System

### Priority Order
1. ESC/POS thermal printer (if configured)
2. Windows default printer (fallback)
3. PDF export (always available as last resort)

### Receipt Format
```
════════════════════════
     TATWEER POS
   [Branch Name]
   [Branch Address]
   VAT: [Tax Number]
════════════════════════
Order: #[Number]
Date:  [DD/MM/YYYY HH:MM]
Cashier: [Name]
────────────────────────
[Item Name]
  [Qty] × $[Price]     $[Total]
────────────────────────
Subtotal:         $XX.XX
Discount:        -$XX.XX
Tax (15%):        $XX.XX
────────────────────────
TOTAL:            $XX.XX
Cash:             $XX.XX
Change:           $XX.XX
════════════════════════
  Thank you for visiting!
  [Footer text from settings]
════════════════════════
```

### Dev Mode Printing
In development (no printer connected):
- Show a **PrintPreviewWindow** with styled receipt
- "Print" button simulates success with toast notification
- Always allow PDF export

---

## 🔄 Sync & Cache System

### Cache Rules
```csharp
// Cache service usage — ALWAYS specify TTL and version
CacheService.Set("products", data, ttl: TimeSpan.FromMinutes(15), version: "v2");
var products = CacheService.Get<List<Product>>("products", version: "v2");
// Returns null if expired, version mismatch, or not found
```

### Sync Queue Rules
```csharp
// Every data mutation must go through sync queue
SyncQueue.Enqueue(new SyncOperation
{
    Type = SyncType.Upsert,
    Entity = "Order",
    EntityId = order.Id,
    Payload = JsonConvert.SerializeObject(order),
    Timestamp = DateTime.UtcNow,
    DeviceId = LicenseManager.DeviceId
});
```

### Conflict Resolution
- Server timestamp always wins
- Conflict logged to audit trail
- User notified via warning toast (not blocking)
- Conflicted record stored in `ConflictLog` for review

### Sync Status Indicator Colors
```
🟢 Green  = Fully synced
🟡 Amber  = Pending changes (syncing)
🔴 Red    = Sync error (show retry button)
⚫ Gray   = Offline mode
```

---

## 🔒 License Protection

### How It Works
```csharp
// On every app launch:
var fingerprint = HardwareFingerprint.Generate();
// Combines: CPU ID + Motherboard Serial + Primary MAC
// Output: "A3F2-9D1C-4B8E-7F20"

var result = LicenseManager.Validate(storedLicenseKey, fingerprint);
if (!result.IsValid)
{
    ShowUnauthorizedScreen();
    return;
}
```

### Dev Mode License
Use license key `DEV-0000-0000-0000` to bypass hardware check during development.
This key is only valid when `#DEBUG` preprocessor is active.

### License File Location
```
%APPDATA%\TatweerPOS\license.dat  (encrypted)
```

---

## 📝 Audit Logging

### Log Every Action
```csharp
// Always log significant actions
AuditService.Log(new AuditEntry
{
    Action = AuditAction.Sale,
    UserId = session.EmployeeId,
    BranchId = session.BranchId,
    Description = $"Order #{order.Number} — ${order.Total}",
    Timestamp = DateTime.UtcNow
});
```

### Actions to Always Log
- LOGIN, LOGOUT, PIN_LOCK, PIN_UNLOCK
- SALE, REFUND, VOID, HOLD_ORDER
- STOCK_ADJUSTMENT, PRODUCT_EDIT
- EMPLOYEE_CREATE, EMPLOYEE_EDIT
- SETTINGS_CHANGE
- DISCOUNT_OVERRIDE (manager override)
- LICENSE_VALIDATION, LICENSE_FAILURE
- SYNC_SUCCESS, SYNC_FAILURE, SYNC_CONFLICT
- CACHE_CLEAR, BACKUP_CREATE, BACKUP_RESTORE

---

## 🎯 Mock Data Guidelines

### Always Use Realistic Data
```csharp
// ✅ Good mock data
new Product { Name = "Coca Cola 330ml", Price = 2.50m, Stock = 144, Barcode = "5449000000996" }

// ❌ Bad mock data  
new Product { Name = "Product 1", Price = 10m, Stock = 100 }
```

### Mock Data Volumes
```
Employees:  8-10  (one per role)
Products:   50+   (across all categories)
Customers:  25+   (with history)
Orders:     60+   (mix of statuses)
Branches:   5     (including Main)
Suppliers:  10
Tables:     20    (restaurant layout)
```

### Product Categories by Business Mode
```
Retail:      Electronics, Clothing, Home, Beauty, Food, Other
Restaurant:  Burgers, Pizza, Drinks, Desserts, Sides, Breakfast
Pharmacy:    Medications, Vitamins, Personal Care, Baby, Medical Devices
Supermarket: Dairy, Bakery, Produce, Frozen, Beverages, Snacks
```

---

## 🌐 Localization Rules

### Adding a New String
```xml
<!-- In Resources/Strings.xaml -->
<sys:String x:Key="str_PointOfSale">Point of Sale</sys:String>
<sys:String x:Key="str_PointOfSale_AR">نقطة البيع</sys:String>
```

### RTL Layout
```csharp
// In ThemeManager or LanguageService
Application.Current.MainWindow.FlowDirection =
    currentLanguage == "AR"
        ? FlowDirection.RightToLeft
        : FlowDirection.LeftToRight;
```

---

## ⚡ Animation Guidelines

### Standard Durations
```
Micro (button press):     100ms
Fast (icon swap):         150ms
Normal (hover):           200ms
Page transition:          300ms
Entrance stagger:         60ms per element
Modal appear:             250ms
Success celebration:      600ms
```

### Easing Functions
```
UI transitions:    CubicEase Out
Entrances:         BackEase Out (slight overshoot)
Exits:             CubicEase In
Hover:             QuadraticEase Out
Shake (error):     Linear
```

### Standard Entrance Pattern
```csharp
// Apply to all page loads
AnimationHelper.StaggeredEntrance(new UIElement[]
{
    HeaderSection,      // delay: 0ms
    FiltersRow,         // delay: 60ms
    ContentGrid,        // delay: 120ms
    FooterBar,          // delay: 180ms
}, slideDistance: 30, fadeDuration: 400);
```

---

## 🚫 Things Claude Must NEVER Do

1. **Never hardcode color hex values** in XAML — use `{StaticResource}`
2. **Never hardcode UI strings** — use `{StaticResource str_*}`
3. **Never use default WPF control styling** — always apply MaterialDesign/HandyControl styles
4. **Never create a page without entrance animations**
5. **Never skip loading states** — every async operation needs a spinner/skeleton
6. **Never skip empty states** — every list needs an empty state with icon + message
7. **Never mutate data without adding to sync queue**
8. **Never skip audit logging** for significant actions
9. **Never store passwords or PINs in plain text**
10. **Never use `Thread.Sleep`** — always use `async/await`
11. **Never catch exceptions silently** — log with Serilog + show user-friendly error
12. **Never hardcode branch or employee data** in views — always bind from ViewModel

---

## ✅ Definition of Done (for each screen)

Before considering a screen complete, verify:
- [ ] Entrance animation on page load
- [ ] All strings use localization keys
- [ ] All colors use theme resource keys
- [ ] Dark theme looks correct
- [ ] Light theme looks correct
- [ ] Arabic RTL layout works
- [ ] Empty state implemented
- [ ] Loading state implemented
- [ ] Error state implemented
- [ ] All actions logged to audit trail
- [ ] Relevant actions added to sync queue
- [ ] Role-based visibility applied
- [ ] Keyboard shortcuts work (where applicable)
- [ ] Toast notifications on success/error

---

## 📁 File Naming Conventions

```
Views:        [Name]Window.xaml  or  [Name]Page.xaml
ViewModels:   [Name]ViewModel.cs
Models:       [Name].cs  (singular)
Services:     [Name]Service.cs
Helpers:      [Name]Helper.cs
Converters:   [Name]Converter.cs
Components:   [Name]Control.xaml
```

---

## 🐛 Error Handling Pattern

```csharp
try
{
    IsLoading = true;
    await SomeOperationAsync();
    NotificationService.ShowSuccess("str_OperationSuccess");
    AuditService.Log(AuditAction.Whatever, ...);
}
catch (LicenseException ex)
{
    Log.Error(ex, "License validation failed");
    NavigateTo<UnauthorizedView>();
}
catch (SyncException ex)
{
    Log.Warning(ex, "Sync failed — queued for retry");
    NotificationService.ShowWarning("str_SyncFailed");
    SyncQueue.MarkForRetry(ex.OperationId);
}
catch (Exception ex)
{
    Log.Error(ex, "Unexpected error in {Operation}", nameof(SomeOperationAsync));
    NotificationService.ShowError("str_UnexpectedError");
}
finally
{
    IsLoading = false;
}
```

---

*Last updated: March 2025 — Tatweer Development Team*