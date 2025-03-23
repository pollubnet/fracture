namespace Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

public class LocationMapObject : IMapObject
{
    public Map? SubMap { get; set; }
    public string Type { get; set; } = "Location";

    public bool IsInteractive { get; set; }
}
