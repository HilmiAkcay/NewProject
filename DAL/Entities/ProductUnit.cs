namespace DAL.Entities;

public class ProductUnit : EntityBase
{
    public int ProductId { get; set; }
    public int UnitId { get; set; }
    public int Factor { get; set; } //int or decimal?

    // Navigation
    public Product? Product { get; set; }
    public Unit? Unit { get; set; }

    public ICollection<PurchasePrice> PurchasePrices { get; set; } = new List<PurchasePrice>();
    public ICollection<SalesPrice> SalesPrices { get; set; } = new List<SalesPrice>();
    public ICollection<PriceRuleAssignment> PriceRuleAssignments { get; set; } = new List<PriceRuleAssignment>();
}
