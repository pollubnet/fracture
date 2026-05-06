using System.Collections.Concurrent;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;

namespace Fracture.Server.Modules.Users.Services;

public class ItemDropStateService
{
    private readonly IItemPlacementService _itemPlacementService;
    private readonly ConcurrentDictionary<string, HashSet<Position>> _baseDropsByMap = new();
    private readonly ConcurrentDictionary<string, HashSet<Position>> _collectedDropsByUserMap =
        new();
    private readonly Lock _lock = new();

    public ItemDropStateService(IItemPlacementService itemPlacementService)
    {
        _itemPlacementService = itemPlacementService;
    }

    public bool HasItemDrop(int userId, Map map, int x, int y)
    {
        var position = new Position(x, y);
        var baseDrops = GetBaseDrops(map);
        var collected = GetCollectedDrops(userId, map);

        lock (_lock)
        {
            return baseDrops.Contains(position) && !collected.Contains(position);
        }
    }

    public bool TryCollect(int userId, Map map, int x, int y)
    {
        var position = new Position(x, y);
        var baseDrops = GetBaseDrops(map);
        var collected = GetCollectedDrops(userId, map);

        lock (_lock)
        {
            if (!baseDrops.Contains(position) || collected.Contains(position))
                return false;

            collected.Add(position);
            return true;
        }
    }

    private HashSet<Position> GetBaseDrops(Map map)
    {
        var mapKey = GetMapKey(map);
        return _baseDropsByMap.GetOrAdd(
            mapKey,
            _ => new HashSet<Position>(_itemPlacementService.GenerateDropPositions(map))
        );
    }

    private HashSet<Position> GetCollectedDrops(int userId, Map map)
    {
        var userMapKey = GetUserMapKey(userId, map);
        return _collectedDropsByUserMap.GetOrAdd(userMapKey, _ => new HashSet<Position>());
    }

    private static string GetMapKey(Map map)
    {
        return map.Name ?? $"{map.LocationType}:{map.Width}x{map.Height}";
    }

    private static string GetUserMapKey(int userId, Map map)
    {
        return $"{userId}:{GetMapKey(map)}";
    }
}
