namespace DAL.Entities;

public class PriceRuleAssignment : EntityBase
{
    public int PriceRuleId { get; set; }
    public int? ProductUnitId { get; set; }
    public int? ProductId { get; set; }
    public int? AccountId { get; set; }
    public int? AccountGroupId { get; set; }

    public decimal? FixedPrice { get; set; }
    public decimal? DiscountPercent { get; set; }

    // Navigation
    public PriceRule? PriceRule { get; set; }
    public ProductUnit? ProductUnit { get; set; }
    public Account? Account { get; set; }
    public AccountGroup? AccountGroup { get; set; }
    public Product? Product { get; set; }
}
