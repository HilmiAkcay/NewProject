namespace DAL.Entities;

public class PriceRuleAssignment : EntityBase
{
    public int PriceRuleId { get; set; }
    public int ProductUnitId { get; set; }
    public int AccountId { get; set; }
    public decimal? FixedPrice { get; set; }
    public decimal? DiscountPercent { get; set; }

    // Navigation
    public PriceRule? PriceRule { get; set; }
    public ProducUnit? ProductUnit { get; set; }
    public Account? Account { get; set; }
}
