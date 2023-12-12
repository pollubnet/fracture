using Fracture.AccountManagement.Application;
using Fracture.AccountManagement.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.AccountManagement.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountManagementModule(
            this IServiceCollection services
        )
        {
            services.AddApplication().AddInfrastructure();

            return services;
        }
    }
}
