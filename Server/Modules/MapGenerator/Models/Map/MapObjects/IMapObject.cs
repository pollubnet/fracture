namespace Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

public interface IMapObject
{
    string Type { get; set; }
    bool IsInteractive { get; set; }
}
