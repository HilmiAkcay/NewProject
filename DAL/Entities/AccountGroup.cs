namespace DAL.Entities;
public class AccountGroup : EntityBase
{
    public string Name { get; set; } = default!;

    public ICollection<AccountGroupAccount> Accounts { get; set; } = new List<AccountGroupAccount>();
}

