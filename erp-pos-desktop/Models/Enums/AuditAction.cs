namespace TatweerPOS.Models.Enums;

public enum AuditAction
{
    Login,
    Logout,
    PinLock,
    PinUnlock,
    Sale,
    Refund,
    Void,
    HoldOrder,
    StockAdjustment,
    ProductEdit,
    EmployeeCreate,
    EmployeeEdit,
    SettingsChange,
    DiscountOverride,
    LicenseValidation,
    LicenseFailure,
    SyncSuccess,
    SyncFailure,
    SyncConflict,
    CacheClear,
    BackupCreate,
    BackupRestore
}
