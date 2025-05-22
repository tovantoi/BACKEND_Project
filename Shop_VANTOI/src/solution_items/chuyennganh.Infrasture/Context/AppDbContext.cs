using chuyennganh.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace chuyennganh.Infrasture.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Order> Orders { get; set; }           // ✅ DbSet cho Order
    public DbSet<OrderItem> OrderItems { get; set; }   // ✅ DbSet cho OrderItem
    public DbSet<Product> Products { get; set; }       // ✅ Nếu cần
    public DbSet<ProductReview> ProductReviews { get; set; } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

