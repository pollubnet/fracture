using System.Collections.Concurrent;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.Shared;
using Microsoft.EntityFrameworkCore;

namespace Fracture.Server.Modules.Users.Services;

public class ItemDropStateService
{
    private static readonly HashSet<Position> EmptyPositions = new();

    private readonly IItemPlacementService _itemPlacementService;
    private readonly FractureDbContext _dbContext;
    private readonly ConcurrentDictionary<string, HashSet<Position>> _baseDropsByMap = new();
    private readonly ConcurrentDictionary<string, HashSet<Position>> _collectedDropsByUserSeed =
        new();
    private readonly Lock _lock = new();

    public ItemDropStateService(
        IItemPlacementService itemPlacementService,
        FractureDbContext dbContext
    )
    {
        _itemPlacementService = itemPlacementService;
        _dbContext = dbContext;
    }

    public bool HasItemDrop(int userId, Map map, int x, int y)
    {
        var position = new Position(x, y);
        var baseDrops = GetBaseDrops(map);
        var collected = GetCollectedDropsIfLoaded(userId);

        lock (_lock)
        {
            return baseDrops.Contains(position) && !collected.Contains(position);
        }
    }

    public async Task EnsureLoadedAsync(int userId)
    {
        await GetCollectedDropsAsync(userId);
    }

    public async Task<bool> TryCollectAsync(int userId, Map map, int x, int y)
    {
        var position = new Position(x, y);
        var baseDrops = GetBaseDrops(map);
        var collected = await GetCollectedDropsAsync(userId);

        lock (_lock)
        {
            if (!baseDrops.Contains(position) || collected.Contains(position))
                return false;

            collected.Add(position);
        }

        _dbContext.ItemsDropped.Add(
            new ItemDropped
            {
                UserId = userId,
                MapSeed = RandomProvider.Seed,
                X = x,
                Y = y,
            }
        );

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task AttachItemAsync(int userId, Map map, int x, int y, int itemId)
    {
        var mapSeed = RandomProvider.Seed;
        var entry = await _dbContext.ItemsDropped.FirstOrDefaultAsync(d =>
            d.UserId == userId && d.MapSeed == mapSeed && d.X == x && d.Y == y
        );

        if (entry == null)
            return;

        entry.ItemId = itemId;
        await _dbContext.SaveChangesAsync();
    }

    private HashSet<Position> GetBaseDrops(Map map)
    {
        var mapKey = GetMapKey(map);
        return _baseDropsByMap.GetOrAdd(
            mapKey,
            _ => new HashSet<Position>(_itemPlacementService.GenerateDropPositions(map))
        );
    }

    private HashSet<Position> GetCollectedDropsIfLoaded(int userId)
    {
        var userSeedKey = GetUserSeedKey(userId);
        return _collectedDropsByUserSeed.TryGetValue(userSeedKey, out var collected)
            ? collected
            : EmptyPositions;
    }

    private async Task<HashSet<Position>> GetCollectedDropsAsync(int userId)
    {
        var userSeedKey = GetUserSeedKey(userId);
        if (_collectedDropsByUserSeed.TryGetValue(userSeedKey, out var cached))
            return cached;

        var positions = await _dbContext
            .ItemsDropped.AsNoTracking()
            .Where(d => d.UserId == userId && d.MapSeed == RandomProvider.Seed)
            .Select(d => new Position(d.X, d.Y))
            .ToListAsync();

        var collected = new HashSet<Position>(positions);
        _collectedDropsByUserSeed[userSeedKey] = collected;
        return collected;
    }

    private static string GetMapKey(Map map)
    {
        return map.Name ?? $"{map.LocationType}:{map.Width}x{map.Height}";
    }

    private static string GetUserSeedKey(int userId)
    {
        return $"{userId}:{RandomProvider.Seed}";
    }
}
