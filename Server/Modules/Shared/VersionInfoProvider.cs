using System.Reflection;

namespace Fracture.Server.Modules.Shared;

class VersionInfoProvider
{
    public VersionInfoProvider()
    {
        InformationalVersion =
            Assembly
                .GetEntryAssembly()
                ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion ?? string.Empty;

        ShortVersion = InformationalVersion.Substring(0, InformationalVersion.IndexOf(".Branch"));
    }

    public string InformationalVersion { get; init; }

    public string ShortVersion { get; init; }
}
