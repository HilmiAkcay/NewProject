namespace DAL.Entities;

public class AccountAddress : EntityBase
{
    public int AccountId { get; set; }
    public string AddressType { get; set; } = "General";
    // Billing, Delivery, Warehouse, Legal, Return
    public string AddressLine1 { get; set; } = default!;
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int CountryId { get; set; } = default!;
    public string? PostalCode { get; set; }

    // Navigation
    public Account? Account { get; set; }
    public Country Country { get; set; } = default!;
}
