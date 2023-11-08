using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Game.DialogManagement.Domain.Repositories;
using Game.DialogManagement.Infrastracture.PersistenceLayer.Repositories;

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
            return services;
        }
    }
}
