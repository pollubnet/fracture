﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fracture.NonPlayerCharacter.Application;
using Fracture.NonPlayerCharacter.Infrastructure;

namespace Fracture.NonPlayerCharacter.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNonPlayerCharacterModule(
            this IServiceCollection services,
            string NPCDbContextConnectionString
        )
        {
            services.AddApplication().AddInfrastructure(NPCDbContextConnectionString);

            return services;
        }
    }
}
