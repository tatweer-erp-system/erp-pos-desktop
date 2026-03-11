namespace TatweerPOS.Data;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

public static class MockTables
{
    public static List<Table> All => new()
    {
        // ═══════════════════════════════════════════════════
        // FLOOR 1 — Indoor (Tables 1-8)
        // ═══════════════════════════════════════════════════

        new Table { Id = "T01", Number = 1,  Capacity = 2, Status = TableStatus.Available,  Shape = TableShape.Square,    Section = TableSection.Indoor, Floor = 1 },
        new Table { Id = "T02", Number = 2,  Capacity = 2, Status = TableStatus.Occupied,   Shape = TableShape.Square,    Section = TableSection.Indoor, Floor = 1, CurrentOrderId = "ORD-031", WaiterName = "Yusuf Ali" },
        new Table { Id = "T03", Number = 3,  Capacity = 4, Status = TableStatus.Occupied,   Shape = TableShape.Rectangle, Section = TableSection.Indoor, Floor = 1, CurrentOrderId = "ORD-041", WaiterName = "Yusuf Ali" },
        new Table { Id = "T04", Number = 4,  Capacity = 4, Status = TableStatus.Available,  Shape = TableShape.Rectangle, Section = TableSection.Indoor, Floor = 1 },
        new Table { Id = "T05", Number = 5,  Capacity = 6, Status = TableStatus.Reserved,   Shape = TableShape.Rectangle, Section = TableSection.Indoor, Floor = 1, WaiterName = "Khalid Nasser" },
        new Table { Id = "T06", Number = 6,  Capacity = 4, Status = TableStatus.Occupied,   Shape = TableShape.Round,     Section = TableSection.Indoor, Floor = 1, CurrentOrderId = "ORD-046", WaiterName = "Khalid Nasser" },
        new Table { Id = "T07", Number = 7,  Capacity = 8, Status = TableStatus.Available,  Shape = TableShape.Rectangle, Section = TableSection.Indoor, Floor = 1 },
        new Table { Id = "T08", Number = 8,  Capacity = 6, Status = TableStatus.Cleaning,   Shape = TableShape.Rectangle, Section = TableSection.Indoor, Floor = 1 },

        // ═══════════════════════════════════════════════════
        // FLOOR 1 — Outdoor (Tables 9-12)
        // ═══════════════════════════════════════════════════

        new Table { Id = "T09", Number = 9,  Capacity = 2, Status = TableStatus.Available,  Shape = TableShape.Round,     Section = TableSection.Outdoor, Floor = 1 },
        new Table { Id = "T10", Number = 10, Capacity = 4, Status = TableStatus.Reserved,   Shape = TableShape.Round,     Section = TableSection.Outdoor, Floor = 1 },
        new Table { Id = "T11", Number = 11, Capacity = 4, Status = TableStatus.Available,  Shape = TableShape.Square,    Section = TableSection.Outdoor, Floor = 1 },
        new Table { Id = "T12", Number = 12, Capacity = 6, Status = TableStatus.Available,  Shape = TableShape.Rectangle, Section = TableSection.Outdoor, Floor = 1 },

        // ═══════════════════════════════════════════════════
        // FLOOR 2 — VIP (Tables 13-16)
        // ═══════════════════════════════════════════════════

        new Table { Id = "T13", Number = 13, Capacity = 4,  Status = TableStatus.Available,  Shape = TableShape.Round,     Section = TableSection.VIP, Floor = 2 },
        new Table { Id = "T14", Number = 14, Capacity = 6,  Status = TableStatus.Available,  Shape = TableShape.Rectangle, Section = TableSection.VIP, Floor = 2 },
        new Table { Id = "T15", Number = 15, Capacity = 8,  Status = TableStatus.Available,  Shape = TableShape.Rectangle, Section = TableSection.VIP, Floor = 2 },
        new Table { Id = "T16", Number = 16, Capacity = 8,  Status = TableStatus.Occupied,   Shape = TableShape.Rectangle, Section = TableSection.VIP, Floor = 2, CurrentOrderId = "ORD-052", WaiterName = "Reem Faisal" },

        // ═══════════════════════════════════════════════════
        // FLOOR 2 — Bar (Tables 17-20)
        // ═══════════════════════════════════════════════════

        new Table { Id = "T17", Number = 17, Capacity = 1, Status = TableStatus.Available,  Shape = TableShape.Round, Section = TableSection.Bar, Floor = 2 },
        new Table { Id = "T18", Number = 18, Capacity = 2, Status = TableStatus.Available,  Shape = TableShape.Round, Section = TableSection.Bar, Floor = 2 },
        new Table { Id = "T19", Number = 19, Capacity = 3, Status = TableStatus.Cleaning,   Shape = TableShape.Round, Section = TableSection.Bar, Floor = 2 },
        new Table { Id = "T20", Number = 20, Capacity = 2, Status = TableStatus.Available,  Shape = TableShape.Round, Section = TableSection.Bar, Floor = 2 },
    };
}
