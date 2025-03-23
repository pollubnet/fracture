using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.NoiseGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public class MapParameters
{
    public LocationType LocationType { get; set; }
    public string[]? SubMapAssignmentLocation { get; set; }

    public NoiseParameters NoiseParameters { get; set; } = new();
    public int Width { get; set; } = 64;
    public int Height { get; set; } = 64;
    public List<BiomeCategory> BiomeCategories { get; set; } = new();
}
