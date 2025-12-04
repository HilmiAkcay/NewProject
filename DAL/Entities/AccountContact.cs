namespace DAL.Entities;

public class AccountContact : EntityBase
{
    public int AccountId { get; set; }
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? ContactType { get; set; } // Accounting, Logistics, Sales, etc.

    // Navigation
    public Account? Account { get; set; }
}
