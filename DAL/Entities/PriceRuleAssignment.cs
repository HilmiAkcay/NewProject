namespace NewDAL.Entities
{
    public class PriceRuleAssignment : EntityBase
    {
        public int PriceRuleId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductGroupId { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerGroupId { get; set; }
        public int? ProductUnitId { get; set; }
    }
}
