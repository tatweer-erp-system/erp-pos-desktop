namespace TatweerPOS.Data;

using TatweerPOS.Models;

public static class MockBranches
{
    public static List<Branch> All => new()
    {
        new Branch
        {
            Id = "1", Name = "Main Branch", Location = "Riyadh - King Fahd Road",
            Address = "King Fahd Road, Al-Olaya District, Riyadh 12211, Saudi Arabia",
            Phone = "0112345001", TaxNumber = "300456789012345", IsActive = true
        },
        new Branch
        {
            Id = "2", Name = "North Branch", Location = "Riyadh - Olaya Street",
            Address = "Olaya Street, Al-Mughrazzat District, Riyadh 12463, Saudi Arabia",
            Phone = "0112345002", TaxNumber = "300456789012346", IsActive = true
        },
        new Branch
        {
            Id = "3", Name = "South Branch", Location = "Jeddah - Tahlia Street",
            Address = "Tahlia Street, Al-Andalus District, Jeddah 23326, Saudi Arabia",
            Phone = "0122345003", TaxNumber = "300456789012347", IsActive = true
        },
        new Branch
        {
            Id = "4", Name = "East Branch", Location = "Dammam - King Saud Road",
            Address = "King Saud Road, Al-Faisaliyah District, Dammam 32241, Saudi Arabia",
            Phone = "0132345004", TaxNumber = "300456789012348", IsActive = true
        },
        new Branch
        {
            Id = "5", Name = "West Branch", Location = "Mecca - Ibrahim Al-Khalil",
            Address = "Ibrahim Al-Khalil Street, Al-Aziziyah District, Mecca 24231, Saudi Arabia",
            Phone = "0125345005", TaxNumber = "300456789012349", IsActive = false
        },
    };
}
