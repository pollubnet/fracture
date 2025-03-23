using Fracture.Server.Modules.MapGenerator;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Hangfire;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapManagerService
{
    private readonly IMapRepository _mapRepository;
    private readonly object _lock = new();
    private readonly IRecurringJobManager _jobManager;
    private readonly ILogger<MapManagerService> _logger;
    private readonly MapDataImportService _mapDataImportService;

    public Map CurrentMap { get; set; }

    public MapManagerService(
        IMapRepository mapRepository,
        IRecurringJobManager jobManager,
        ILogger<MapManagerService> logger,
        MapDataImportService mapDataImportService
    )
    {
        _jobManager = jobManager;
        _logger = logger;
        _mapDataImportService = mapDataImportService;
        _mapRepository = mapRepository;
    }

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

    public bool HasMap()
    {
        lock (_lock)
        {
            return CurrentMap != null;
        }
    }

    public Task<Map> SetRandomMainMapAsync()
    {
        SetWorldMap(GetRandomMainMap());
        return Task.FromResult(GetRandomMainMap());
    }

    private Map GetRandomMainMap()
    {
        var mainMaps = _mapRepository.GetAllMapsByLocation(LocationType.MainLocation).ToList();
        if (!mainMaps.Any())
            throw new InvalidOperationException("No main maps found!");

        var random = new Random();
        return mainMaps[random.Next(mainMaps.Count)];
    }

    public async Task InitializeAndScheduleMapsAsync()
    {
        _mapRepository.ClearMaps();
        await _mapDataImportService.ImportMapsAsync();

        _jobManager.AddOrUpdate<MapManagerService>(
            "cykliczny-import-map",
            service => service.RefreshMapsAndSetNewAsync(),
            Cron.Minutely
        );
        _logger.LogInformation(
            "Mapy zostały zainicjalizowane, a zadanie cykliczne zostało zarejestrowane."
        );
        CurrentMap = GetRandomMainMap();
    }

    public async Task RefreshMapsAndSetNewAsync()
    {
        _logger.LogInformation("Rozpoczynanie importu map...");

        await _mapDataImportService.ImportMapsAsync();

        _logger.LogInformation("Import zakończony. Wybieranie nowej mapy...");

        var newMap = GetRandomMainMap();
        SetWorldMap(newMap);

        _logger.LogInformation($"Nowa mapa ustawiona: {newMap.Name}");
    }
}
