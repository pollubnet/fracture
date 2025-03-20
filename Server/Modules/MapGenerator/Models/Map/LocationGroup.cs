namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class LocationGroup
{
    public string GroupName { get; set; } = default!;
    public LocationType LocationType { get; set; }
    public List<Node> Nodes { get; set; } = new();

    public int Count => Nodes.Count;

    public (int minX, int minY, int maxX, int maxY) GetBounds()
    {
        int minX = Nodes.Min(n => n.XId);
        int minY = Nodes.Min(n => n.YId);
        int maxX = Nodes.Max(n => n.XId);
        int maxY = Nodes.Max(n => n.YId);
        return (minX, minY, maxX, maxY);
    }
}
