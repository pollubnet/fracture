using Fracture.Server.Modules.MapGenerator.Models;

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
        Location location
    )
    {
        var randomWeights = new int[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                randomWeights[x, y] = random.Next(0, weights[x, y]);
            }
        }

        var weights1D = new List<(int, int, int)> { };
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                weights1D.Add((x, y, randomWeights[x, y]));
            }
        }
        var towns = weights1D
            .OrderBy(w => w.Item3)
            .TakeLast(locationCount)
            .Select(n => (n.Item1, n.Item2))
            .ToHashSet();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (towns.Contains((nodes[x, y].XId, nodes[x, y].YId)))
                {
                    nodes[x, y].Location = location;
                }
            }
        }
        return nodes;
    }
}
