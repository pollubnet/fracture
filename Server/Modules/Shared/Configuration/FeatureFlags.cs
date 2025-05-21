using Microsoft.FeatureManagement;

namespace Fracture.Server.Modules.Shared.Configuration;

public static class FeatureFlags
{
    public const string CONFIG_SECTION = "FeatureFlags";
    public const string USE_AI = "UseAI";

    public static IServiceCollection AddTransientIfFeatureEnabled<TInterface, TImplementation>(
        this IServiceCollection services,
        string featureName
    )
        where TImplementation : class, TInterface
    {
        return services.AddTransientIfFeatureEnabled(
            featureName,
            typeof(TInterface),
            typeof(TImplementation)
        );
    }

    public static IServiceCollection AddTransientIfFeatureEnabled(
        this IServiceCollection services,
        string featureName,
        Type serviceType,
        Type implementationType
    )
    {
        var featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManagerSnapshot>();

        if (featureManager.IsEnabledAsync(featureName).Result)
            return services.AddTransient(serviceType, implementationType);

        return services;
    }

    public static IServiceCollection AddSingletonIfFeatureEnabled<TInterface, TImplementation>(
        this IServiceCollection services,
        string featureName
    )
        where TImplementation : class, TInterface
    {
        return services.AddSingletonIfFeatureEnabled(
            featureName,
            typeof(TInterface),
            typeof(TImplementation)
        );
    }

    public static IServiceCollection AddSingletonIfFeatureEnabled(
        this IServiceCollection services,
        string featureName,
        Type serviceType,
        Type implementationType
    )
    {
        var featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManagerSnapshot>();

        if (featureManager.IsEnabledAsync(featureName).Result)
            return services.AddSingleton(serviceType, implementationType);

        return services;
    }

    public static IServiceCollection AddSingletonIfFeatureEnabled<TInterface>(
        this IServiceCollection services,
        string featureName,
        Func<IServiceProvider, TInterface> implementationFactory
    )
        where TInterface : class
    {
        var featureManager = services
            .BuildServiceProvider()
            .GetRequiredService<IFeatureManagerSnapshot>();

        if (featureManager.IsEnabledAsync(featureName).Result)
            return services.AddSingleton(typeof(TInterface), implementationFactory);

        return services;
    }
}
