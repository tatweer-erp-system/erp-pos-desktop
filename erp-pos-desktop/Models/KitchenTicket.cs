using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class KitchenTicket
{
    public string Id { get; set; } = "";
    public string OrderId { get; set; } = "";
    public string OrderNumber { get; set; } = "";
    public List<KitchenTicketItem> Items { get; set; } = new();
    public KitchenTicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PreparedAt { get; set; }
    public int? TableNumber { get; set; }
    public OrderType OrderType { get; set; }
}

public class KitchenTicketItem
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
