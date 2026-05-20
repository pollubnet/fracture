using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class LocationGroupGeneratorService(
    ILocationWeightGeneratorService weightGen,
    ILocationGeneratorService locationGen,
    IFloodFillService<Node> floodFill,
    RandomProvider rndProvider
) : ILocationGroupGeneratorService
{
    private readonly Random _rnd = rndProvider.Random;

    public List<LocationGroup> GenerateGroups(
        Node[,] grid,
        int width,
        int height,
        LocationType type
    )
    {
        var count = _rnd.Next(5, 9);
        var weights = weightGen.GenerateWeights(grid, height, width);
        var updated = locationGen.Generate(grid, weights, height, width, _rnd, count, type);

        var groups = floodFill.FindGroups(
            updated,
            n => n.LocationType == type,
            (a, b) => a.LocationType == b.LocationType,
            (_, id) => $"{type}{id}"
        );

        foreach (var (name, coords) in groups)
        foreach (var (x, y) in coords)
            updated[x, y].GroupName = name;

        return groups
            .Select(g => new LocationGroup
            {
                GroupName = g.Key,
                LocationType = type,
                Nodes = g.Value.Select(c => updated[c.Item1, c.Item2]).ToList(),
            })
            .ToList();
    }
}
