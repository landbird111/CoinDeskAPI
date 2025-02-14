using CoinDeskAPI.Models.DataLayerModels.Currency;
using Microsoft.EntityFrameworkCore;

namespace CoinDeskAPI.Provider.Currency;

public class CurrencyContext : DbContext
{
    public CurrencyContext(DbContextOptions<CurrencyContext> options) : base(options)
    {
    }

    public DbSet<CurrencyInfo> CurrencyInfos { get; set; }
    public DbSet<CurrencyLang> CurrencyLangs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyInfo>().ToTable("CurrencyInfo");
        modelBuilder.Entity<CurrencyLang>().ToTable("CurrencyLang");
        modelBuilder.Entity<CurrencyLang>()
            .HasOne(p => p.CurrencyInfo)
            .WithMany(b => b.CurrencyLangs)
            .HasForeignKey(p => p.CurrencyInfoId);
    }
}