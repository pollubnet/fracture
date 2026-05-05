using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class ItemPlacementService(RandomProvider rndProvider) : IItemPlacementService
{
    private const double ItemDensity = 0.01;

    public IReadOnlySet<Position> GenerateDropPositions(Map map)
    {
        var rnd = new Random(RandomProvider.Seed);
        var targetCount = Math.Max(1, (int)Math.Round(map.Width * map.Height * ItemDensity));
        var attempts = 0;
        var maxAttempts = targetCount * 10;
        var drops = new HashSet<Position>();

        while (drops.Count < targetCount && attempts < maxAttempts)
        {
            attempts++;

            var x = rnd.Next(0, map.Width);
            var y = rnd.Next(0, map.Height);
            var node = map.Grid[x, y];

            if (!node.Walkable || node.LocationType != LocationType.None)
                continue;

            if (node.MapObject != null)
                continue;

            drops.Add(new Position(x, y));
        }

        return drops;
    }
}
