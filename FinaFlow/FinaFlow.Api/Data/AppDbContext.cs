using FinaFlow.Api.Data.Mappings;
using FinaFlow.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FinaFlow.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryMapping());
        modelBuilder.ApplyConfiguration(new TransactionMapping());
    }
}
