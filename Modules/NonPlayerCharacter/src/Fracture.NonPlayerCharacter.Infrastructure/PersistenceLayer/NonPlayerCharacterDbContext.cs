using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Fracture.NonPlayerCharacter.Domain.Data.Entities;
using NPC = Fracture.NonPlayerCharacter.Domain.Data.Entities.NonPlayerCharacter;

namespace Fracture.NonPlayerCharacter.Infrastructure.PersistenceLayer
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
