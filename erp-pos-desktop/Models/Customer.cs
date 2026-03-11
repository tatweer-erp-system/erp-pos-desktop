using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Customer
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public CustomerTier Tier { get; set; }
    public int LoyaltyPoints { get; set; }
    public decimal TotalSpent { get; set; }
    public int TotalVisits { get; set; }
    public DateTime JoinDate { get; set; }
    public string Notes { get; set; } = "";
    public bool IsActive { get; set; } = true;
}
