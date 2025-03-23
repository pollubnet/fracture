namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class Map
{
    public string? Name { get; set; }

    public LocationType LocationType { get; set; }

    public List<LocationGroup> LocationGroups { get; set; } = new();

    public int Width { get; set; }
    public int Height { get; set; }
    public Node[,] Grid { get; set; }
}
