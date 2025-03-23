using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator;

public interface IMapRepository
{
    /// <summary>
    /// Save map to repository with given name. If map with given name already exists, it will be overwritten.
    /// </summary>
    void SaveMap(string mapName, Map map);

    /// <summary>
    /// Pull map with given name from repository. Returns null if map with given name does not exist.
    /// </summary>
    Map? GetMap(string mapName);

    /// <summary>
    /// Remove map with given name from repository. Returns true if map was removed, false otherwise.
    /// </summary>
    bool RemoveMap(string mapName);

    /// <summary>
    /// Returns list of all map names in repository.
    /// </summary>
    List<Map> GetAllMapsByLocation(LocationType locationType);
    List<string> GetAllMapNames();
    void ClearMaps();
}
