using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class WorldGenerationService(
    IMapParameterSelectorService parameterSelector,
    IMapGeneratorService mapGenerator,
    ISubMapAssignmentService subMapAssigner,
    ILogger<WorldGenerationService> logger
) : IWorldGenerationService
{
    public async Task<Map> GenerateWorldMapAsync()
    {
        logger.LogInformation("Generating main world...");
        var parameters = await parameterSelector.GetParametersAsync();
        var mainParameter = parameterSelector.SelectRandom(parameters, LocationType.MainLocation);
        var map = await mapGenerator.GetMap(mainParameter);

        await subMapAssigner.AssignSubMapsAsync(map, parameters);

        return map;
    }
}
