using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public interface ILocationWeightGeneratorService
{
    public int[,] GenerateWeights(Node[,] nodes, int height, int width);
}
