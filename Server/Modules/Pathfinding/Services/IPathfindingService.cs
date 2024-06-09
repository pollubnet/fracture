using MapGenerator.Pathfindings.Data;

namespace MapGenerator.Pathfindings.Services;

public interface IPathfindingService
{
    List<IPathfindingNode> FindPath(IPathfindingNode start, IPathfindingNode stop);
}
