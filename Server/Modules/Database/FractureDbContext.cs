using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Users;
using Microsoft.EntityFrameworkCore;

namespace Fracture.Server.Modules.Database
{
    public class FractureDbContext : DbContext
    {
        public FractureDbContext(DbContextOptions<FractureDbContext> options)
            : base(options) { }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemStatistics> ItemStatistics { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity
                    .Property(u => u.Id)
                    .UseIdentityByDefaultColumn()
                    .HasIdentityOptions(startValue: 1, incrementBy: 1);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity
                    .Property(i => i.Id)
                    .UseIdentityByDefaultColumn()
                    .HasIdentityOptions(startValue: 1, incrementBy: 1);
            });

            modelBuilder
                .Entity<Item>()
                .HasOne(i => i.Statistics)
                .WithOne(s => s.Item)
                .HasForeignKey<ItemStatistics>(s => s.ItemId);

            modelBuilder
                .Entity<Item>()
                .HasOne(i => i.CreatedBy)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.CreatedById);

            modelBuilder
                .Entity<ItemStatistics>()
                .HasOne(s => s.Item)
                .WithOne(i => i.Statistics)
                .HasForeignKey<ItemStatistics>(s => s.ItemId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
