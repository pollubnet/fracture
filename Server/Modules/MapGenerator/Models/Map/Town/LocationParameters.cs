using System.Text.Json;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;

namespace Fracture.Server.Modules.MapGenerator.Models.Map.Town;

public class LocationParameters
{
    private readonly ILogger<LocationParameters> _logger;
    private readonly Dictionary<string, LocationBiomeParam> _paramsDict;

    public LocationParameters(ILogger<LocationParameters> logger)
    {
        _logger = logger;
        _paramsDict = new Dictionary<string, LocationBiomeParam>();
    }

    public LocationBiomeParam Get(string name)
    {
        if (!_paramsDict.ContainsKey(name))
        {
            if (_paramsDict.Count == 0)
                _logger.LogWarning(
                    "No LocationBiomeParams were initialized for the current submap."
                );
            return new LocationBiomeParam("Missing", 0, 0);
        }

        return _paramsDict[name];
    }

    public void Initialize(MapParameters mapParameters, string subMapName)
    {
        _paramsDict.Clear();

        if (mapParameters.BiomeCategories == null)
        {
            _logger.LogError("MapParameters is missing BiomeCategories.");
            return;
        }

        foreach (var biome in mapParameters.BiomeCategories.SelectMany(b => b.Biomes))
        {
            if (biome.Locations == null)
            {
                _logger.LogWarning($"Biome '{biome.Name}' has no locations defined.");
                continue;
            }

            foreach (var location in biome.Locations)
            {
                if (string.Equals(location.Name, subMapName, StringComparison.OrdinalIgnoreCase))
                {
                    var param = new LocationBiomeParam(biome.Name, location.Weight, location.Mult);
                    _paramsDict[biome.Name] = param;
                }
            }
        }
    }
}

public record LocationBiomeParam(string Name, int Weight, float Mult);
