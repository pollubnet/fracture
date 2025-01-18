using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public class TownBiomeWeightGenService : ITownWeightGeneratorService
{
    private readonly TownParameters _townParameters;

    public TownBiomeWeightGenService(ILogger<TownParameters> logger)
    {
        _townParameters = new TownParameters(logger);
        _townParameters.Initialize("Normal");
    }

    public void GenerateWeights(ref Node[,] nodes, int height, int width)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var weight = CalculateWeight(nodes, height, width, i, j);
                nodes[i, j].TownWeight = weight;
            }
        }
    }

    private int CalculateWeight(Node[,] nodes, int height, int width, int x, int y)
    {
        Node currentNode = nodes[x, y];
        List<Node> neighbors = [];
        //Defines relative coordinates where to take neighbors from
        //This one is a square without the node we're testing for
        var neighborsMask = new[]
        {
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1),
        };
        neighbors.AddRange(
            neighborsMask
                .Where(n =>
                    x + n.Item1 < height
                    && y + n.Item2 < width
                    && x + n.Item1 >= 0
                    && y + n.Item2 >= 0
                )
                .Select(n => nodes[x + n.Item1, y + n.Item2])
        );
        //Check for land - we don't want water cities
        bool areaHasLand =
            neighbors.Any(n => n.NoiseValue > 0.20f) || currentNode.NoiseValue > 0.20f;
        if (areaHasLand)
        {
            return (int)(neighbors.Sum(GetWeight) * GetMultiplier(currentNode));
        }
        else
        {
            return 0;
        }
    }

    private int GetWeight(Node node)
    {
        var biomeParams = _townParameters.Get(node.Biome.Name);
        return biomeParams.Weight;
    }

    // Self multipliers to make a mountain city surrounded mountains or hills less likely
    private float GetMultiplier(Node node)
    {
        var biomeParams = _townParameters.Get(node.Biome.Name);
        return biomeParams.Mult;
    }
}
