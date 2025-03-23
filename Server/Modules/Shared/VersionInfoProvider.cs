namespace Fracture.Server.Modules.Shared;

internal class VersionInfoProvider
{
    public VersionInfoProvider()
    {
        var lines = File.ReadAllLines("version.txt");
        ShortVersion = lines[0];
        InformationalVersion = $"{lines[0]}-{lines[1]}";
    }

    public string InformationalVersion { get; init; }

    public string ShortVersion { get; init; }
}
