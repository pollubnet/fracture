namespace Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

public class NpcMapObject : IMapObject
{
    public required string Id { get; set; }
    public bool IsInteractive { get; set; }
}
