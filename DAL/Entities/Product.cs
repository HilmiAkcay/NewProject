namespace DAL.Entities;
using System.Collections.Generic;

public class Product : EntityBase
{
    public string Name { get; set; } = default!;
    public int ProductGroupId { get; set; }
    public int DefaultPuId { get; set; }
    public int DefaultTaxRateId { get; set; }
    public string SKU { get; set; } = default!;

    // Navigation properties
    public ProductGroup? ProductGroup { get; set; }
    public ProductUnit? DefaultPu { get; set; }
    public TaxRate? DefaultTaxRate { get; set; }

    public ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();
    public ICollection<PriceRuleAssignment> PriceRuleAssignments { get; set; } = new List<PriceRuleAssignment>();
}
