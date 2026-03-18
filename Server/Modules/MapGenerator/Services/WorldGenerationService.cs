using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class WorldGenerationService(
    IMapParameterSelectorService parameterSelector,
    IMapGeneratorService mapGenerator,
    ISubMapAssignmentService subMapAssigner,
    INameGenerator nameGenerator,
    ILogger<WorldGenerationService> logger
) : IWorldGenerationService
{
    public async Task<Map> GenerateWorldMapAsync()
    {
        logger.LogInformation("Generating main world...");
        var parameters = await parameterSelector.GetParametersAsync();

        // generate main map
        var mainParameter = parameterSelector.SelectRandom(parameters, LocationType.MainLocation);
        var map = await mapGenerator.GetMap(mainParameter);

        // generate submaps
        await subMapAssigner.AssignSubMapsAsync(map, parameters);

        // rename locations
        foreach (var item in map.LocationGroups)
        {
            item.GroupName = item.LocationType switch
            {
                LocationType.MainLocation => "The World",
                LocationType.Town => "Town " + await nameGenerator.GenerateNameAsync(),
                LocationType.Cave => "Cave " + await nameGenerator.GenerateNameAsync(),
                _ => item.GroupName,
            };

            foreach (var node in item.Nodes)
            {
                node.GroupName = item.GroupName;
            }
        }

        return map;
    }
}
