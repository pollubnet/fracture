using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Game.DialogManagement.Domain.Repositories;
using Game.DialogManagement.Infrastracture.PersistenceLayer.Repositories;
using Game.DialogManagement.Domain.Providers;
using Game.DialogManagement.Infrastracture.Providers;

namespace Game.DialogManagement.Infrastracture
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
