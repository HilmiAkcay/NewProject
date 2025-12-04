namespace DAL.Entities;

public class AccountGroupAccount : EntityBase
{
    public int AccountId { get; set; }
    public int AccountGroupId { get; set; }

    // Navigation
    public Account? Account { get; set; }
    public AccountGroup? AccountGroup { get; set; }
}
