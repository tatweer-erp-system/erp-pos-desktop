using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Order
{
    public string Id { get; set; } = "";
    public string Number { get; set; } = "";
    public string BranchId { get; set; } = "";
    public string EmployeeId { get; set; } = "";
    public string EmployeeName { get; set; } = "";
    public string CustomerId { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public OrderType OrderType { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public DiscountType? DiscountType { get; set; }
    public string? CouponCode { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal ChangeAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Notes { get; set; } = "";
    public int? TableNumber { get; set; }
}
