using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapParameterSelectorService : IMapParameterSelectorService
{
    private readonly MapDataImportService _dataImportService;
    private readonly ILogger<MapParameterSelectorService> _logger;
    private readonly Random _rnd;

    public MapParameterSelectorService(
        MapDataImportService dataImportService,
        ILogger<MapParameterSelectorService> logger,
        RandomProvider rndProvider
    )
    {
        _dataImportService = dataImportService;
        _logger = logger;
        _rnd = rndProvider.Random;
    }

    public async Task<Dictionary<string, MapParameters>> GetParametersAsync() =>
        await _dataImportService.ImportMapParametersAsync();

    public MapParameters SelectRandom(
        Dictionary<string, MapParameters> parameters,
        LocationType type
    )
    {
        var filtered = parameters.Where(p => p.Value.LocationType == type).ToList();
        if (!filtered.Any())
            throw new Exception($"No parameters found for {type}");
        return filtered[_rnd.Next(filtered.Count)].Value;
    }
}
