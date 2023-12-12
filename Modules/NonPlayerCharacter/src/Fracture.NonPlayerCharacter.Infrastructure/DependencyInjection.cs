using Fracture.NonPlayerCharacter.Infrastructure.PersistenceLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.NonPlayerCharacter.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string NPCDbContextConnectionString
        )
        {
            services.AddDbContext<NonPlayerCharacterDbContext>(options =>
            {
                options.UseNpgsql(NPCDbContextConnectionString);
            });

            return services;
        }
    }
}
