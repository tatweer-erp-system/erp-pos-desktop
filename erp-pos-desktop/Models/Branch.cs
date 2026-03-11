namespace TatweerPOS.Models;

public class Branch
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public string Address { get; set; } = "";
    public string Phone { get; set; } = "";
    public string TaxNumber { get; set; } = "";
    public bool IsActive { get; set; } = true;

    public override string ToString() => Name;
}
