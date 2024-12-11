using System.Text.Json;
using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapParametersService
{
    public MapParameters? ReadMapParametersFromJson(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new EnumJsonConverter<TerrainType>(),
                new EnumJsonConverter<BiomeType>(),
                new ColorJsonConverter(),
            },
        };
        try
        {
            if (!File.Exists(filePath))
                return null;
            var jsonContent = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<MapParameters>(jsonContent, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return null;
        }
    }
}
