namespace TatweerPOS.Data;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

public static class MockCoupons
{
    public static List<Coupon> All => new()
    {
        new Coupon
        {
            Code = "SAVE10", DiscountType = DiscountType.Percent, Value = 10m,
            MinOrderAmount = 0m, IsActive = true, ExpiresAt = new DateTime(2026, 12, 31)
        },
        new Coupon
        {
            Code = "FLAT20", DiscountType = DiscountType.Flat, Value = 20m,
            MinOrderAmount = 50m, IsActive = true, ExpiresAt = new DateTime(2026, 12, 31)
        },
        new Coupon
        {
            Code = "GC-2025-001", DiscountType = DiscountType.Flat, Value = 50m,
            MinOrderAmount = 0m, IsActive = true, ExpiresAt = new DateTime(2027, 6, 30)
        },
        new Coupon
        {
            Code = "BUNDLE5", DiscountType = DiscountType.Percent, Value = 5m,
            MinOrderAmount = 100m, IsActive = true, ExpiresAt = new DateTime(2026, 9, 30)
        },
    };
}
