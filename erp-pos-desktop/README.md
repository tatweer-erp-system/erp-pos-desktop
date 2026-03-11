# 🏪 Tatweer POS — Desktop Point of Sale System

<div align="center">

![Tatweer POS](https://img.shields.io/badge/Tatweer-POS-6366F1?style=for-the-badge&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-Windows-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![License](https://img.shields.io/badge/License-Proprietary-EF4444?style=for-the-badge)

**A modern, feature-rich desktop POS system built with WPF + .NET 8**
*Supports Retail, Restaurant, Pharmacy & Supermarket businesses*

</div>

---

## ✨ Design Philosophy

Tatweer POS is built with a **dark-first, modern aesthetic** using:
- `MaterialDesignInXAML` — Material Design 3 components & ripple effects
- `HandyControl` — Extended modern controls, drawer panels, blur effects
- `LiveCharts2` — Animated, real-time charts for analytics
- Full **Dark & Light theme** switching with smooth crossfade animation
- **EN / AR bilingual** support with full RTL layout switching
- Smooth **WPF Storyboard animations** throughout — entrances, transitions, micro-interactions

---

## 🖥️ System Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| OS | Windows 10 (1903+) | Windows 11 |
| RAM | 4 GB | 8 GB |
| Storage | 500 MB | 2 GB |
| Display | 1280×720 | 1920×1080 |
| .NET Runtime | .NET 8 Desktop | .NET 8 Desktop |

---

## 🚀 Getting Started

### Prerequisites
```bash
# Install .NET 8 SDK
https://dotnet.microsoft.com/download/dotnet/8.0

# Install Visual Studio 2022 (Community is free)
# Workload required: .NET Desktop Development
```

### Installation
```bash
# Clone the repository
git clone https://github.com/your-org/tatweer-pos.git
cd tatweer-pos

# Restore NuGet packages
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project TatweerPOS
```

### First Launch
1. App generates a **machine fingerprint** on first run
2. Enter your **license key** to activate (dev key: `DEV-0000-0000-0000`)
3. Select your **branch** from the login screen
4. Login with default credentials (see below)

---

## 🔑 Default Credentials (Mock / Dev Mode)

| Role | Email | Password | PIN |
|------|-------|----------|-----|
| Admin | admin@tatweer.com | Admin@123 | 0000 |
| Manager | manager@tatweer.com | Manager@123 | 0000 |
| Supervisor | supervisor@tatweer.com | Super@123 | 0000 |
| Cashier | cashier@tatweer.com | Cash@123 | 0000 |
| Pharmacist | pharma@tatweer.com | Pharma@123 | 0000 |

> ⚠️ **Change all default passwords before production deployment.**

---

## 📦 NuGet Dependencies

```xml
<PackageReference Include="MaterialDesignThemes"              Version="5.1.0" />
<PackageReference Include="MaterialDesignColors"              Version="3.1.0" />
<PackageReference Include="HandyControl"                      Version="3.5.1" />
<PackageReference Include="LiveChartsCore.SkiaSharpView.WPF" Version="2.0.0" />
<PackageReference Include="CommunityToolkit.Mvvm"             Version="8.3.2" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
<PackageReference Include="Newtonsoft.Json"                   Version="13.0.3" />
<PackageReference Include="ESCPOS.Net"                        Version="3.0.0" />
<PackageReference Include="PdfSharp"                          Version="6.0.0" />
<PackageReference Include="Serilog"                           Version="3.1.1" />
<PackageReference Include="Serilog.Sinks.File"                Version="5.0.0" />
```

---

## 🗂️ Project Structure

```
TatweerPOS/
│
├── 📁 Assets/
│   ├── Fonts/                    # Embedded fonts
│   ├── Icons/                    # SVG & PNG icons
│   └── Images/                   # Logo, placeholder images
│
├── 📁 Components/                # Reusable UserControls
│   ├── ThemeToggleButton.xaml
│   ├── LanguageToggle.xaml
│   ├── SyncStatusIndicator.xaml
│   ├── PinLockOverlay.xaml
│   └── LoadingSkeleton.xaml
│
├── 📁 Converters/                # XAML Value Converters
│   ├── BoolToVisibilityConverter.cs
│   ├── ThemeToIconConverter.cs
│   ├── StockStatusConverter.cs
│   └── CurrencyFormatConverter.cs
│
├── 📁 Data/                      # Mock data & repositories
│   ├── MockEmployees.cs
│   ├── MockProducts.cs
│   ├── MockCustomers.cs
│   ├── MockOrders.cs
│   ├── MockBranches.cs
│   └── MockSuppliers.cs
│
├── 📁 Helpers/
│   ├── AnimationHelper.cs        # Storyboard factory methods
│   ├── HardwareFingerprint.cs    # Machine ID generation
│   ├── LicenseManager.cs        # License validation
│   ├── CacheManager.cs          # Cache with TTL & versioning
│   ├── SyncQueue.cs             # Offline sync queue
│   └── PrintHelper.cs           # ESC/POS + PDF printing
│
├── 📁 Models/
│   ├── Employee.cs
│   ├── Product.cs
│   ├── ProductVariant.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── Customer.cs
│   ├── Branch.cs
│   ├── Supplier.cs
│   ├── Table.cs
│   ├── Shift.cs
│   ├── AuditLog.cs
│   ├── LicenseInfo.cs
│   └── SyncRecord.cs
│
├── 📁 Resources/
│   ├── Themes/
│   │   ├── DarkTheme.xaml        # Full dark color palette
│   │   ├── LightTheme.xaml       # Full light color palette
│   │   └── ThemeManager.cs       # Runtime theme switching
│   ├── Colors.xaml               # Semantic color tokens
│   ├── Styles.xaml               # Global control styles
│   ├── Animations.xaml           # Reusable storyboards
│   └── Strings.xaml              # EN/AR localization strings
│
├── 📁 Services/
│   ├── AuthService.cs            # Login, session, PIN lock
│   ├── LicenseService.cs         # Hardware fingerprint & activation
│   ├── SyncService.cs            # Cloud sync with conflict resolution
│   ├── PrintService.cs           # ESC/POS + Windows + PDF printing
│   ├── CacheService.cs           # Versioned cache with TTL
│   ├── AuditService.cs           # Action logging
│   └── NotificationService.cs   # Toast/snackbar notifications
│
├── 📁 ViewModels/
│   ├── LoginViewModel.cs
│   ├── MainViewModel.cs
│   ├── POSViewModel.cs
│   ├── InventoryViewModel.cs
│   ├── CustomersViewModel.cs
│   ├── OrdersViewModel.cs
│   ├── TablesViewModel.cs
│   ├── KitchenViewModel.cs
│   ├── EmployeesViewModel.cs
│   ├── SuppliersViewModel.cs
│   ├── ReportsViewModel.cs
│   └── SettingsViewModel.cs
│
├── 📁 Views/
│   ├── LoginWindow.xaml
│   ├── MainWindow.xaml           # App shell + sidebar
│   └── Pages/
│       ├── POSPage.xaml
│       ├── InventoryPage.xaml
│       ├── CustomersPage.xaml
│       ├── OrdersPage.xaml
│       ├── TablesPage.xaml
│       ├── KitchenPage.xaml
│       ├── EmployeesPage.xaml
│       ├── SuppliersPage.xaml
│       ├── ReportsPage.xaml
│       └── SettingsPage.xaml
│
├── App.xaml
├── App.xaml.cs
└── TatweerPOS.csproj
```

---

## 🎨 Theme System

### Switching Themes
```csharp
// Toggle between dark and light
ThemeManager.Toggle();

// Apply specific theme
ThemeManager.Apply(AppTheme.Dark);
ThemeManager.Apply(AppTheme.Light);

// Get current theme
var current = ThemeManager.CurrentTheme; // AppTheme.Dark or AppTheme.Light
```

### Color Tokens (always use these, never hardcode hex)
```xml
<!-- Backgrounds -->
{StaticResource WindowBg}
{StaticResource CardBg}
{StaticResource InputBg}

<!-- Text -->
{StaticResource TextPrimary}
{StaticResource TextSecondary}
{StaticResource TextMuted}

<!-- Accents -->
{StaticResource PrimaryBrush}      <!-- #6366F1 Indigo -->
{StaticResource SuccessBrush}      <!-- #10B981 Green -->
{StaticResource ErrorBrush}        <!-- #EF4444 Red -->
{StaticResource WarningBrush}      <!-- #F59E0B Amber -->
```

---

## 🖨️ Printing

### Receipt Printing
```csharp
// ESC/POS thermal printer
await PrintService.PrintReceiptAsync(order, printer: "EPSON TM-T88");

// Windows printer (any installed printer)
await PrintService.PrintReceiptWindowsAsync(order);

// PDF export fallback
await PrintService.ExportReceiptPdfAsync(order, path: "receipt.pdf");
```

### Barcode Label Printing
```csharp
await PrintService.PrintBarcodeLabelAsync(product, quantity: 10);
```

---

## 🔒 License & Copy Protection

Tatweer POS uses **hardware fingerprint-based licensing**:

1. On first launch → app collects: CPU ID + Motherboard Serial + Primary MAC address
2. Generates a unique **machine fingerprint hash**
3. Admin enters a **license key** tied to that fingerprint
4. License validated locally on every launch
5. Copying app to another machine → fingerprint mismatch → blocked

```csharp
// Get machine fingerprint (for support/activation)
var fingerprint = HardwareFingerprint.Generate();
// Example: "A3F2-9D1C-4B8E-7F20"

// Validate license
var result = LicenseManager.Validate(licenseKey, fingerprint);
// result.IsValid, result.ExpiresAt, result.BranchName
```

> 📧 To generate a license key: contact support@tatweer.com with your machine fingerprint.

---

## 🔄 Sync & Offline Mode

### How Sync Works
- All changes made offline are stored in a **local sync queue**
- When internet is restored → queue replays in order
- **Conflict resolution**: server timestamp wins (last-write-wins)
- Conflicts are logged to audit trail for review
- Sync status always visible in top bar (🟢 synced / 🟡 pending / 🔴 error)

### Sync Status
```csharp
// Check sync status
var status = SyncService.GetStatus();
// status.PendingChanges, status.LastSync, status.IsOnline

// Force manual sync
await SyncService.SyncNowAsync();

// Clear sync queue (dangerous — use only if advised by support)
SyncService.ClearQueue();
```

### Cache Management
- All cached data has a **version stamp** + **TTL**
- Stale cache auto-invalidated on app start
- Manual cache clear available in Settings → Advanced

---

## 📊 Supported Business Modes

| Mode | Extra Features |
|------|---------------|
| **Retail** | Standard POS, variants, barcodes |
| **Restaurant** | Tables, KDS, modifiers, recipes |
| **Pharmacy** | Expiry tracking, batch numbers, Rx notes |
| **Supermarket** | Weight-based items, self-checkout ready |

Switch business mode in **Settings → General → Business Mode**.

---

## 🌐 Localization

```xml
<!-- Switch language at runtime -->
FlowDirection="LeftToRight"   <!-- English -->
FlowDirection="RightToLeft"   <!-- Arabic -->
```

All strings are defined in `Resources/Strings.xaml` with EN/AR keys.
Never hardcode UI text — always use `{StaticResource str_KeyName}`.

---

## 📝 Audit Log

Every significant action is logged automatically:
```
[2025-03-11 14:32:01] LOGIN      Sara Ahmed (Manager) — Main Branch
[2025-03-11 14:35:22] SALE       Order #1042 — $85.10 — Cash
[2025-03-11 14:40:11] REFUND     Order #1038 — $25.00 — Sara Ahmed
[2025-03-11 14:55:00] STOCK_ADJ  Coca Cola +50 units — Omar Hassan
[2025-03-11 15:00:00] PIN_LOCK   Screen locked — Sara Ahmed
```

View audit log in **Reports → Audit Log** (Manager+ only).

---

## 🛡️ Security Best Practices

- Never store passwords in plain text (bcrypt hashed)
- PIN codes hashed before storage
- License key encrypted at rest
- Audit log is append-only (cannot be deleted by users)
- Session auto-expires after 5 min inactivity (PIN lock)
- Manager override required for: voids, refunds, discounts >20%

---

## 📞 Support

| | |
|---|---|
| 📧 Email | support@tatweer.com |
| 🌐 Website | www.tatweer.com |
| 📱 Phone | +966 XX XXX XXXX |

---

<div align="center">
© 2025 Tatweer. All rights reserved. Unauthorized copying or distribution is strictly prohibited.
</div>