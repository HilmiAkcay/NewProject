namespace DAL.Entities;

public class Product : EntityBase
{
    public string Name { get; set; } = default!;
    public int ProductGroupId { get; set; }
    public int DefaultPuId { get; set; }
    public int DefaultTaxRateId { get; set; }
    public string SKU { get; set; } = default!;

    // Navigation properties
    public ProductGroup? ProductGroup { get; set; }
    public ProducUnit? DefaultPu { get; set; }
    public TaxRate? DefaultTaxRate { get; set; }

    public ICollection<ProducUnit> ProductUnits { get; set; } = new List<ProducUnit>();
    public ICollection<PriceRuleAssignment> PriceRuleAssignments { get; set; } = new List<PriceRuleAssignment>();
}
