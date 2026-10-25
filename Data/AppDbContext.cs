// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using dotnet_api.Entities;
namespace dotnet_api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            e.HasIndex(p => p.Name).IsUnique();
            e.Property(p => p.Price).HasPrecision(18, 2);
        });
    }
}
