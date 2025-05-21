using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class LocationGroupGeneratorService : ILocationGroupGeneratorService
{
    private readonly ILocationWeightGeneratorService _weightGen;
    private readonly ILocationGeneratorService _locationGen;
    private readonly IFloodFillService<Node> _floodFill;
    private readonly Random _rnd = new();

    public LocationGroupGeneratorService(
        ILocationWeightGeneratorService weightGen,
        ILocationGeneratorService locationGen,
        IFloodFillService<Node> floodFill
    )
    {
        _weightGen = weightGen;
        _locationGen = locationGen;
        _floodFill = floodFill;
    }

    public List<LocationGroup> GenerateGroups(
        Node[,] grid,
        int width,
        int height,
        LocationType type
    )
    {
        var count = _rnd.Next(5, 9);
        var weights = _weightGen.GenerateWeights(grid, height, width);
        var updated = _locationGen.Generate(grid, weights, height, width, _rnd, count, type);

        var groups = _floodFill.FindGroups(
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
