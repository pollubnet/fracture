using Fracture.Server.Modules.MapGenerator;
using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class WorldMapService
{
    private readonly IMapRepository _mapRepository;
    private readonly object _lock = new();

    private WorldMap? _currentWorldMap;

    public WorldMapService(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public WorldMap? GetWorldMap()
    {
        lock (_lock)
        {
            return _currentWorldMap;
        }
    }

    public void SetWorldMap(WorldMap map)
    {
        lock (_lock)
        {
            _currentWorldMap = map;
        }
    }

    public bool HasWorldMap()
    {
        lock (_lock)
        {
            return _currentWorldMap != null;
        }
    }

    public Task<WorldMap> GetOrGenerateWorldMapAsync()
    {
        if (HasWorldMap())
            return Task.FromResult(_currentWorldMap!);
        var worldMap = new WorldMap { MainMap = GetRandomMainMap() };

        SetWorldMap(worldMap);

        return Task.FromResult(worldMap);
    }

    private Map GetRandomMainMap()
    {
        var mainMaps = _mapRepository.GetAllMapsByLocation(LocationType.MainLocation).ToList();
        if (!mainMaps.Any())
            throw new InvalidOperationException("No main maps found!");

        var random = new Random();
        return mainMaps[random.Next(mainMaps.Count)];
    }
}
