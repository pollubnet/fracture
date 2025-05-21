using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapDataImportService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MapDataImportService> _logger;
    private readonly MapParametersReader _mapParametersReader;

    public MapDataImportService(
        ILogger<MapDataImportService> logger,
        IConfiguration configuration,
        MapParametersReader mapParametersReader
    )
    {
        _logger = logger;
        _configuration = configuration;
        _mapParametersReader = mapParametersReader;
    }

    private Dictionary<string, string>? GetFolderStructure()
    {
        var folderStructureSection = _configuration.GetSection("MapSettings:MapFolderStructure");
        return folderStructureSection.Get<Dictionary<string, string>>();
    }

    private bool ValidateBasePath(string basePath)
    {
        if (string.IsNullOrWhiteSpace(basePath))
        {
            _logger.LogError("Missing 'MapSettings:MapFolderPath' in configuration.");
            return false;
        }

        return true;
    }

    public async Task<Dictionary<string, MapParameters>> ImportMapParametersAsync()
    {
        var parametersDictionary = new Dictionary<string, MapParameters>();

        var basePath = _configuration["MapSettings:MapFolderPath"];
        if (basePath != null && !ValidateBasePath(basePath))
            return parametersDictionary;

        var folderStructure = GetFolderStructure();
        if (folderStructure == null || !folderStructure.Any())
        {
            _logger.LogError("MapFolderStructure section is missing or empty in configuration.");
            return parametersDictionary;
        }

        foreach (var (key, relativePath) in folderStructure)
            if (basePath != null)
                await ProcessFolderAsync(key, relativePath, basePath, parametersDictionary);

        _logger.LogInformation("Map parameters import completed.");
        return parametersDictionary;
    }

    private async Task ProcessFolderAsync(
        string key,
        string relativePath,
        string basePath,
        Dictionary<string, MapParameters> parametersDictionary
    )
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            _logger.LogWarning($"Empty or null path in MapFolderStructure.{key}");
            return;
        }

        var fullPath = Path.Combine(basePath, relativePath);
        if (!Directory.Exists(fullPath))
        {
            _logger.LogWarning($"Directory not found: {fullPath}");
            return;
        }

        var files = Directory.GetFiles(fullPath, "*.json");
        if (files.Length == 0)
        {
            _logger.LogInformation($"No JSON files found in: {fullPath}");
            return;
        }

        _logger.LogInformation($"Processing folder '{key}': {fullPath}");
        await ProcessJsonFilesAsync(files, parametersDictionary);
    }

    private async Task ProcessJsonFilesAsync(
        string[] files,
        Dictionary<string, MapParameters> parametersDictionary
    )
    {
        foreach (var file in files)
            await ProcessFileAsync(file, parametersDictionary);
    }

    private Task ProcessFileAsync(
        string file,
        Dictionary<string, MapParameters> parametersDictionary
    )
    {
        try
        {
            var parameters = _mapParametersReader.ReadMapParametersFromJson(file);
            if (parameters == null)
            {
                _logger.LogWarning($"Invalid parameters in file: {file}");
                return Task.CompletedTask;
            }
            var key = Path.GetFileNameWithoutExtension(file);
            parametersDictionary[key] = parameters;
            _logger.LogInformation($"Parameters from file '{file}' successfully imported.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error processing file: {file}");
        }

        return Task.CompletedTask;
    }
}
