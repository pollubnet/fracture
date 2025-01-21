using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public class LocationBiomeWeightGenService : ILocationWeightGeneratorService
{
    private readonly TownParameters _townParameters;

    public LocationBiomeWeightGenService(ILogger<TownParameters> logger)
    {
        _townParameters = new TownParameters(logger);
        _townParameters.Initialize("Normal");
    }

    public int[,] GenerateWeights(Node[,] nodes, int height, int width)
    {
        var weights = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var weight = CalculateWeight(nodes, height, width, i, j);
                weights[i, j] = weight;
            }
        }
        return weights;
    }

    private int CalculateWeight(Node[,] nodes, int height, int width, int x, int y)
    {
        Node currentNode = nodes[x, y];
        List<Node> neighbors = [];
        //Defines relative coordinates/convolution mask where to take neighbors from
        //This one is a square without the node we're testing for
        var neighborsMask = new (int X, int Y)[]
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
                .Where(n => x + n.X < height && y + n.Y < width && x + n.X >= 0 && y + n.Y >= 0)
                .Select(n => nodes[x + n.X, y + n.Y])
        );
        //Check for land - we don't want water cities
        bool areaHasLand = neighbors.Any(n => n.Walkable) || currentNode.Walkable;
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
