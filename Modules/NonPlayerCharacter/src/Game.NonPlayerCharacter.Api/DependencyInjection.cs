using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.NonPlayerCharacter.Application;
using Game.NonPlayerCharacter.Infrastructure;

namespace Game.NonPlayerCharacter.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNonPlayerCharacterModule(
            this IServiceCollection services
        )
        {
            services.AddApplication().AddInfrastructure();

            return services;
        }
    }
}
