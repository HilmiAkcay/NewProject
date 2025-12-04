namespace DAL.Entities;

public class PurchasePrice : EntityBase
{
    public int ProductUnitId { get; set; }
    public int AccountId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int TaxRateId { get; set; }
    public bool IsGrossPrice { get; set; }

}
