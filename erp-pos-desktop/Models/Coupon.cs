using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Coupon
{
    public string Code { get; set; } = "";
    public DiscountType DiscountType { get; set; }
    public decimal Value { get; set; }
    public decimal MinOrderAmount { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime? ExpiresAt { get; set; }
}
