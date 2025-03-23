namespace Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

public class LocationMapObject : IMapObject
{
    public string Type { get; set; }
    public Map subMap { get; set; }
}
