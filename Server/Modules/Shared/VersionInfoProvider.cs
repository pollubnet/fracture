namespace Fracture.Server.Modules.Shared;

internal class VersionInfoProvider
{
    public VersionInfoProvider()
    {
        if (File.Exists("version.txt"))
        {
            var lines = File.ReadAllLines("version.txt");
            ShortVersion = lines[0];
            InformationalVersion = $"{lines[0]}-{lines[1]}";
        }
        else
        {
            ShortVersion = "1";
            InformationalVersion = "1-unknown";
        }
    }

    public string InformationalVersion { get; init; }

    public string ShortVersion { get; init; }
}
