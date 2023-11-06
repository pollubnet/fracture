using Game.AccountManagement.Application;
using Game.AccountManagement.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.AccountManagement.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountMangementModule(this IServiceCollection services)
        {
            services.AddApplication().AddInfrastructure();

            return services;
        }
    }
}
