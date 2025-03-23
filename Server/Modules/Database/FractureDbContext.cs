using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace Fracture.Server.Modules.Database;

public class FractureDbContext : DbContext
{
    public FractureDbContext(DbContextOptions<FractureDbContext> options)
        : base(options) { }

    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<ItemStatistics> ItemStatistics { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>().HasOne(i => i.Statistics).WithOne(s => s.Item);

        modelBuilder.Entity<Item>().HasOne(i => i.CreatedBy).WithMany(u => u.Items);

        modelBuilder.Entity<User>().HasMany(u => u.Items).WithOne(i => i.CreatedBy);

        modelBuilder.Entity<ItemStatistics>().HasOne(s => s.Item).WithOne(i => i.Statistics);

        base.OnModelCreating(modelBuilder);
    }
}
