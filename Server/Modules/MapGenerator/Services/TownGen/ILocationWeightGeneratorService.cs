using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public interface ILocationWeightGeneratorService
{
    public int[,] GenerateWeights(Node[,] nodes, int height, int width);
    public void SetLocationParameters(MapParameters mapParameters);
}
