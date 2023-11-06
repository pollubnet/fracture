using Game.DialogManagement.Application;
using Game.DialogManagement.Infrastracture;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DialogManagement.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDialogManagementModule(this IServiceCollection services)
        {
            services.AddApplication().AddInfrastructure();

            return services;
        }
    }
}
