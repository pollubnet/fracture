using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class SubMapAssignmentService : ISubMapAssignmentService
{
    private readonly ILocationWeightGeneratorService _weightGenerator;
    private readonly ILocationGeneratorService _locationGenerator;
    private readonly IFloodFillService<Node> _floodFillService;
    private readonly IMapGeneratorService _mapGenerator;
    private readonly ILocationGroupGeneratorService _groupGenerator;
    private readonly ILogger<SubMapAssignmentService> _logger;
    private readonly Random _rnd = new();

    public SubMapAssignmentService(
        ILocationWeightGeneratorService weightGenerator,
        ILocationGeneratorService locationGenerator,
        IFloodFillService<Node> floodFillService,
        IMapGeneratorService mapGenerator,
        ILocationGroupGeneratorService groupGenerator,
        ILogger<SubMapAssignmentService> logger
    )
    {
        _weightGenerator = weightGenerator;
        _locationGenerator = locationGenerator;
        _floodFillService = floodFillService;
        _mapGenerator = mapGenerator;
        _groupGenerator = groupGenerator;
        _logger = logger;
    }

    public async Task AssignSubMapsAsync(Map map, Dictionary<string, MapParameters> parameters)
    {
        foreach (
            var mainParam in parameters.Values.Where(p =>
                p.LocationType == LocationType.MainLocation
            )
        )
        {
            if (mainParam.SubMapAssignmentLocations == null)
                continue;

            foreach (var subTypeName in mainParam.SubMapAssignmentLocations)
            {
                var subType = (LocationType)Enum.Parse(typeof(LocationType), subTypeName);
                var candidates = parameters
                    .Where(p => p.Value.LocationType == subType)
                    .Select(p => p.Value)
                    .ToList();
                if (!candidates.Any())
                    continue;

                (_weightGenerator as LocationBiomeWeightGenService)?.SetLocationParameters(
                    mainParam,
                    subType.ToString()
                );

                var groups = _groupGenerator.GenerateGroups(
                    map.Grid,
                    map.Width,
                    map.Height,
                    subType
                );
                foreach (var group in groups)
                {
                    var subParam = candidates[_rnd.Next(candidates.Count)];
                    var subMap = await _mapGenerator.GetMap(subParam);
                    foreach (var node in group.Nodes)
                    {
                        node.MapObject = new LocationMapObject
                        {
                            IsInteractive = true,
                            SubMap = subMap,
                        };
                    }
                    map.LocationGroups.Add(group);
                }
            }
        }
    }
}
