namespace DAL.Entities;
public class Unit : EntityBase
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;

    public ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();
}

