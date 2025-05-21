using System.Text.Json;
using System.Text.Json.Serialization;
using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapParametersService
{
    private ILogger<MapParametersService> _logger;

    public MapParametersService(ILogger<MapParametersService> logger)
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
