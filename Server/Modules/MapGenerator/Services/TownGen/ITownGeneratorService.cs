using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public interface ITownGeneratorService
{
    public void Generate(ref Node[,] nodes, int height, int width, int seed, int townCount);
}
