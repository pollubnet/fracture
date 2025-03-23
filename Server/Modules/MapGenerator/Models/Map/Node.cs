using System.ComponentModel.DataAnnotations;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.Pathfinding.Models;

namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class Node : IPathfindingNode
{
    [Required]
    public int XId { get; set; }

    [Required]
    public int YId { get; set; }

    [Required]
    public Biome.Biome Biome { get; set; }

    [Required]
    public TerrainType TerrainType { get; set; }

    [Required]
    public float NoiseValue { get; set; }

    public string GroupName { get; set; }

    public LocationType LocationType { get; set; } = LocationType.None;
    public IMapObject MapObject { get; set; }

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost => GCost + HCost;
    public bool Walkable { get; set; }
    public IPathfindingNode? PreviousNode { get; set; }

    public Node(int xId, int yId, Biome.Biome biome)
    {
        XId = xId;
        YId = yId;
        Biome = biome;
    }
}
