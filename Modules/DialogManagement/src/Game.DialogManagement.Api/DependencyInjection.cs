using Game.DialogManagement.Application;
using Game.DialogManagement.Infrastracture;
using Microsoft.Extensions.DependencyInjection;

namespace Game.DialogManagement.Api
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
