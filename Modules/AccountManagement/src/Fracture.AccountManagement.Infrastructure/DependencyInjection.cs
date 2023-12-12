using Fracture.AccountManagement.Domain.Repositories;
using Fracture.AccountManagement.Infrastructure.PersistenceLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.AccountManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
