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
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}
