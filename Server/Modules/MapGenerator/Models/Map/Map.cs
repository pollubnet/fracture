using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class Map
{
    private readonly Random _rnd;

    public string? Name { get; set; }

    public LocationType LocationType { get; init; }

    public List<LocationGroup> LocationGroups { get; init; } = [];

    public int Width { get; init; }
    public int Height { get; init; }
    public required Node[,] Grid { get; init; }

    public Map(RandomProvider rndProvider)
    {
        _rnd = rndProvider.Random;
    }

    public Position GetRandomWalkableNode()
    {
        Position node;
        do
        {
            node = new Position() { X = _rnd.Next(0, Width), Y = _rnd.Next(0, Height) };
        } while (!Grid[node.X, node.Y].Walkable);

        return node;
    }
}
