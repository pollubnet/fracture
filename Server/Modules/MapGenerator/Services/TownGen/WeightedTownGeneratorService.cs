using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public class WeightedTownGeneratorService : ITownGeneratorService
{
    public void Generate(ref Node[,] nodes, int height, int width, int seed, int townCount)
    {
        var random = new Random(seed);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodes[x, y].TownWeight = random.Next(0, nodes[x, y].TownWeight);
            }
        }
        var nodes1D = nodes.Cast<Node>().ToArray();
        var towns = nodes1D
            .OrderBy(n => n.TownWeight)
            .TakeLast(townCount)
            .Select(n => (n.XId, n.YId))
            .ToHashSet();
        var nodesDuplicate = nodes.Clone() as Node[,];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                nodes[x, y].IsTown = towns.Contains((nodes[x, y].XId, nodes[x, y].YId));
            }
        }
    }
}
