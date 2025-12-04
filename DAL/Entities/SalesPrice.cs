namespace DAL.Entities;

public class SalesPrice : EntityBase
{
    public int ProductUnitId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public int TaxRateId { get; set; }
    public int IsGrossPrice { get; set; }
}
