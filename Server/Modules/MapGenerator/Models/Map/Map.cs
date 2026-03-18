namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class Map
{
    public string? Name { get; set; }

    public LocationType LocationType { get; init; }

    public List<LocationGroup> LocationGroups { get; init; } = [];

    public int Width { get; init; }
    public int Height { get; init; }
    public required Node[,] Grid { get; init; }

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
