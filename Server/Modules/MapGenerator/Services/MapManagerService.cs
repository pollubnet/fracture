using Fracture.Server.Modules.MapGenerator.Models.Map;
using Hangfire;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapManagerService(
    IMapRepository mapRepository,
    ILogger<MapManagerService> logger,
    IWorldGenerationService worldGenerationService
)
{
    private readonly Lock _lock = new();

    public Map CurrentMap { get; private set; } = default!;

    public Map? GetWorldMap()
    {
        lock (_lock)
        {
            return CurrentMap;
        }
    }

    public void SetWorldMap(Map map)
    {
        lock (_lock)
        {
            CurrentMap = map;
        }
    }

    public async Task GenerateWorldAsync()
    {
        mapRepository.ClearMaps();
        var map = await worldGenerationService.GenerateWorldMapAsync();
        mapRepository.SaveMap(map);
        SetWorldMap(map);

        logger.LogInformation("Maps initialized.");
    }

    public async Task RefreshMapsAndSetNewAsync()
    {
        logger.LogInformation("Generating new world...");

        var newMap = await worldGenerationService.GenerateWorldMapAsync();
        mapRepository.SaveMap(newMap);
        SetWorldMap(newMap);

        logger.LogInformation($"New map set: {newMap.Name}");
    }
}
