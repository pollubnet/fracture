using Fracture.Server.Modules.Npc.Models;

namespace Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

public class NpcMapObject : IMapObject
{
    public required string Id { get; set; }
    public required NpcEntity Entity { get; set; }
    public bool IsInteractive { get; set; }
    public bool IsVisited { get; set; }
}
