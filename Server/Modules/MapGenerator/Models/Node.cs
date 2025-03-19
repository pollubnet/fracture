using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Fracture.Server.Modules.Pathfinding.Models;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class Node : IPathfindingNode
{
    [Required]
    public int XId { get; set; }

    [Required]
    public int YId { get; set; }

    [Required]
    public Biome Biome { get; set; }

    [Required]
    public TerrainType TerrainType { get; set; }

    [Required]
    public float NoiseValue { get; set; }

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public bool Walkable { get; set; }
    public IPathfindingNode? PreviousNode { get; set; }

    public Node(int xId, int yId, Biome biome)
    {
        XId = xId;
        YId = yId;
        Biome = biome;
    }
}
