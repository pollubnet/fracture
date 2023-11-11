using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Game.NonPlayerCharacter.Domain.Data.Entities;
using NPC = Game.NonPlayerCharacter.Domain.Data.Entities.NonPlayerCharacter;

namespace Game.NonPlayerCharacter.Infrastructure.PersistenceLayer
{
    public class NonPlayerCharacterDbContext : DbContext
    {
        public NonPlayerCharacterDbContext(DbContextOptions<NonPlayerCharacterDbContext> options)
            : base(options) { }

        public DbSet<NPC> NonPlayerCharacters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NPC>().OwnsOne(st => st.Story);
        }
    }
}
