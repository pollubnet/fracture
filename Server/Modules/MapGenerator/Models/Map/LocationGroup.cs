namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class LocationGroup
{
    public string GroupName { get; set; } = default!;
    public LocationType LocationType { get; set; }
    public List<Node> Nodes { get; set; } = new();

    public int Count => Nodes.Count;

    public (int minX, int minY, int maxX, int maxY) GetBounds()
    {
        var minX = Nodes.Min(n => n.XId);
        var minY = Nodes.Min(n => n.YId);
        var maxX = Nodes.Max(n => n.XId);
        var maxY = Nodes.Max(n => n.YId);
        return (minX, minY, maxX, maxY);
    }
}
