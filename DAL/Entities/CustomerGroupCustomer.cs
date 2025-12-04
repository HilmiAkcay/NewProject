namespace NewDAL.Entities
{
    public class CustomerGroupCustomer : EntityBase
    {
        public int CustomerId { get; set; }
        public int CustomerGroupId { get; set; }
    }
}
