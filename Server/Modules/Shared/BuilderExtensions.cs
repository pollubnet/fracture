using Fracture.Server.Modules.AI.Services;
using Fracture.Server.Modules.Shared.Configuration;
using Microsoft.FeatureManagement;

namespace Fracture.Server.Modules.Shared
{
    public static class BuilderExtensions
    {
        /// <summary>
        /// Adds to services elements only if there is a proper feature flag enabled in the configuration
        /// </summary>
        public static async Task AddFeatureGatedServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var featureManager = serviceProvider.GetRequiredService<IFeatureManager>();
            if (await featureManager.IsEnabledAsync(FeatureFlags.USE_AI))
            {
                services.AddSingleton<
                    IAIInstructionProvider,
                    OpenAICompatibleInstructionProvider
                >();
            }
        }
    }
}
