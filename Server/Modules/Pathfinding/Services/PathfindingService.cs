using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.Pathfinding.Models;

namespace Fracture.Server.Modules.Pathfinding.Services;

public class PathfindingService : IPathfindingService
{
    private readonly IMapGeneratorService _mapGeneratorService;

    public PathfindingService(IMapGeneratorService mapGeneratorService)
    {
        _mapGeneratorService = mapGeneratorService;
    }

    public List<IPathfindingNode>? FindPath(IPathfindingNode start, IPathfindingNode stop)
    {
        List<IPathfindingNode> toEvaluateSet = new();
        List<IPathfindingNode> evaluatedSet = new();

        toEvaluateSet.Add(start);

        while (toEvaluateSet.Count > 0)
        {
            var current = GetLowestFScoreNode(toEvaluateSet);

            if (current.XId == stop.XId && current.YId == stop.YId)
            {
                return ReconstructPath(current);
            }

            toEvaluateSet.Remove(current);
            evaluatedSet.Add(current);

            foreach (IPathfindingNode neighbor in GetNeighbors(current))
            {
                if (evaluatedSet.Contains(neighbor) || !neighbor.Walkable)
                    continue;

                int tempGCost = current.GCost + 1;

                if (!toEvaluateSet.Contains(neighbor) || tempGCost < neighbor.GCost)
                {
                    neighbor.PreviousNode = current;
                    neighbor.GCost = tempGCost;
                    neighbor.HCost =
                        Math.Abs(neighbor.XId - stop.XId) + Math.Abs(neighbor.YId - stop.YId);

                    if (!toEvaluateSet.Contains(neighbor))
                        toEvaluateSet.Add(neighbor);
                }
            }
        }

        Console.WriteLine("No valid path found.");
        return null;
    }

    private List<IPathfindingNode> GetNeighbors(IPathfindingNode node)
    {
        var neighbors = new List<IPathfindingNode>();
        int[] dx = [0, 1, 0, -1, 1, 1, -1, -1];
        int[] dy = [1, 0, -1, 0, 1, -1, 1, -1]; // dx and dy arrays are determining 18 different move sequences, aka dx[i] = -1, dy[i] = 1 means we are moving to a top left tile

        for (int i = 0; i < 8; i++)
        {
            int newX = node.XId + dx[i];
            int newY = node.YId + dy[i];

            if (
                newX >= 0
                && newX < _mapGeneratorService.Map.Grid.GetLength(0)
                && newY >= 0
                && newY < _mapGeneratorService.Map.Grid.GetLength(1)
            )
            {
                IPathfindingNode neighbor = _mapGeneratorService.Map.Grid[newX, newY];
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private List<IPathfindingNode> ReconstructPath(IPathfindingNode node)
    {
        List<IPathfindingNode> path = new List<IPathfindingNode>();

        while (node != null)
        {
            path.Insert(0, node);
            node = node.PreviousNode;
        }

        return path;
    }

    private IPathfindingNode GetLowestFScoreNode(List<IPathfindingNode> nodes)
    {
        var lowest = nodes[0];
        foreach (var node in nodes)
        {
            if (node.FCost < lowest.FCost)
                lowest = node;
        }
        return lowest;
    }
}
