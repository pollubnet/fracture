using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator;

public class InMemoryMapRepository : IMapRepository
{
    private readonly Dictionary<string, Map> _maps = new();

    public void SaveMap(string mapName, Map map)
    {
        if (string.IsNullOrWhiteSpace(mapName))
            throw new ArgumentNullException(nameof(mapName));

        _maps[mapName] = map;
    }

    public Map? GetMap(string mapName)
    {
        if (_maps.TryGetValue(mapName, out var map))
            return map;

        return null;
    }

    public bool RemoveMap(string mapName)
    {
        return _maps.Remove(mapName);
    }

    public List<string> GetAllMapNames()
    {
        return _maps.Keys.ToList();
    }

    public List<Map> GetAllMapsByLocation(LocationType locationType)
    {
        return _maps.Values.Where(x => x.LocationType == locationType).ToList();
    }
}
