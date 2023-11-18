using Game.Shared.External.Providers.Ai;
using Game.Shared.External.Providers.AI;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Shared.External
{
    /// <summary>
    /// Extensions for the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the shared external modules into the dependency injection container.
        /// </summary>
        public static IServiceCollection AddAiProvider<TProvider>(this IServiceCollection services)
            where TProvider : class, IAiBackendProvider
        {
            services.AddSingleton<IAiBackendProvider, TProvider>();
            return services;
        }
    }
}
