using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Table
{
    public string Id { get; set; } = "";
    public int Number { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; }
    public TableShape Shape { get; set; }
    public TableSection Section { get; set; }
    public int Floor { get; set; } = 1;
    public string? CurrentOrderId { get; set; }
    public string? WaiterName { get; set; }
}
