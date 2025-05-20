using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.MapGenerator.Models.Map.Town;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class WorldGenerationService : IWorldGenerationService
{
    private readonly IMapParameterSelectorService _parameterSelector;
    private readonly IMapGeneratorService _mapGenerator;
    private readonly ISubMapAssignmentService _subMapAssigner;
    private readonly ILogger<WorldGenerationService> _logger;

    public WorldGenerationService(
        IMapParameterSelectorService parameterSelector,
        IMapGeneratorService mapGenerator,
        ISubMapAssignmentService subMapAssigner,
        ILogger<WorldGenerationService> logger
    )
    {
        _parameterSelector = parameterSelector;
        _mapGenerator = mapGenerator;
        _subMapAssigner = subMapAssigner;
        _logger = logger;
    }

    public async Task<Map> GenerateWorldMapAsync()
    {
        var parameters = await _parameterSelector.GetParametersAsync();
        var mainParameter = _parameterSelector.SelectRandom(parameters, LocationType.MainLocation);
        var map = await _mapGenerator.GetMap(mainParameter);

        await _subMapAssigner.AssignSubMapsAsync(map, parameters);

        return map;
    }
}
