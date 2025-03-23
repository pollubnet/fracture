namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapDataImportService
{
    private readonly ILogger<MapDataImportService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMapGeneratorService _mapGenerator;
    private readonly IMapRepository _mapRepository;
    private readonly MapParametersReader _mapParametersReader;

    public MapDataImportService(
        ILogger<MapDataImportService> logger,
        IConfiguration configuration,
        IMapGeneratorService mapGenerator,
        IMapRepository mapRepository,
        MapParametersReader mapParametersReader
    )
    {
        _logger = logger;
        _configuration = configuration;
        _mapGenerator = mapGenerator;
        _mapRepository = mapRepository;
        _mapParametersReader = mapParametersReader;
    }

    public async Task ImportMapsAsync()
    {
        _mapRepository.ClearMaps();
        var folderPath = _configuration["MapFolderPath"];
        if (!Directory.Exists(folderPath))
        {
            _logger.LogWarning($"Folder '{folderPath}' doesn't exist.");
            return;
        }

        var files = Directory.GetFiles(folderPath, "*.json");
        foreach (var file in files)
        {
            try
            {
                var parameters = _mapParametersReader.ReadMapParametersFromJson(file);
                if (parameters == null)
                {
                    _logger.LogWarning($"Invalid parameters in file: {file}");
                    continue;
                }

                var map = await _mapGenerator.GetMap(parameters);
                var name =
                    $"map_{Path.GetFileNameWithoutExtension(file)}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
                _mapRepository.SaveMap(name, map);
                _logger.LogInformation($"Map '{name}' successfully generated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing file: {file}");
            }
        }

        _logger.LogInformation("Map import completed.");
    }
}
