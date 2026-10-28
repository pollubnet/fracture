using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class NpcGeneratorService(RandomProvider randomProvider)
{
    /// <summary>
    /// Generates, and places a single NPC on a map, on a random walkable node
    /// </summary>
    /// <param name="map"></param>
    public void PlaceNpc(Map map)
    {
        var candidates = map
            .Grid.Cast<Node>()
            .Where(node =>
                node.Walkable && node.LocationType == LocationType.None && node.MapObject == null
            )
            .ToList();

        if (candidates.Count == 0)
        {
            return;
        }

        var node = candidates[randomProvider.Random.Next(candidates.Count)];
        node.MapObject = new NpcMapObject
        {
            Id = "npc-" + randomProvider.Random.Next(1000),
            IsInteractive = true,
        };
    }
}
