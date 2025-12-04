namespace DAL.Entities;

public class TaxRate : EntityBase
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Rate { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
