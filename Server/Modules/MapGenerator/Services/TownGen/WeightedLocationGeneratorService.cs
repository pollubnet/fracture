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
        //Flatten town coords and sort descending
        var weights1D = new List<(int X, int Y, int Weigth)> { };
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                weights1D.Add((x, y, randomWeights[x, y]));
            }
        }
        weights1D = weights1D.OrderByDescending(w => w.Weigth).ToList();

        //Try creating locations from highest weight to lowest
        int locationsCreated = 0;
        foreach (var town in weights1D)
        {
            //Shorten execution if all eligible locations created
            if (locationsCreated == locationCount || town.Weigth == 0)
                break;
            foreach (var node in nodes)
            {
                if (town.X == node.XId && town.Y == node.YId && node.Location == Location.None)
                {
                    node.Location = location;
                    locationsCreated++;
                }
            }
        }

        return nodes;
    }
}
