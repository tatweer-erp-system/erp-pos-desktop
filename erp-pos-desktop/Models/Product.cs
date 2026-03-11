using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Product
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Barcode { get; set; } = "";
    public string Sku { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }
    public int Stock { get; set; }
    public int MinStock { get; set; } = 5;
    public decimal TaxRate { get; set; } = 15;
    public string ImageUrl { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public BusinessMode BusinessMode { get; set; }
    public string Unit { get; set; } = "pcs";
    public DateTime? ExpiryDate { get; set; }
    public string? BatchNumber { get; set; }
    public string Emoji { get; set; } = "";
}
