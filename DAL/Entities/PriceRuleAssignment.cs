namespace DAL.Entities;

public class PriceRuleAssignment : EntityBase
{
    public int PriceRuleId { get; set; }
    public int? ProductId { get; set; }
    public int? ProductGroupId { get; set; }
    public int? AccountId { get; set; }
    public int? AccountGroupId { get; set; }
    public int? ProductUnitId { get; set; }
}
