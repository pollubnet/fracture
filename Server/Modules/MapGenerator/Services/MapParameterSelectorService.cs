using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapParameterSelectorService : IMapParameterSelectorService
{
    private readonly MapDataImportService _dataImportService;
    private readonly ILogger<MapParameterSelectorService> _logger;
    private readonly Random _rnd = new();

    public MapParameterSelectorService(
        MapDataImportService dataImportService,
        ILogger<MapParameterSelectorService> logger
    )
    {
        _dataImportService = dataImportService;
        _logger = logger;
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
