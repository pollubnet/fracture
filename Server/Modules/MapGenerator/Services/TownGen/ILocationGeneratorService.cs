using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services.TownGen;

public interface ILocationGeneratorService
{
    public Node[,] Generate(
        Node[,] nodes,
        int[,] weights,
        int height,
        int width,
        Random random,
        int locationCount,
        Location location
    );
}
