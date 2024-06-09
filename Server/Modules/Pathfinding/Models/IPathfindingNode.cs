namespace Fracture.Server.Modules.Pathfinding.Models;

public interface IPathfindingNode
{
    public int XId { get; }
    public int YId { get; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get; }
    public bool Walkable { get; set; }
    public IPathfindingNode? PreviousNode { get; set; }
}
