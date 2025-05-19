using System.Text.Json;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;

namespace Fracture.Server.Modules.MapGenerator.Models.Map.Town;

public class TownParameters
{
    private readonly ILogger<TownParameters> _logger;
    private readonly Dictionary<string, TownBiomeParam> _paramsDict;

    public TownParameters(ILogger<TownParameters> logger)
    {
        _logger = logger;
        _paramsDict = new Dictionary<string, TownBiomeParam>();
    }

    public TownBiomeParam Get(string name)
    {
        if (!_paramsDict.ContainsKey(name))
        {
            _logger.LogWarning($"Missing TownBiomeParam for biome: {name}");
            return new TownBiomeParam("Missing", 0, 0);
        }

        return _paramsDict[name];
    }

    public void Initialize(MapParameters mapParameters)
    {
        if (
            mapParameters.SubMapAssignmentLocations == null
            || mapParameters.BiomeCategories == null
        )
        {
            _logger.LogError("MapParameters is missing required data.");
            return;
        }

        foreach (var biome in mapParameters.BiomeCategories.SelectMany(b => b.Biomes))
        {
            if (biome.Locations == null)
            {
                _logger.LogError($"Biome '{biome.Name}' has no locations defined.");
                continue;
            }

            foreach (var location in biome.Locations)
            {
                if (mapParameters.SubMapAssignmentLocations.Contains(location.Name.ToString()))
                {
                    var townBiomeParam = new TownBiomeParam(
                        biome.Name,
                        location.Weight,
                        location.Mult
                    );
                    _paramsDict[biome.Name] = townBiomeParam;
                }
            }
        }

        _logger.LogInformation("Town parameters initialized successfully.");
    }
}

public record TownBiomeParam(string Name, int Weight, float Mult);
