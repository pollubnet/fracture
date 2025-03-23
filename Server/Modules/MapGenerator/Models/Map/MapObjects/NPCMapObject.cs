namespace Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

public class NpcMapObject : IMapObject
{
    public string Type { get; set; }
    public bool IsInteractive { get; set; }
}
