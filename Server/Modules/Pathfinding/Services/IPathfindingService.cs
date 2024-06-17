using Fracture.Server.Modules.Pathfinding.Models;

namespace Fracture.Server.Modules.Pathfinding.Services;

public interface IPathfindingService
{
    List<IPathfindingNode>? FindPath(IPathfindingNode start, IPathfindingNode stop);
}
