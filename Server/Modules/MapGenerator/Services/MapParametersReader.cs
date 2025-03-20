using System.Text.Json;
using System.Text.Json.Serialization;
using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapParametersReader
{
    private ILogger<MapParametersReader> _logger;

    public MapParametersReader(ILogger<MapParametersReader> logger)
    {
        _logger = logger;
    }

    public MapParameters? ReadMapParametersFromJson(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(), new ColorJsonConverter() },
        };
        try
        {
            if (!File.Exists(filePath))
            {
                _logger.LogCritical("File with map parameters does not exist");
                return null;
            }
            _logger.LogInformation("File with map was readed succesfully");
            var jsonContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<MapParameters>(jsonContent, options);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
