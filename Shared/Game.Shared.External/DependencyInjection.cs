using Game.Shared.External.Providers.Ai;
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
            return services.AddAiProvider(typeof(TProvider));
        }

        /// <summary>
        /// Adds the shared external modules into the dependency injection container.
        /// </summary>
        public static IServiceCollection AddAiProvider(
            this IServiceCollection services,
            Type provider
        )
        {
            services.AddSingleton(typeof(IAiBackendProvider), provider);
            return services;
        }

        /// <summary>
        /// Adds the shared external modules into the dependency injection container.
        /// </summary>
        public static IServiceCollection AddPromptTemplateProvider<TProvider>(
            this IServiceCollection services
        )
            where TProvider : class, IPromptTemplateProvider
        {
            return services.AddAiProvider(typeof(TProvider));
        }

        /// <summary>
        /// Adds the shared external modules into the dependency injection container.
        /// </summary>
        public static IServiceCollection AddPromptTemplateProvider(
            this IServiceCollection services,
            Type provider
        )
        {
            services.AddSingleton(typeof(IPromptTemplateProvider), provider);
            return services;
        }
    }
}
