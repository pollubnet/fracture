using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class ItemPlacementService(RandomProvider rndProvider) : IItemPlacementService
{
    private readonly Random _rnd = rndProvider.Random;
    private const double ItemDensity = 0.01;

    public void PlaceItems(Map map)
    {
        var targetCount = Math.Max(1, (int)Math.Round(map.Width * map.Height * ItemDensity));
        var placed = 0;
        var attempts = 0;
        var maxAttempts = targetCount * 10;

        while (placed < targetCount && attempts < maxAttempts)
        {
            attempts++;

            var x = _rnd.Next(0, map.Width);
            var y = _rnd.Next(0, map.Height);
            var node = map.Grid[x, y];

            if (!node.Walkable || node.ItemDrop != null || node.LocationType != LocationType.None)
                continue;

            if (node.MapObject != null)
                continue;

            node.ItemDrop = new ItemDrop();
            placed++;
        }
    }
}
