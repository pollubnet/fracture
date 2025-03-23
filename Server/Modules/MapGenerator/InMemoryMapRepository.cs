using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator;

public class InMemoryMapRepository : IMapRepository
{
    private readonly Dictionary<string, Map> _maps = new();

    public void SaveMap(Map map)
    {
        map.Name = $"map_{DateTime.UtcNow:yyyyMMdd_HHmmss}_{map.LocationType.ToString()}";

        _maps[map.Name] = map;
    }

    public Map? GetMap(string mapName)
    {
        if (_maps.TryGetValue(mapName, out var map))
            return map;

        return null;
    }

    public Map GetRandomMapByLocation(LocationType locationType)
    {
        var maps = GetAllMapsByLocation(locationType).ToList();
        if (!maps.Any())
            throw new InvalidOperationException($"No maps found for location type {locationType}!");

        var random = new Random();
        return maps[random.Next(maps.Count)];
    }

    public bool RemoveMap(string mapName)
    {
        return _maps.Remove(mapName);
    }

    public List<string> GetAllMapNames()
    {
        return _maps.Keys.ToList();
    }

    public void ClearMaps()
    {
        _maps.Clear();
    }

    public List<Map> GetAllMapsByLocation(LocationType locationType)
    {
        return _maps.Values.Where(x => x.LocationType == locationType).ToList();
    }
}
