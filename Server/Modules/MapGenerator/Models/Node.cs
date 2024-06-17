using Fracture.Server.Modules.Pathfinding.Models;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class Node : IPathfindingNode
{
    public int XId { get; set; }
    public int YId { get; set; }
    public Biome? Biome { get; set; }

    public string? Color { get; set; }
    public float NoiseValue { get; set; }

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public bool Walkable { get; set; }
    public IPathfindingNode? PreviousNode { get; set; }

    public Node(int xId, int yId, Biome? biome)
    {
        XId = xId;
        YId = yId;
        Biome = biome;
    }
}
