namespace NewDAL.Entities
{
    public class PriceRule : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte RuleType { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FixedPrice { get; set; }
        public decimal MarginPercent { get; set; }
        public int TaxRateId { get; set; }
        public bool ApplyOnGrossPrice { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
