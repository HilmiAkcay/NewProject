namespace DAL.Entities;

public class Account : EntityBase
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string? TaxNumber { get; set; }
    public string? VATNumber { get; set; }
    public string Currency { get; set; } = "USD";
    public int PaymentTermDays { get; set; } = 30;
    public string? IBAN { get; set; }
    public string? BankName { get; set; }
    public bool TaxExempt { get; set; }
}
