using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public interface ITownWeightGeneratorService
{
    public void GenerateWeights(ref Node[,] nodes, int height, int width);
}
