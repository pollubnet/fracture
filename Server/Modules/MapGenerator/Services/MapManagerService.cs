using Fracture.Server.Modules.MapGenerator.Models.Map;
using Hangfire;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapManagerService
{
    private readonly IRecurringJobManager _jobManager;
    private readonly object _lock = new();
    private readonly ILogger<MapManagerService> _logger;
    private readonly IMapRepository _mapRepository;
    private readonly IWorldGenerationService _worldGenerationService;

    public MapManagerService(
        IWorldGenerationService worldGenerationService,
        IRecurringJobManager jobManager,
        IMapRepository mapRepository,
        ILogger<MapManagerService> logger
    )
    {
        _worldGenerationService = worldGenerationService;
        _jobManager = jobManager;
        _mapRepository = mapRepository;
        _logger = logger;
    }

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

    public async Task InitializeAndScheduleMapsAsync()
    {
        _mapRepository.ClearMaps();
        var map = await _worldGenerationService.GenerateWorldMapAsync();
        _mapRepository.SaveMap(map);
        SetWorldMap(map);

        _jobManager.AddOrUpdate<MapManagerService>(
            "cykliczny-import-map",
            service => service.RefreshMapsAndSetNewAsync(),
            Cron.Minutely
        );
        _logger.LogInformation(
            "Mapy zostały zainicjalizowane, a zadanie cykliczne zostało zarejestrowane."
        );
    }

    public async Task RefreshMapsAndSetNewAsync()
    {
        _logger.LogInformation("Rozpoczynanie generowania nowej mapy...");

        // Poprawione wywołanie metody GenerateWorldMapAsync()
        var newMap = await _worldGenerationService.GenerateWorldMapAsync();
        _mapRepository.SaveMap(newMap);
        SetWorldMap(newMap);

        _logger.LogInformation($"Nowa mapa ustawiona: {newMap.Name}");
    }
}
