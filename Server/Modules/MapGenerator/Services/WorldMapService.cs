using Fracture.Server.Modules.MapGenerator;
using Fracture.Server.Modules.MapGenerator.Models.Map;

public class WorldMapService
{
    private readonly IMapRepository _mapRepository;
    private WorldMap? _worldMap;

    public WorldMapService(IMapRepository mapRepository)
    {
        _mapRepository = mapRepository;
    }

    public Task<WorldMap> GetOrGenerateWorldMapAsync()
    {
        if (_worldMap != null)
            return Task.FromResult(_worldMap);
        var mainMap = GetRandomMainMap();
        var generatedMap = new WorldMap { MainMap = mainMap };
        _worldMap = generatedMap;

        return Task.FromResult(_worldMap);
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
