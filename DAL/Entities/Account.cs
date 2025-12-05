namespace DAL.Entities;

public class Account : EntityBase
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? TaxNumber { get; set; }
    public string? VATNumber { get; set; }
    public int CurrencyId { get; set; } = default!;
    public int PaymentTermDays { get; set; } = 30;
    public string? IBAN { get; set; }
    public string? BankName { get; set; }
    public bool TaxExempt { get; set; }

    // Navigation
    public Currency Currency { get; set; } = default!;
    public ICollection<PurchasePrice> PurchasePrices { get; set; } = new List<PurchasePrice>();
    public ICollection<AccountContact> Contacts { get; set; } = new List<AccountContact>();
    public ICollection<AccountAddress> Addresses { get; set; } = new List<AccountAddress>();
    public ICollection<AccountGroupAccount> AccountGroupAccounts { get; set; } = new List<AccountGroupAccount>();
    public ICollection<PriceRuleAssignment> PriceRuleAssignments { get; set; } = new List<PriceRuleAssignment>();
}
