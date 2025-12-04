using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductGroup> ProductGroups { get; set; } = null!;
    public DbSet<ProducUnit> ProductUnits { get; set; } = null!;
    public DbSet<Unit> Units { get; set; } = null!;
    public DbSet<TaxRate> TaxRates { get; set; } = null!;
    public DbSet<PurchasePrice> PurchasePrices { get; set; } = null!;
    public DbSet<SalesPrice> SalesPrices { get; set; } = null!;
    public DbSet<PriceRule> PriceRules { get; set; } = null!;
    public DbSet<PriceRuleAssignment> PriceRuleAssignments { get; set; } = null!;

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<AccountGroup> AccountGroups { get; set; } = null!;
    public DbSet<AccountGroupAccount> AccountGroupAccounts { get; set; } = null!;
    public DbSet<AccountContact> AccountContacts { get; set; } = null!;
    public DbSet<AccountAddress> AccountAddresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ProducUnit -> Product (one-to-many)
        modelBuilder.Entity<ProducUnit>(b =>
        {
            b.HasOne(pu => pu.Product)
                .WithMany(p => p.ProductUnits)
                .HasForeignKey(pu => pu.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(pu => pu.Unit)
                .WithMany(u => u.ProductUnits)
                .HasForeignKey(pu => pu.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(pu => pu.PurchasePrices)
                .WithOne(pp => pp.ProductUnit)
                .HasForeignKey(pp => pp.ProductUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(pu => pu.SalesPrices)
                .WithOne(sp => sp.ProductUnit)
                .HasForeignKey(sp => sp.ProductUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(pu => pu.PriceRuleAssignments)
                .WithOne(a => a.ProductUnit)
                .HasForeignKey(a => a.ProductUnitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Product default PU (FK on Product) — explicit to avoid ambiguity with ProductUnits collection
        modelBuilder.Entity<Product>(b =>
        {
            b.HasOne(p => p.DefaultPu)
                .WithMany() // ProducUnit has no dedicated inverse nav for "DefaultPu"
                .HasForeignKey(p => p.DefaultPuId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(p => p.ProductGroup)
                .WithMany(pg => pg.Products)
                .HasForeignKey(p => p.ProductGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(p => p.DefaultTaxRate)
                .WithMany(tr => tr.Products)
                .HasForeignKey(p => p.DefaultTaxRateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TaxRate <-> PurchasePrice / SalesPrice
        modelBuilder.Entity<PurchasePrice>(b =>
        {
            b.HasOne(pp => pp.TaxRate)
                .WithMany(tr => tr.PurchasePrices)
                .HasForeignKey(pp => pp.TaxRateId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(pp => pp.Account)
                .WithMany(a => a.PurchasePrices)
                .HasForeignKey(pp => pp.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SalesPrice>(b =>
        {
            b.HasOne(sp => sp.TaxRate)
                .WithMany(tr => tr.SalesPrices)
                .HasForeignKey(sp => sp.TaxRateId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PriceRuleAssignment relations
        modelBuilder.Entity<PriceRuleAssignment>(b =>
        {
            b.HasOne(a => a.PriceRule)
                .WithMany(pr => pr.PriceRuleAssignments)
                .HasForeignKey(a => a.PriceRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(a => a.Account)
                .WithMany(ac => ac.PriceRuleAssignments)
                .HasForeignKey(a => a.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AccountGroupAccount
        modelBuilder.Entity<AccountGroupAccount>(b =>
        {
            b.HasOne(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.AccountGroup)
                .WithMany()
                .HasForeignKey(x => x.AccountGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
