namespace TatweerPOS.Data;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

public static class MockOrders
{
    private static DateTime D(int daysAgo, int hour = 12, int min = 0)
        => DateTime.Today.AddDays(-daysAgo).AddHours(hour).AddMinutes(min);

    public static List<Order> All => new()
    {
        // ═══════════════════════════════════════════════════
        // COMPLETED  (30 orders)
        // ═══════════════════════════════════════════════════

        new Order { Id = "ORD-001", Number = "POS-0001", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C001", CustomerName = "Mohammed Al-Rashidi", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 3,
            Items = new() {
                new OrderItem { Id = "OI-001", OrderId = "ORD-001", ProductId = "P001", ProductName = "Classic Burger", Emoji = "\ud83c\udf54", Quantity = 2, UnitPrice = 28m },
                new OrderItem { Id = "OI-002", OrderId = "ORD-001", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 2, UnitPrice = 12m },
                new OrderItem { Id = "OI-003", OrderId = "ORD-001", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 90m, DiscountAmount = 0m, TaxTotal = 13.50m, GrandTotal = 103.50m, AmountPaid = 110m, ChangeAmount = 6.50m, CreatedAt = D(28, 12, 30), CompletedAt = D(28, 13, 0) },

        new Order { Id = "ORD-002", Number = "POS-0002", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C002", CustomerName = "Fatima Al-Zahrani", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 5,
            Items = new() {
                new OrderItem { Id = "OI-004", OrderId = "ORD-002", ProductId = "P007", ProductName = "Pepperoni Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 40m },
                new OrderItem { Id = "OI-005", OrderId = "ORD-002", ProductId = "P012", ProductName = "Orange Juice", Emoji = "\ud83e\uddc3", Quantity = 2, UnitPrice = 12m },
            },
            Subtotal = 64m, DiscountAmount = 0m, TaxTotal = 9.60m, GrandTotal = 73.60m, AmountPaid = 73.60m, ChangeAmount = 0m, CreatedAt = D(27, 13, 15), CompletedAt = D(27, 13, 45) },

        new Order { Id = "ORD-003", Number = "POS-0003", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-006", OrderId = "ORD-003", ProductId = "P004", ProductName = "Chicken Burger", Emoji = "\ud83c\udf57", Quantity = 3, UnitPrice = 30m },
                new OrderItem { Id = "OI-007", OrderId = "ORD-003", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 3, UnitPrice = 12m },
            },
            Subtotal = 126m, DiscountAmount = 10m, DiscountType = Models.Enums.DiscountType.Flat, TaxTotal = 17.40m, GrandTotal = 133.40m, AmountPaid = 140m, ChangeAmount = 6.60m, CreatedAt = D(26, 11, 0), CompletedAt = D(26, 11, 20) },

        new Order { Id = "ORD-004", Number = "POS-0004", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C006", CustomerName = "Huda Bin Salman", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 1,
            Items = new() {
                new OrderItem { Id = "OI-008", OrderId = "ORD-004", ProductId = "P008", ProductName = "BBQ Chicken Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 45m },
                new OrderItem { Id = "OI-009", OrderId = "ORD-004", ProductId = "P016", ProductName = "Chocolate Cake", Emoji = "\ud83c\udf70", Quantity = 1, UnitPrice = 22m },
                new OrderItem { Id = "OI-010", OrderId = "ORD-004", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 2, UnitPrice = 15m },
            },
            Subtotal = 97m, DiscountAmount = 0m, TaxTotal = 14.55m, GrandTotal = 111.55m, AmountPaid = 111.55m, ChangeAmount = 0m, CreatedAt = D(25, 19, 0), CompletedAt = D(25, 19, 45) },

        new Order { Id = "ORD-005", Number = "POS-0005", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C003", CustomerName = "Abdullah Al-Qahtani", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-011", OrderId = "ORD-005", ProductId = "P006", ProductName = "Margherita Pizza", Emoji = "\ud83c\udf55", Quantity = 2, UnitPrice = 35m },
                new OrderItem { Id = "OI-012", OrderId = "ORD-005", ProductId = "P011", ProductName = "Pepsi", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 80m, DiscountAmount = 0m, TaxTotal = 12m, GrandTotal = 92m, AmountPaid = 92m, ChangeAmount = 0m, CreatedAt = D(25, 14, 0), CompletedAt = D(25, 14, 40) },

        new Order { Id = "ORD-006", Number = "POS-0006", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C004", CustomerName = "Noura Al-Harbi", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-013", OrderId = "ORD-006", ProductId = "P031", ProductName = "Paracetamol 500mg", Emoji = "\ud83d\udc8a", Quantity = 2, UnitPrice = 8m },
                new OrderItem { Id = "OI-014", OrderId = "ORD-006", ProductId = "P034", ProductName = "Vitamin C 1000mg", Emoji = "\ud83c\udf4a", Quantity = 1, UnitPrice = 18m },
            },
            Subtotal = 34m, DiscountAmount = 0m, TaxTotal = 5.10m, GrandTotal = 39.10m, AmountPaid = 40m, ChangeAmount = 0.90m, CreatedAt = D(24, 10, 30), CompletedAt = D(24, 10, 35) },

        new Order { Id = "ORD-007", Number = "POS-0007", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C008", CustomerName = "Rania Al-Shammari", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 7,
            Items = new() {
                new OrderItem { Id = "OI-015", OrderId = "ORD-007", ProductId = "P003", ProductName = "Double Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 42m },
                new OrderItem { Id = "OI-016", OrderId = "ORD-007", ProductId = "P018", ProductName = "Chicken Nuggets", Emoji = "\ud83c\udf57", Quantity = 1, UnitPrice = 18m },
                new OrderItem { Id = "OI-017", OrderId = "ORD-007", ProductId = "P015", ProductName = "Mint Tea", Emoji = "\ud83c\udf75", Quantity = 1, UnitPrice = 10m },
            },
            Subtotal = 70m, DiscountAmount = 0m, TaxTotal = 10.50m, GrandTotal = 80.50m, AmountPaid = 80.50m, ChangeAmount = 0m, CreatedAt = D(23, 13, 45), CompletedAt = D(23, 14, 15) },

        new Order { Id = "ORD-008", Number = "POS-0008", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-018", OrderId = "ORD-008", ProductId = "P021", ProductName = "Wireless Mouse", Emoji = "\ud83d\uddb1\ufe0f", Quantity = 1, UnitPrice = 45m },
                new OrderItem { Id = "OI-019", OrderId = "ORD-008", ProductId = "P022", ProductName = "USB-C Cable", Emoji = "\ud83d\udd0c", Quantity = 2, UnitPrice = 15m },
            },
            Subtotal = 75m, DiscountAmount = 0m, TaxTotal = 11.25m, GrandTotal = 86.25m, AmountPaid = 90m, ChangeAmount = 3.75m, CreatedAt = D(22, 16, 0), CompletedAt = D(22, 16, 10) },

        new Order { Id = "ORD-009", Number = "POS-0009", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C005", CustomerName = "Saad Al-Dosari", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-020", OrderId = "ORD-009", ProductId = "P041", ProductName = "Fresh Milk 1L", Emoji = "\ud83e\udd5b", Quantity = 3, UnitPrice = 7m },
                new OrderItem { Id = "OI-021", OrderId = "ORD-009", ProductId = "P044", ProductName = "White Bread Loaf", Emoji = "\ud83c\udf5e", Quantity = 2, UnitPrice = 5m },
                new OrderItem { Id = "OI-022", OrderId = "ORD-009", ProductId = "P046", ProductName = "Banana per kg", Emoji = "\ud83c\udf4c", Quantity = 2, UnitPrice = 6m },
            },
            Subtotal = 43m, DiscountAmount = 0m, TaxTotal = 6.45m, GrandTotal = 49.45m, AmountPaid = 49.45m, ChangeAmount = 0m, CreatedAt = D(21, 9, 30), CompletedAt = D(21, 10, 0) },

        new Order { Id = "ORD-010", Number = "POS-0010", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C012", CustomerName = "Lama Al-Subaie", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 13,
            Items = new() {
                new OrderItem { Id = "OI-023", OrderId = "ORD-010", ProductId = "P009", ProductName = "Four Cheese Pizza", Emoji = "\ud83e\uddc0", Quantity = 1, UnitPrice = 42m },
                new OrderItem { Id = "OI-024", OrderId = "ORD-010", ProductId = "P019", ProductName = "Pancakes", Emoji = "\ud83e\udd5e", Quantity = 1, UnitPrice = 20m },
                new OrderItem { Id = "OI-025", OrderId = "ORD-010", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 2, UnitPrice = 15m },
            },
            Subtotal = 92m, DiscountAmount = 0m, TaxTotal = 13.80m, GrandTotal = 105.80m, AmountPaid = 105.80m, ChangeAmount = 0m, CreatedAt = D(20, 20, 0), CompletedAt = D(20, 20, 50) },

        new Order { Id = "ORD-011", Number = "POS-0011", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C015", CustomerName = "Nasser Al-Tamimi", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-026", OrderId = "ORD-011", ProductId = "P023", ProductName = "Bluetooth Speaker", Emoji = "\ud83d\udd0a", Quantity = 1, UnitPrice = 120m },
            },
            Subtotal = 120m, DiscountAmount = 0m, TaxTotal = 18m, GrandTotal = 138m, AmountPaid = 140m, ChangeAmount = 2m, CreatedAt = D(19, 15, 30), CompletedAt = D(19, 15, 35) },

        new Order { Id = "ORD-012", Number = "POS-0012", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C020", CustomerName = "Reema Al-Fayez", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 14,
            Items = new() {
                new OrderItem { Id = "OI-027", OrderId = "ORD-012", ProductId = "P002", ProductName = "Cheese Burger", Emoji = "\ud83e\uddc0", Quantity = 2, UnitPrice = 32m },
                new OrderItem { Id = "OI-028", OrderId = "ORD-012", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 2, UnitPrice = 12m },
                new OrderItem { Id = "OI-029", OrderId = "ORD-012", ProductId = "P016", ProductName = "Chocolate Cake", Emoji = "\ud83c\udf70", Quantity = 2, UnitPrice = 22m },
                new OrderItem { Id = "OI-030", OrderId = "ORD-012", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 142m, DiscountAmount = 10m, DiscountType = Models.Enums.DiscountType.Percent, CouponCode = "SAVE10", TaxTotal = 19.80m, GrandTotal = 151.80m, AmountPaid = 151.80m, ChangeAmount = 0m, CreatedAt = D(18, 19, 30), CompletedAt = D(18, 20, 15) },

        new Order { Id = "ORD-013", Number = "POS-0013", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-031", OrderId = "ORD-013", ProductId = "P040", ProductName = "Blood Pressure Monitor", Emoji = "\ud83e\ude7a", Quantity = 1, UnitPrice = 150m },
            },
            Subtotal = 150m, DiscountAmount = 0m, TaxTotal = 22.50m, GrandTotal = 172.50m, AmountPaid = 172.50m, ChangeAmount = 0m, CreatedAt = D(17, 11, 0), CompletedAt = D(17, 11, 15) },

        new Order { Id = "ORD-014", Number = "POS-0014", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C009", CustomerName = "Faisal Al-Mutairi", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 2,
            Items = new() {
                new OrderItem { Id = "OI-032", OrderId = "ORD-014", ProductId = "P001", ProductName = "Classic Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 28m },
                new OrderItem { Id = "OI-033", OrderId = "ORD-014", ProductId = "P013", ProductName = "Water 500ml", Emoji = "\ud83d\udca7", Quantity = 1, UnitPrice = 3m },
            },
            Subtotal = 31m, DiscountAmount = 0m, TaxTotal = 4.65m, GrandTotal = 35.65m, AmountPaid = 40m, ChangeAmount = 4.35m, CreatedAt = D(16, 12, 0), CompletedAt = D(16, 12, 25) },

        new Order { Id = "ORD-015", Number = "POS-0015", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C023", CustomerName = "Waleed Al-Dossary", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-034", OrderId = "ORD-015", ProductId = "P025", ProductName = "Cotton T-Shirt", Emoji = "\ud83d\udc55", Quantity = 2, UnitPrice = 55m },
                new OrderItem { Id = "OI-035", OrderId = "ORD-015", ProductId = "P026", ProductName = "Baseball Cap", Emoji = "\ud83e\udde2", Quantity = 1, UnitPrice = 35m },
            },
            Subtotal = 145m, DiscountAmount = 0m, TaxTotal = 21.75m, GrandTotal = 166.75m, AmountPaid = 166.75m, ChangeAmount = 0m, CreatedAt = D(15, 17, 0), CompletedAt = D(15, 17, 10) },

        new Order { Id = "ORD-016", Number = "POS-0016", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-036", OrderId = "ORD-016", ProductId = "P032", ProductName = "Ibuprofen 400mg", Emoji = "\ud83d\udc8a", Quantity = 1, UnitPrice = 12m },
                new OrderItem { Id = "OI-037", OrderId = "ORD-016", ProductId = "P035", ProductName = "Hand Sanitizer 250ml", Emoji = "\ud83e\uddf4", Quantity = 2, UnitPrice = 10m },
            },
            Subtotal = 32m, DiscountAmount = 0m, TaxTotal = 4.80m, GrandTotal = 36.80m, AmountPaid = 40m, ChangeAmount = 3.20m, CreatedAt = D(14, 9, 0), CompletedAt = D(14, 9, 5) },

        new Order { Id = "ORD-017", Number = "POS-0017", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C013", CustomerName = "Hassan Al-Asmari", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 4,
            Items = new() {
                new OrderItem { Id = "OI-038", OrderId = "ORD-017", ProductId = "P006", ProductName = "Margherita Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 35m },
                new OrderItem { Id = "OI-039", OrderId = "ORD-017", ProductId = "P015", ProductName = "Mint Tea", Emoji = "\ud83c\udf75", Quantity = 2, UnitPrice = 10m },
            },
            Subtotal = 55m, DiscountAmount = 0m, TaxTotal = 8.25m, GrandTotal = 63.25m, AmountPaid = 63.25m, ChangeAmount = 0m, CreatedAt = D(13, 20, 15), CompletedAt = D(13, 20, 50) },

        new Order { Id = "ORD-018", Number = "POS-0018", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C001", CustomerName = "Mohammed Al-Rashidi", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mixed, OrderType = OrderType.DineIn, TableNumber = 6,
            Items = new() {
                new OrderItem { Id = "OI-040", OrderId = "ORD-018", ProductId = "P003", ProductName = "Double Burger", Emoji = "\ud83c\udf54", Quantity = 2, UnitPrice = 42m },
                new OrderItem { Id = "OI-041", OrderId = "ORD-018", ProductId = "P009", ProductName = "Four Cheese Pizza", Emoji = "\ud83e\uddc0", Quantity = 1, UnitPrice = 42m },
                new OrderItem { Id = "OI-042", OrderId = "ORD-018", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 2, UnitPrice = 12m },
                new OrderItem { Id = "OI-043", OrderId = "ORD-018", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 3, UnitPrice = 5m },
            },
            Subtotal = 165m, DiscountAmount = 20m, DiscountType = Models.Enums.DiscountType.Flat, CouponCode = "FLAT20", TaxTotal = 21.75m, GrandTotal = 166.75m, AmountPaid = 166.75m, ChangeAmount = 0m, CreatedAt = D(12, 13, 0), CompletedAt = D(12, 14, 0) },

        new Order { Id = "ORD-019", Number = "POS-0019", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-044", OrderId = "ORD-019", ProductId = "P042", ProductName = "Greek Yogurt 500g", Emoji = "\ud83e\udd5b", Quantity = 3, UnitPrice = 9m },
                new OrderItem { Id = "OI-045", OrderId = "ORD-019", ProductId = "P043", ProductName = "Cheddar Cheese Block", Emoji = "\ud83e\uddc0", Quantity = 1, UnitPrice = 18m },
                new OrderItem { Id = "OI-046", OrderId = "ORD-019", ProductId = "P045", ProductName = "Butter Croissant", Emoji = "\ud83e\udd50", Quantity = 4, UnitPrice = 4m },
            },
            Subtotal = 61m, DiscountAmount = 0m, TaxTotal = 9.15m, GrandTotal = 70.15m, AmountPaid = 75m, ChangeAmount = 4.85m, CreatedAt = D(11, 8, 45), CompletedAt = D(11, 9, 0) },

        new Order { Id = "ORD-020", Number = "POS-0020", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C016", CustomerName = "Salwa Al-Harthy", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 8,
            Items = new() {
                new OrderItem { Id = "OI-047", OrderId = "ORD-020", ProductId = "P020", ProductName = "Eggs Benedict", Emoji = "\ud83c\udf73", Quantity = 2, UnitPrice = 25m },
                new OrderItem { Id = "OI-048", OrderId = "ORD-020", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 2, UnitPrice = 15m },
            },
            Subtotal = 80m, DiscountAmount = 0m, TaxTotal = 12m, GrandTotal = 92m, AmountPaid = 92m, ChangeAmount = 0m, CreatedAt = D(10, 9, 0), CompletedAt = D(10, 9, 40) },

        new Order { Id = "ORD-021", Number = "POS-0021", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C022", CustomerName = "Dalal Al-Jabri", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-049", OrderId = "ORD-021", ProductId = "P024", ProductName = "Power Bank 10000mAh", Emoji = "\ud83d\udd0b", Quantity = 1, UnitPrice = 75m },
                new OrderItem { Id = "OI-050", OrderId = "ORD-021", ProductId = "P022", ProductName = "USB-C Cable", Emoji = "\ud83d\udd0c", Quantity = 1, UnitPrice = 15m },
            },
            Subtotal = 90m, DiscountAmount = 0m, TaxTotal = 13.50m, GrandTotal = 103.50m, AmountPaid = 103.50m, ChangeAmount = 0m, CreatedAt = D(9, 14, 0), CompletedAt = D(9, 14, 20) },

        new Order { Id = "ORD-022", Number = "POS-0022", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C010", CustomerName = "Maha Al-Ghamdi", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-051", OrderId = "ORD-022", ProductId = "P029", ProductName = "Hand Cream", Emoji = "\ud83e\uddf4", Quantity = 1, UnitPrice = 25m },
                new OrderItem { Id = "OI-052", OrderId = "ORD-022", ProductId = "P030", ProductName = "Lip Balm", Emoji = "\ud83d\udc84", Quantity = 2, UnitPrice = 12m },
                new OrderItem { Id = "OI-053", OrderId = "ORD-022", ProductId = "P027", ProductName = "Scented Candle Set", Emoji = "\ud83d\udd6f\ufe0f", Quantity = 1, UnitPrice = 40m },
            },
            Subtotal = 89m, DiscountAmount = 0m, TaxTotal = 13.35m, GrandTotal = 102.35m, AmountPaid = 105m, ChangeAmount = 2.65m, CreatedAt = D(8, 16, 30), CompletedAt = D(8, 16, 40) },

        new Order { Id = "ORD-023", Number = "POS-0023", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C025", CustomerName = "James Anderson", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 15,
            Items = new() {
                new OrderItem { Id = "OI-054", OrderId = "ORD-023", ProductId = "P007", ProductName = "Pepperoni Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 40m },
                new OrderItem { Id = "OI-055", OrderId = "ORD-023", ProductId = "P001", ProductName = "Classic Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 28m },
                new OrderItem { Id = "OI-056", OrderId = "ORD-023", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 78m, DiscountAmount = 0m, TaxTotal = 11.70m, GrandTotal = 89.70m, AmountPaid = 89.70m, ChangeAmount = 0m, CreatedAt = D(7, 13, 0), CompletedAt = D(7, 13, 40) },

        new Order { Id = "ORD-024", Number = "POS-0024", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-057", OrderId = "ORD-024", ProductId = "P033", ProductName = "Amoxicillin 500mg", Emoji = "\ud83d\udc8a", Quantity = 1, UnitPrice = 22m },
                new OrderItem { Id = "OI-058", OrderId = "ORD-024", ProductId = "P039", ProductName = "Digital Thermometer", Emoji = "\ud83c\udf21\ufe0f", Quantity = 1, UnitPrice = 35m },
            },
            Subtotal = 57m, DiscountAmount = 0m, TaxTotal = 8.55m, GrandTotal = 65.55m, AmountPaid = 70m, ChangeAmount = 4.45m, CreatedAt = D(6, 10, 0), CompletedAt = D(6, 10, 10) },

        new Order { Id = "ORD-025", Number = "POS-0025", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C018", CustomerName = "Amal Bin Laden", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 3,
            Items = new() {
                new OrderItem { Id = "OI-059", OrderId = "ORD-025", ProductId = "P005", ProductName = "Veggie Burger", Emoji = "\ud83e\udd66", Quantity = 1, UnitPrice = 26m },
                new OrderItem { Id = "OI-060", OrderId = "ORD-025", ProductId = "P012", ProductName = "Orange Juice", Emoji = "\ud83e\uddc3", Quantity = 1, UnitPrice = 12m },
            },
            Subtotal = 38m, DiscountAmount = 0m, TaxTotal = 5.70m, GrandTotal = 43.70m, AmountPaid = 43.70m, ChangeAmount = 0m, CreatedAt = D(5, 12, 30), CompletedAt = D(5, 13, 0) },

        new Order { Id = "ORD-026", Number = "POS-0026", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C002", CustomerName = "Fatima Al-Zahrani", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-061", OrderId = "ORD-026", ProductId = "P048", ProductName = "Frozen Pizza", Emoji = "\ud83c\udf55", Quantity = 2, UnitPrice = 15m },
                new OrderItem { Id = "OI-062", OrderId = "ORD-026", ProductId = "P049", ProductName = "Ice Cream Tub 1L", Emoji = "\ud83c\udf68", Quantity = 1, UnitPrice = 20m },
                new OrderItem { Id = "OI-063", OrderId = "ORD-026", ProductId = "P050", ProductName = "Water Pack (12x500ml)", Emoji = "\ud83d\udca7", Quantity = 1, UnitPrice = 12m },
            },
            Subtotal = 62m, DiscountAmount = 0m, TaxTotal = 9.30m, GrandTotal = 71.30m, AmountPaid = 71.30m, ChangeAmount = 0m, CreatedAt = D(4, 18, 0), CompletedAt = D(4, 18, 30) },

        new Order { Id = "ORD-027", Number = "POS-0027", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C019", CustomerName = "Yasser Al-Shahrani", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 1,
            Items = new() {
                new OrderItem { Id = "OI-064", OrderId = "ORD-027", ProductId = "P008", ProductName = "BBQ Chicken Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 45m },
                new OrderItem { Id = "OI-065", OrderId = "ORD-027", ProductId = "P011", ProductName = "Pepsi", Emoji = "\ud83e\udd64", Quantity = 1, UnitPrice = 5m },
            },
            Subtotal = 50m, DiscountAmount = 0m, TaxTotal = 7.50m, GrandTotal = 57.50m, AmountPaid = 60m, ChangeAmount = 2.50m, CreatedAt = D(3, 13, 30), CompletedAt = D(3, 14, 0) },

        new Order { Id = "ORD-028", Number = "POS-0028", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C006", CustomerName = "Huda Bin Salman", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 16,
            Items = new() {
                new OrderItem { Id = "OI-066", OrderId = "ORD-028", ProductId = "P002", ProductName = "Cheese Burger", Emoji = "\ud83e\uddc0", Quantity = 1, UnitPrice = 32m },
                new OrderItem { Id = "OI-067", OrderId = "ORD-028", ProductId = "P004", ProductName = "Chicken Burger", Emoji = "\ud83c\udf57", Quantity = 1, UnitPrice = 30m },
                new OrderItem { Id = "OI-068", OrderId = "ORD-028", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 2, UnitPrice = 12m },
                new OrderItem { Id = "OI-069", OrderId = "ORD-028", ProductId = "P016", ProductName = "Chocolate Cake", Emoji = "\ud83c\udf70", Quantity = 1, UnitPrice = 22m },
                new OrderItem { Id = "OI-070", OrderId = "ORD-028", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 2, UnitPrice = 15m },
            },
            Subtotal = 138m, DiscountAmount = 0m, TaxTotal = 20.70m, GrandTotal = 158.70m, AmountPaid = 158.70m, ChangeAmount = 0m, CreatedAt = D(2, 19, 0), CompletedAt = D(2, 20, 0) },

        new Order { Id = "ORD-029", Number = "POS-0029", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-071", OrderId = "ORD-029", ProductId = "P037", ProductName = "Baby Wipes (80 sheets)", Emoji = "\ud83d\udc76", Quantity = 2, UnitPrice = 14m },
                new OrderItem { Id = "OI-072", OrderId = "ORD-029", ProductId = "P038", ProductName = "Baby Shampoo 200ml", Emoji = "\ud83d\udc76", Quantity = 1, UnitPrice = 20m },
            },
            Subtotal = 48m, DiscountAmount = 0m, TaxTotal = 7.20m, GrandTotal = 55.20m, AmountPaid = 60m, ChangeAmount = 4.80m, CreatedAt = D(1, 11, 0), CompletedAt = D(1, 11, 10) },

        new Order { Id = "ORD-030", Number = "POS-0030", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C024", CustomerName = "Sarah Mitchell", Status = OrderStatus.Completed, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-073", OrderId = "ORD-030", ProductId = "P028", ProductName = "Wooden Photo Frame", Emoji = "\ud83d\uddbc\ufe0f", Quantity = 2, UnitPrice = 30m },
                new OrderItem { Id = "OI-074", OrderId = "ORD-030", ProductId = "P027", ProductName = "Scented Candle Set", Emoji = "\ud83d\udd6f\ufe0f", Quantity = 1, UnitPrice = 40m },
            },
            Subtotal = 100m, DiscountAmount = 5m, DiscountType = Models.Enums.DiscountType.Percent, CouponCode = "BUNDLE5", TaxTotal = 14.25m, GrandTotal = 109.25m, AmountPaid = 109.25m, ChangeAmount = 0m, CreatedAt = D(1, 15, 0), CompletedAt = D(1, 15, 30) },

        // ═══════════════════════════════════════════════════
        // PENDING  (10 orders)
        // ═══════════════════════════════════════════════════

        new Order { Id = "ORD-031", Number = "POS-0031", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C007", CustomerName = "Khalid Al-Otaibi", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 2,
            Items = new() {
                new OrderItem { Id = "OI-075", OrderId = "ORD-031", ProductId = "P001", ProductName = "Classic Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 28m },
                new OrderItem { Id = "OI-076", OrderId = "ORD-031", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 1, UnitPrice = 5m },
            },
            Subtotal = 33m, DiscountAmount = 0m, TaxTotal = 4.95m, GrandTotal = 37.95m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 9, 0) },

        new Order { Id = "ORD-032", Number = "POS-0032", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-077", OrderId = "ORD-032", ProductId = "P007", ProductName = "Pepperoni Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 40m },
            },
            Subtotal = 40m, DiscountAmount = 0m, TaxTotal = 6m, GrandTotal = 46m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 9, 15) },

        new Order { Id = "ORD-033", Number = "POS-0033", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C011", CustomerName = "Turki Al-Enazi", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 5,
            Items = new() {
                new OrderItem { Id = "OI-078", OrderId = "ORD-033", ProductId = "P004", ProductName = "Chicken Burger", Emoji = "\ud83c\udf57", Quantity = 2, UnitPrice = 30m },
                new OrderItem { Id = "OI-079", OrderId = "ORD-033", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 2, UnitPrice = 12m },
                new OrderItem { Id = "OI-080", OrderId = "ORD-033", ProductId = "P011", ProductName = "Pepsi", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 94m, DiscountAmount = 0m, TaxTotal = 14.10m, GrandTotal = 108.10m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 9, 30) },

        new Order { Id = "ORD-034", Number = "POS-0034", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-081", OrderId = "ORD-034", ProductId = "P041", ProductName = "Fresh Milk 1L", Emoji = "\ud83e\udd5b", Quantity = 2, UnitPrice = 7m },
                new OrderItem { Id = "OI-082", OrderId = "ORD-034", ProductId = "P047", ProductName = "Apple per kg", Emoji = "\ud83c\udf4e", Quantity = 3, UnitPrice = 8m },
            },
            Subtotal = 38m, DiscountAmount = 0m, TaxTotal = 5.70m, GrandTotal = 43.70m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 10, 0) },

        new Order { Id = "ORD-035", Number = "POS-0035", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C014", CustomerName = "Deema Al-Jubeir", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 4,
            Items = new() {
                new OrderItem { Id = "OI-083", OrderId = "ORD-035", ProductId = "P019", ProductName = "Pancakes", Emoji = "\ud83e\udd5e", Quantity = 1, UnitPrice = 20m },
                new OrderItem { Id = "OI-084", OrderId = "ORD-035", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 1, UnitPrice = 15m },
            },
            Subtotal = 35m, DiscountAmount = 0m, TaxTotal = 5.25m, GrandTotal = 40.25m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 10, 15) },

        new Order { Id = "ORD-036", Number = "POS-0036", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-085", OrderId = "ORD-036", ProductId = "P031", ProductName = "Paracetamol 500mg", Emoji = "\ud83d\udc8a", Quantity = 3, UnitPrice = 8m },
                new OrderItem { Id = "OI-086", OrderId = "ORD-036", ProductId = "P036", ProductName = "Face Mask Pack (50)", Emoji = "\ud83d\ude37", Quantity = 1, UnitPrice = 15m },
            },
            Subtotal = 39m, DiscountAmount = 0m, TaxTotal = 5.85m, GrandTotal = 44.85m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 10, 30) },

        new Order { Id = "ORD-037", Number = "POS-0037", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C021", CustomerName = "Majed Al-Saud", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 9,
            Items = new() {
                new OrderItem { Id = "OI-087", OrderId = "ORD-037", ProductId = "P006", ProductName = "Margherita Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 35m },
                new OrderItem { Id = "OI-088", OrderId = "ORD-037", ProductId = "P013", ProductName = "Water 500ml", Emoji = "\ud83d\udca7", Quantity = 2, UnitPrice = 3m },
            },
            Subtotal = 41m, DiscountAmount = 0m, TaxTotal = 6.15m, GrandTotal = 47.15m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 11, 0) },

        new Order { Id = "ORD-038", Number = "POS-0038", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C017", CustomerName = "Bandar Al-Malki", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-089", OrderId = "ORD-038", ProductId = "P023", ProductName = "Bluetooth Speaker", Emoji = "\ud83d\udd0a", Quantity = 1, UnitPrice = 120m },
            },
            Subtotal = 120m, DiscountAmount = 0m, TaxTotal = 18m, GrandTotal = 138m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 11, 15) },

        new Order { Id = "ORD-039", Number = "POS-0039", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-090", OrderId = "ORD-039", ProductId = "P002", ProductName = "Cheese Burger", Emoji = "\ud83e\uddc0", Quantity = 1, UnitPrice = 32m },
                new OrderItem { Id = "OI-091", OrderId = "ORD-039", ProductId = "P018", ProductName = "Chicken Nuggets", Emoji = "\ud83c\udf57", Quantity = 1, UnitPrice = 18m },
            },
            Subtotal = 50m, DiscountAmount = 0m, TaxTotal = 7.50m, GrandTotal = 57.50m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 11, 30) },

        new Order { Id = "ORD-040", Number = "POS-0040", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C004", CustomerName = "Noura Al-Harbi", Status = OrderStatus.Pending, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 10,
            Items = new() {
                new OrderItem { Id = "OI-092", OrderId = "ORD-040", ProductId = "P009", ProductName = "Four Cheese Pizza", Emoji = "\ud83e\uddc0", Quantity = 1, UnitPrice = 42m },
                new OrderItem { Id = "OI-093", OrderId = "ORD-040", ProductId = "P015", ProductName = "Mint Tea", Emoji = "\ud83c\udf75", Quantity = 2, UnitPrice = 10m },
            },
            Subtotal = 62m, DiscountAmount = 0m, TaxTotal = 9.30m, GrandTotal = 71.30m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 11, 45) },

        // ═══════════════════════════════════════════════════
        // IN PROGRESS  (8 orders)
        // ═══════════════════════════════════════════════════

        new Order { Id = "ORD-041", Number = "POS-0041", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C001", CustomerName = "Mohammed Al-Rashidi", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 3,
            Items = new() {
                new OrderItem { Id = "OI-094", OrderId = "ORD-041", ProductId = "P003", ProductName = "Double Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 42m },
                new OrderItem { Id = "OI-095", OrderId = "ORD-041", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 1, UnitPrice = 12m },
                new OrderItem { Id = "OI-096", OrderId = "ORD-041", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 1, UnitPrice = 5m },
            },
            Subtotal = 59m, DiscountAmount = 0m, TaxTotal = 8.85m, GrandTotal = 67.85m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 12, 0) },

        new Order { Id = "ORD-042", Number = "POS-0042", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-097", OrderId = "ORD-042", ProductId = "P008", ProductName = "BBQ Chicken Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 45m },
                new OrderItem { Id = "OI-098", OrderId = "ORD-042", ProductId = "P012", ProductName = "Orange Juice", Emoji = "\ud83e\uddc3", Quantity = 1, UnitPrice = 12m },
            },
            Subtotal = 57m, DiscountAmount = 0m, TaxTotal = 8.55m, GrandTotal = 65.55m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 12, 10) },

        new Order { Id = "ORD-043", Number = "POS-0043", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C008", CustomerName = "Rania Al-Shammari", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 7,
            Items = new() {
                new OrderItem { Id = "OI-099", OrderId = "ORD-043", ProductId = "P005", ProductName = "Veggie Burger", Emoji = "\ud83e\udd66", Quantity = 2, UnitPrice = 26m },
                new OrderItem { Id = "OI-100", OrderId = "ORD-043", ProductId = "P015", ProductName = "Mint Tea", Emoji = "\ud83c\udf75", Quantity = 2, UnitPrice = 10m },
            },
            Subtotal = 72m, DiscountAmount = 0m, TaxTotal = 10.80m, GrandTotal = 82.80m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 12, 20) },

        new Order { Id = "ORD-044", Number = "POS-0044", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C005", CustomerName = "Saad Al-Dosari", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery,
            Items = new() {
                new OrderItem { Id = "OI-101", OrderId = "ORD-044", ProductId = "P044", ProductName = "White Bread Loaf", Emoji = "\ud83c\udf5e", Quantity = 3, UnitPrice = 5m },
                new OrderItem { Id = "OI-102", OrderId = "ORD-044", ProductId = "P041", ProductName = "Fresh Milk 1L", Emoji = "\ud83e\udd5b", Quantity = 2, UnitPrice = 7m },
                new OrderItem { Id = "OI-103", OrderId = "ORD-044", ProductId = "P046", ProductName = "Banana per kg", Emoji = "\ud83c\udf4c", Quantity = 2, UnitPrice = 6m },
            },
            Subtotal = 41m, DiscountAmount = 0m, TaxTotal = 6.15m, GrandTotal = 47.15m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 12, 30) },

        new Order { Id = "ORD-045", Number = "POS-0045", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway,
            Items = new() {
                new OrderItem { Id = "OI-104", OrderId = "ORD-045", ProductId = "P001", ProductName = "Classic Burger", Emoji = "\ud83c\udf54", Quantity = 2, UnitPrice = 28m },
                new OrderItem { Id = "OI-105", OrderId = "ORD-045", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 66m, DiscountAmount = 0m, TaxTotal = 9.90m, GrandTotal = 75.90m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 12, 45) },

        new Order { Id = "ORD-046", Number = "POS-0046", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C012", CustomerName = "Lama Al-Subaie", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 6,
            Items = new() {
                new OrderItem { Id = "OI-106", OrderId = "ORD-046", ProductId = "P007", ProductName = "Pepperoni Pizza", Emoji = "\ud83c\udf55", Quantity = 1, UnitPrice = 40m },
                new OrderItem { Id = "OI-107", OrderId = "ORD-046", ProductId = "P016", ProductName = "Chocolate Cake", Emoji = "\ud83c\udf70", Quantity = 1, UnitPrice = 22m },
                new OrderItem { Id = "OI-108", OrderId = "ORD-046", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 1, UnitPrice = 15m },
            },
            Subtotal = 77m, DiscountAmount = 0m, TaxTotal = 11.55m, GrandTotal = 88.55m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 13, 0) },

        new Order { Id = "ORD-047", Number = "POS-0047", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 11,
            Items = new() {
                new OrderItem { Id = "OI-109", OrderId = "ORD-047", ProductId = "P020", ProductName = "Eggs Benedict", Emoji = "\ud83c\udf73", Quantity = 1, UnitPrice = 25m },
                new OrderItem { Id = "OI-110", OrderId = "ORD-047", ProductId = "P015", ProductName = "Mint Tea", Emoji = "\ud83c\udf75", Quantity = 1, UnitPrice = 10m },
            },
            Subtotal = 35m, DiscountAmount = 0m, TaxTotal = 5.25m, GrandTotal = 40.25m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 13, 15) },

        new Order { Id = "ORD-048", Number = "POS-0048", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", CustomerId = "C022", CustomerName = "Dalal Al-Jabri", Status = OrderStatus.InProgress, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.DineIn, TableNumber = 8,
            Items = new() {
                new OrderItem { Id = "OI-111", OrderId = "ORD-048", ProductId = "P006", ProductName = "Margherita Pizza", Emoji = "\ud83c\udf55", Quantity = 2, UnitPrice = 35m },
                new OrderItem { Id = "OI-112", OrderId = "ORD-048", ProductId = "P018", ProductName = "Chicken Nuggets", Emoji = "\ud83c\udf57", Quantity = 1, UnitPrice = 18m },
                new OrderItem { Id = "OI-113", OrderId = "ORD-048", ProductId = "P011", ProductName = "Pepsi", Emoji = "\ud83e\udd64", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 98m, DiscountAmount = 0m, TaxTotal = 14.70m, GrandTotal = 112.70m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 13, 30) },

        // ═══════════════════════════════════════════════════
        // ON HOLD  (5 orders)
        // ═══════════════════════════════════════════════════

        new Order { Id = "ORD-049", Number = "POS-0049", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.OnHold, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 1, Notes = "Customer stepped out, will return shortly",
            Items = new() {
                new OrderItem { Id = "OI-114", OrderId = "ORD-049", ProductId = "P003", ProductName = "Double Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 42m },
                new OrderItem { Id = "OI-115", OrderId = "ORD-049", ProductId = "P010", ProductName = "Coca Cola", Emoji = "\ud83e\udd64", Quantity = 1, UnitPrice = 5m },
            },
            Subtotal = 47m, DiscountAmount = 0m, TaxTotal = 7.05m, GrandTotal = 54.05m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 11, 0) },

        new Order { Id = "ORD-050", Number = "POS-0050", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C009", CustomerName = "Faisal Al-Mutairi", Status = OrderStatus.OnHold, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway, Notes = "Waiting for additional items",
            Items = new() {
                new OrderItem { Id = "OI-116", OrderId = "ORD-050", ProductId = "P025", ProductName = "Cotton T-Shirt", Emoji = "\ud83d\udc55", Quantity = 1, UnitPrice = 55m },
            },
            Subtotal = 55m, DiscountAmount = 0m, TaxTotal = 8.25m, GrandTotal = 63.25m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 10, 30) },

        new Order { Id = "ORD-051", Number = "POS-0051", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", Status = OrderStatus.OnHold, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery, Notes = "Customer unreachable, retry in 30 min",
            Items = new() {
                new OrderItem { Id = "OI-117", OrderId = "ORD-051", ProductId = "P043", ProductName = "Cheddar Cheese Block", Emoji = "\ud83e\uddc0", Quantity = 2, UnitPrice = 18m },
                new OrderItem { Id = "OI-118", OrderId = "ORD-051", ProductId = "P045", ProductName = "Butter Croissant", Emoji = "\ud83e\udd50", Quantity = 6, UnitPrice = 4m },
            },
            Subtotal = 60m, DiscountAmount = 0m, TaxTotal = 9m, GrandTotal = 69m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 10, 0) },

        new Order { Id = "ORD-052", Number = "POS-0052", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", CustomerId = "C016", CustomerName = "Salwa Al-Harthy", Status = OrderStatus.OnHold, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 12, Notes = "Manager approval needed for discount",
            Items = new() {
                new OrderItem { Id = "OI-119", OrderId = "ORD-052", ProductId = "P008", ProductName = "BBQ Chicken Pizza", Emoji = "\ud83c\udf55", Quantity = 2, UnitPrice = 45m },
                new OrderItem { Id = "OI-120", OrderId = "ORD-052", ProductId = "P014", ProductName = "Arabic Coffee", Emoji = "\u2615", Quantity = 2, UnitPrice = 15m },
            },
            Subtotal = 120m, DiscountAmount = 0m, TaxTotal = 18m, GrandTotal = 138m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 9, 45) },

        new Order { Id = "ORD-053", Number = "POS-0053", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.OnHold, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.Takeaway, Notes = "Payment issue, customer checking wallet",
            Items = new() {
                new OrderItem { Id = "OI-121", OrderId = "ORD-053", ProductId = "P024", ProductName = "Power Bank 10000mAh", Emoji = "\ud83d\udd0b", Quantity = 1, UnitPrice = 75m },
            },
            Subtotal = 75m, DiscountAmount = 0m, TaxTotal = 11.25m, GrandTotal = 86.25m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(0, 10, 15) },

        // ═══════════════════════════════════════════════════
        // VOIDED  (4 orders)
        // ═══════════════════════════════════════════════════

        new Order { Id = "ORD-054", Number = "POS-0054", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", Status = OrderStatus.Voided, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 4, Notes = "Customer left before ordering",
            Items = new() {
                new OrderItem { Id = "OI-122", OrderId = "ORD-054", ProductId = "P001", ProductName = "Classic Burger", Emoji = "\ud83c\udf54", Quantity = 1, UnitPrice = 28m },
            },
            Subtotal = 28m, DiscountAmount = 0m, TaxTotal = 4.20m, GrandTotal = 32.20m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(5, 14, 0) },

        new Order { Id = "ORD-055", Number = "POS-0055", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", Status = OrderStatus.Voided, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway, Notes = "Duplicate order entered in error",
            Items = new() {
                new OrderItem { Id = "OI-123", OrderId = "ORD-055", ProductId = "P007", ProductName = "Pepperoni Pizza", Emoji = "\ud83c\udf55", Quantity = 2, UnitPrice = 40m },
            },
            Subtotal = 80m, DiscountAmount = 0m, TaxTotal = 12m, GrandTotal = 92m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(10, 16, 0) },

        new Order { Id = "ORD-056", Number = "POS-0056", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", Status = OrderStatus.Voided, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery, Notes = "Wrong delivery address, customer cancelled",
            Items = new() {
                new OrderItem { Id = "OI-124", OrderId = "ORD-056", ProductId = "P042", ProductName = "Greek Yogurt 500g", Emoji = "\ud83e\udd5b", Quantity = 4, UnitPrice = 9m },
                new OrderItem { Id = "OI-125", OrderId = "ORD-056", ProductId = "P044", ProductName = "White Bread Loaf", Emoji = "\ud83c\udf5e", Quantity = 2, UnitPrice = 5m },
            },
            Subtotal = 46m, DiscountAmount = 0m, TaxTotal = 6.90m, GrandTotal = 52.90m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(15, 11, 30) },

        new Order { Id = "ORD-057", Number = "POS-0057", BranchId = "1", EmployeeId = "E006", EmployeeName = "Reem Faisal", Status = OrderStatus.Voided, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 2, Notes = "System test order",
            Items = new() {
                new OrderItem { Id = "OI-126", OrderId = "ORD-057", ProductId = "P013", ProductName = "Water 500ml", Emoji = "\ud83d\udca7", Quantity = 1, UnitPrice = 3m },
            },
            Subtotal = 3m, DiscountAmount = 0m, TaxTotal = 0.45m, GrandTotal = 3.45m, AmountPaid = 0m, ChangeAmount = 0m, CreatedAt = D(20, 8, 0) },

        // ═══════════════════════════════════════════════════
        // REFUNDED  (3 orders)
        // ═══════════════════════════════════════════════════

        new Order { Id = "ORD-058", Number = "POS-0058", BranchId = "1", EmployeeId = "E004", EmployeeName = "Yusuf Ali", CustomerId = "C015", CustomerName = "Nasser Al-Tamimi", Status = OrderStatus.Refunded, PaymentMethod = PaymentMethod.Card, OrderType = OrderType.Takeaway, Notes = "Defective product returned",
            Items = new() {
                new OrderItem { Id = "OI-127", OrderId = "ORD-058", ProductId = "P021", ProductName = "Wireless Mouse", Emoji = "\ud83d\uddb1\ufe0f", Quantity = 1, UnitPrice = 45m },
            },
            Subtotal = 45m, DiscountAmount = 0m, TaxTotal = 6.75m, GrandTotal = 51.75m, AmountPaid = 51.75m, ChangeAmount = 0m, CreatedAt = D(8, 14, 0), CompletedAt = D(8, 14, 5) },

        new Order { Id = "ORD-059", Number = "POS-0059", BranchId = "1", EmployeeId = "E005", EmployeeName = "Khalid Nasser", CustomerId = "C002", CustomerName = "Fatima Al-Zahrani", Status = OrderStatus.Refunded, PaymentMethod = PaymentMethod.Cash, OrderType = OrderType.DineIn, TableNumber = 5, Notes = "Food quality complaint, full refund issued",
            Items = new() {
                new OrderItem { Id = "OI-128", OrderId = "ORD-059", ProductId = "P004", ProductName = "Chicken Burger", Emoji = "\ud83c\udf57", Quantity = 1, UnitPrice = 30m },
                new OrderItem { Id = "OI-129", OrderId = "ORD-059", ProductId = "P017", ProductName = "French Fries", Emoji = "\ud83c\udf5f", Quantity = 1, UnitPrice = 12m },
            },
            Subtotal = 42m, DiscountAmount = 0m, TaxTotal = 6.30m, GrandTotal = 48.30m, AmountPaid = 48.30m, ChangeAmount = 0m, CreatedAt = D(12, 13, 30), CompletedAt = D(12, 13, 50) },

        new Order { Id = "ORD-060", Number = "POS-0060", BranchId = "2", EmployeeId = "E009", EmployeeName = "Layla Mahmoud", Status = OrderStatus.Refunded, PaymentMethod = PaymentMethod.Mobile, OrderType = OrderType.Delivery, Notes = "Wrong item delivered, partial refund",
            Items = new() {
                new OrderItem { Id = "OI-130", OrderId = "ORD-060", ProductId = "P034", ProductName = "Vitamin C 1000mg", Emoji = "\ud83c\udf4a", Quantity = 2, UnitPrice = 18m },
                new OrderItem { Id = "OI-131", OrderId = "ORD-060", ProductId = "P031", ProductName = "Paracetamol 500mg", Emoji = "\ud83d\udc8a", Quantity = 1, UnitPrice = 8m },
            },
            Subtotal = 44m, DiscountAmount = 0m, TaxTotal = 6.60m, GrandTotal = 50.60m, AmountPaid = 50.60m, ChangeAmount = 0m, CreatedAt = D(18, 10, 0), CompletedAt = D(18, 10, 15) },
    };
}
