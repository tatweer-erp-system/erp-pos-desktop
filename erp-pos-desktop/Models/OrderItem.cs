using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class OrderItem
{
    public string Id { get; set; } = "";
    public string OrderId { get; set; } = "";
    public string ProductId { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string Emoji { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; } = 0;
    public DiscountType? DiscountType { get; set; }
    public decimal TaxRate { get; set; } = 15;
    public string? Notes { get; set; }
    public List<string> Modifiers { get; set; } = new();

    public decimal TaxAmount => (UnitPrice * Quantity - DiscountAmount) * TaxRate / 100;
    public decimal LineTotal => UnitPrice * Quantity - DiscountAmount + TaxAmount;
}
