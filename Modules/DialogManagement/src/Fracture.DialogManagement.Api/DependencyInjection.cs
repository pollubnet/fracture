using Fracture.DialogManagement.Application;
using Fracture.DialogManagement.Infrastracture;
using Microsoft.Extensions.DependencyInjection;

namespace Fracture.DialogManagement.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDialogManagementModule(
            this IServiceCollection services,
            string redisConnectionString
        )
        {
            services.AddApplication().AddInfrastructure(redisConnectionString);

            return services;
        }
    }
}
