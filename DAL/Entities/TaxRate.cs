namespace DAL.Entities;

public class TaxRate : EntityBase
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Rate { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<PurchasePrice> PurchasePrices { get; set; } = new List<PurchasePrice>();
    public ICollection<SalesPrice> SalesPrices { get; set; } = new List<SalesPrice>();
    public ICollection<PriceRule> PriceRules { get; set; } = new List<PriceRule>();
}
