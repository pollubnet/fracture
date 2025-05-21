using System.Text.Json;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class TownParameters
{
    private readonly Dictionary<string, TownBiomeParam> _paramsDict;
    private readonly ILogger<TownParameters> _logger;

    public TownParameters(ILogger<TownParameters> logger)
    {
        _logger = logger;
        _paramsDict = new Dictionary<string, TownBiomeParam>();
    }

    public TownBiomeParam Get(string name)
    {
        if (!_paramsDict.ContainsKey(name))
        {
            //Log
            return new TownBiomeParam("Missing", 0, 0);
        }
        return _paramsDict[name];
    }

    public void Initialize(string fileName)
    {
        var path = "Config/LocationParameters/Town/" + fileName + ".json";
        if (File.Exists(path))
        {
            try
            {
                var json = File.ReadAllText(path);
                var paramsList = JsonSerializer.Deserialize<List<TownBiomeParam>>(json);
                paramsList.ForEach(param => _paramsDict.Add(param.Name, param));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to load parameters");
            }
        }
    }
}

public record TownBiomeParam(string Name, int Weight, float Mult);
