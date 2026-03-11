namespace TatweerPOS.Data;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

public static class MockKitchenTickets
{
    private static DateTime D(int minutesAgo)
        => DateTime.Now.AddMinutes(-minutesAgo);

    public static List<KitchenTicket> All => new()
    {
        // ═══════════════════════════════════════════════════
        // NEW  (5 tickets)
        // ═══════════════════════════════════════════════════

        new KitchenTicket
        {
            Id = "KT-001", OrderId = "ORD-031", OrderNumber = "POS-0031",
            Status = KitchenTicketStatus.New, CreatedAt = D(2), TableNumber = 2, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Classic Burger", Quantity = 1, Notes = "Medium well" },
                new KitchenTicketItem { Name = "Coca Cola", Quantity = 1 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-002", OrderId = "ORD-033", OrderNumber = "POS-0033",
            Status = KitchenTicketStatus.New, CreatedAt = D(3), TableNumber = 5, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Chicken Burger", Quantity = 2, Notes = "Extra cheese on both" },
                new KitchenTicketItem { Name = "French Fries", Quantity = 2 },
                new KitchenTicketItem { Name = "Pepsi", Quantity = 2 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-003", OrderId = "ORD-035", OrderNumber = "POS-0035",
            Status = KitchenTicketStatus.New, CreatedAt = D(4), TableNumber = 4, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Pancakes", Quantity = 1, Notes = "Extra maple syrup" },
                new KitchenTicketItem { Name = "Arabic Coffee", Quantity = 1 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-004", OrderId = "ORD-037", OrderNumber = "POS-0037",
            Status = KitchenTicketStatus.New, CreatedAt = D(1), TableNumber = 9, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Margherita Pizza", Quantity = 1, Notes = "Light on the cheese" },
                new KitchenTicketItem { Name = "Water 500ml", Quantity = 2 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-005", OrderId = "ORD-039", OrderNumber = "POS-0039",
            Status = KitchenTicketStatus.New, CreatedAt = D(1), OrderType = OrderType.Takeaway,
            Items = new()
            {
                new KitchenTicketItem { Name = "Cheese Burger", Quantity = 1, Notes = "No pickles" },
                new KitchenTicketItem { Name = "Chicken Nuggets", Quantity = 1 },
            }
        },

        // ═══════════════════════════════════════════════════
        // PREPARING  (4 tickets)
        // ═══════════════════════════════════════════════════

        new KitchenTicket
        {
            Id = "KT-006", OrderId = "ORD-041", OrderNumber = "POS-0041",
            Status = KitchenTicketStatus.Preparing, CreatedAt = D(12), TableNumber = 3, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Double Burger", Quantity = 1, Notes = "Well done" },
                new KitchenTicketItem { Name = "French Fries", Quantity = 1, Notes = "Extra crispy" },
                new KitchenTicketItem { Name = "Coca Cola", Quantity = 1 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-007", OrderId = "ORD-042", OrderNumber = "POS-0042",
            Status = KitchenTicketStatus.Preparing, CreatedAt = D(10), OrderType = OrderType.Takeaway,
            Items = new()
            {
                new KitchenTicketItem { Name = "BBQ Chicken Pizza", Quantity = 1, Notes = "Extra BBQ sauce" },
                new KitchenTicketItem { Name = "Orange Juice", Quantity = 1 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-008", OrderId = "ORD-043", OrderNumber = "POS-0043",
            Status = KitchenTicketStatus.Preparing, CreatedAt = D(15), TableNumber = 7, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Veggie Burger", Quantity = 2, Notes = "No onions" },
                new KitchenTicketItem { Name = "Mint Tea", Quantity = 2 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-009", OrderId = "ORD-046", OrderNumber = "POS-0046",
            Status = KitchenTicketStatus.Preparing, CreatedAt = D(8), TableNumber = 6, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Pepperoni Pizza", Quantity = 1 },
                new KitchenTicketItem { Name = "Chocolate Cake", Quantity = 1, Notes = "Add whipped cream" },
                new KitchenTicketItem { Name = "Arabic Coffee", Quantity = 1 },
            }
        },

        // ═══════════════════════════════════════════════════
        // READY  (3 tickets)
        // ═══════════════════════════════════════════════════

        new KitchenTicket
        {
            Id = "KT-010", OrderId = "ORD-045", OrderNumber = "POS-0045",
            Status = KitchenTicketStatus.Ready, CreatedAt = D(25), PreparedAt = D(5), OrderType = OrderType.Takeaway,
            Items = new()
            {
                new KitchenTicketItem { Name = "Classic Burger", Quantity = 2 },
                new KitchenTicketItem { Name = "Coca Cola", Quantity = 2 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-011", OrderId = "ORD-047", OrderNumber = "POS-0047",
            Status = KitchenTicketStatus.Ready, CreatedAt = D(30), PreparedAt = D(8), TableNumber = 11, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Eggs Benedict", Quantity = 1 },
                new KitchenTicketItem { Name = "Mint Tea", Quantity = 1 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-012", OrderId = "ORD-048", OrderNumber = "POS-0048",
            Status = KitchenTicketStatus.Ready, CreatedAt = D(35), PreparedAt = D(10), TableNumber = 8, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Margherita Pizza", Quantity = 2 },
                new KitchenTicketItem { Name = "Chicken Nuggets", Quantity = 1, Notes = "With honey mustard" },
                new KitchenTicketItem { Name = "Pepsi", Quantity = 2 },
            }
        },

        // ═══════════════════════════════════════════════════
        // SERVED  (3 tickets)
        // ═══════════════════════════════════════════════════

        new KitchenTicket
        {
            Id = "KT-013", OrderId = "ORD-027", OrderNumber = "POS-0027",
            Status = KitchenTicketStatus.Served, CreatedAt = D(120), PreparedAt = D(100), TableNumber = 1, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "BBQ Chicken Pizza", Quantity = 1 },
                new KitchenTicketItem { Name = "Pepsi", Quantity = 1 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-014", OrderId = "ORD-028", OrderNumber = "POS-0028",
            Status = KitchenTicketStatus.Served, CreatedAt = D(180), PreparedAt = D(155), TableNumber = 16, OrderType = OrderType.DineIn,
            Items = new()
            {
                new KitchenTicketItem { Name = "Cheese Burger", Quantity = 1 },
                new KitchenTicketItem { Name = "Chicken Burger", Quantity = 1 },
                new KitchenTicketItem { Name = "French Fries", Quantity = 2 },
                new KitchenTicketItem { Name = "Chocolate Cake", Quantity = 1 },
                new KitchenTicketItem { Name = "Arabic Coffee", Quantity = 2 },
            }
        },
        new KitchenTicket
        {
            Id = "KT-015", OrderId = "ORD-029", OrderNumber = "POS-0029",
            Status = KitchenTicketStatus.Served, CreatedAt = D(90), PreparedAt = D(75), OrderType = OrderType.Takeaway,
            Items = new()
            {
                new KitchenTicketItem { Name = "Classic Burger", Quantity = 1 },
                new KitchenTicketItem { Name = "French Fries", Quantity = 1, Notes = "Extra salt" },
            }
        },
    };
}
