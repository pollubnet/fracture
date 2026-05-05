namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class ItemDrop
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N");
}
