namespace TatweerPOS.Models;

public class Supplier
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Contact { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string PaymentTerms { get; set; } = "";
    public int TotalOrders { get; set; }
    public DateTime LastOrderDate { get; set; }
    public string Website { get; set; } = "";
    public string Notes { get; set; } = "";
    public bool IsActive { get; set; } = true;
}
