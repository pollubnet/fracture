using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Fracture.DialogManagement.Domain.Repositories;
using Fracture.DialogManagement.Infrastracture.PersistenceLayer.Repositories;
using Fracture.DialogManagement.Domain.Providers;
using Fracture.DialogManagement.Infrastracture.Providers;

namespace Fracture.DialogManagement.Infrastracture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string redisConnectionString
        )
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString)
            );
            services.AddScoped<IDialogueRepository, RedisDialogueRepository>();
            services.AddSingleton<IAiChatProvider, AiChatProvider>();
            return services;
        }
    }
}
