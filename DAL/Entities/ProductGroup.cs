namespace DAL.Entities;

public class ProductGroup : EntityBase
{
    public string Name { get; set; }

    // Navigation properties
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
