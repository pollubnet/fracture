using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public class WeightedLocationGeneratorService : ILocationGeneratorService
{
    public Node[,] Generate(
        Node[,] nodes,
        int[,] weights,
        int height,
        int width,
        Random random,
        int locationCount,
        LocationType locationType
    )
    {
        var randomWeights = new int[height, width];
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            randomWeights[x, y] = random.Next(0, weights[x, y]);

        // Flatten to 1D list and sort by descending weight
        var weights1D = new List<(int X, int Y, int Weight)>();
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            weights1D.Add((x, y, randomWeights[x, y]));

        weights1D = weights1D.OrderByDescending(w => w.Weight).ToList();

        var locationsCreated = 0;
        foreach (var town in weights1D)
        {
            if (locationsCreated == locationCount || town.Weight == 0)
                break;

            var node = nodes[town.X, town.Y];
            if (node.LocationType == LocationType.None)
            {
                node.LocationType = locationType;
                locationsCreated++;
            }
        }

        return nodes;
    }
}
