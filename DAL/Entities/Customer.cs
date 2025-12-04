namespace NewDAL.Entities
{
    public class Customer : EntityBase
    {
        public string Name { get; set; }
        public int TaxExempt { get; set; }
    }
}
