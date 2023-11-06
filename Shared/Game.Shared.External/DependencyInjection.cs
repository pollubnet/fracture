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
        public static IServiceCollection AddSharedExternal(this IServiceCollection services)
        {
            return services;
        }
    }
}
