namespace NewDAL.Entities
{
    public class Product : EntityBase
    {
        public string Name { get; set; }
        public int ProductGroupId { get; set; }
        public int DefaultPuId { get; set; }
        public int DefaultTaxRateId { get; set; }
        public string SKU { get; set; }
    }
}
