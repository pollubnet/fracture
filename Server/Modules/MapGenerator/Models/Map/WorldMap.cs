namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class WorldMap
{
    public Map MainMap { get; set; }
    public List<Map> SubMaps { get; set; } = new();
}
