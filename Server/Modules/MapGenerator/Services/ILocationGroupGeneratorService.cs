using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface ILocationGroupGeneratorService
{
    List<LocationGroup> GenerateGroups(Node[,] grid, int width, int height, LocationType type);
}
