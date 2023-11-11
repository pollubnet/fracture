using Game.DialogManagement.Application;
using Game.DialogManagement.Infrastracture;
using Microsoft.Extensions.DependencyInjection;

namespace Game.DialogManagement.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDialogManagementModule(this IServiceCollection services)
        {
            // TODO: This should be passed from within the appsettings.
            const string connectionString = "192.168.0.136,password=password";
            services.AddApplication().AddInfrastructure(connectionString);

            return services;
        }
    }
}
