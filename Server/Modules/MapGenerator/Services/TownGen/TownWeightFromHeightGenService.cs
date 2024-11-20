using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

// Some idea - should probably be replaced when biomes get introduced
public class TownWeightFromHeightGenService : ITownWeightGeneratorService
{
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

    //Calculates weight of node from neighbours
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
            from n in neighborsMask
            where
                x + n.Item1 < height && y + n.Item2 < width && x + n.Item1 >= 0 && y + n.Item2 >= 0
            select nodes[x + n.Item1, y + n.Item2]
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

    private static int GetWeight(Node node)
    {
        if (node.NoiseValue < 0.02f)
        {
            return 0; //Deap water - none (or tweak it for special worlds/fantasy cities)
        }
        else if (node.NoiseValue < 0.20f)
        {
            return 20; //Water - coastal trade, likely
        }
        else if (node.NoiseValue < 0.40f)
        {
            return 4; //Coastal - likely, but decreased since it more looks like deserts currently
        }
        else if (node.NoiseValue < 0.55f)
        {
            return 13; //Plains or Woods - farmlands or timber, likely
        }
        else if (node.NoiseValue < 0.70f)
        {
            return 11; //Hills - good defensive, bit less likely
        }
        else if (node.NoiseValue < 0.85f)
        {
            return 9; //Mountains - even better defensive, mines, hard terrain - less likely
        }
        else
        {
            return 7; //High mountains - very improbable
        }
    }

    // Self multipliers to make a mountain city surrounded mountains or hills less likely
    private static float GetMultiplier(Node node)
    {
        if (node.NoiseValue < 0.02f)
        {
            return 0.0f; //Deap water
        }
        else if (node.NoiseValue < 0.20f)
        {
            return 0.6f; //Water
        }
        else if (node.NoiseValue < 0.40f)
        {
            return 1f; //Coastal
        }
        else if (node.NoiseValue < 0.55f)
        {
            return 1f; //Plains or Woods
        }
        else if (node.NoiseValue < 0.70f)
        {
            return 1f; //Hills
        }
        else if (node.NoiseValue < 0.85f)
        {
            return 0.7f; //Mountains
        }
        else
        {
            return 0.5f; //High mountains
        }
    }
}
