namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class Map
{
    public string? Name { get; set; }

    public LocationType LocationType { get; set; }

    public List<LocationGroup> LocationGroups { get; set; } = new();

    public int Width { get; set; }
    public int Height { get; set; }
    public required Node[,] Grid { get; set; }

    public Position GetRandomWalkableNode()
    {
        var rnd = new Random();

        Position node;
        do
        {
            node = new Position() { X = rnd.Next(0, Width), Y = rnd.Next(0, Height) };
        } while (!Grid[node.X, node.Y].Walkable);

        return node;
    }
}
