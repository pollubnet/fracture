using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface IItemPlacementService
{
    void PlaceItems(Map map);
}
