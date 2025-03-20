namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class Map
{
    public string? Name { get; set; }
    public Dictionary<LocationType, int> LocationCounter { get; set; } = new();
    public LocationType LocationType { get; set; }

    public List<LocationGroup> LocationGroups { get; set; } = new();

    public int Width { get; set; }
    public int Height { get; set; }
    public Node[,] Grid { get; set; }

    public Map(Node[,] grid)
    {
        Grid = grid;

        for (var y = 0; y < Grid.GetLength(1); y++)
        {
            for (var x = 0; x < Grid.GetLength(0); x++)
            {
                var node = Grid[x, y];

                if (node?.LocationType != null)
                {
                    if (LocationCounter.TryGetValue(node.LocationType, out int count))
                        LocationCounter[node.LocationType] = count + 1;
                    else
                        LocationCounter[node.LocationType] = 1;

                    if (!string.IsNullOrWhiteSpace(node.GroupName))
                    {
                        var existingGroup = LocationGroups.FirstOrDefault(g =>
                            g.GroupName == node.GroupName
                        );

                        if (existingGroup == null)
                        {
                            var newGroup = new LocationGroup
                            {
                                GroupName = node.GroupName,
                                LocationType = node.LocationType,
                            };

                            newGroup.Nodes.Add(node);
                            LocationGroups.Add(newGroup);
                        }
                        else
                        {
                            existingGroup.Nodes.Add(node);
                        }
                    }
                }
            }
        }
    }
}
