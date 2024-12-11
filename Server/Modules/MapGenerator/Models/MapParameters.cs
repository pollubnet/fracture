using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapParameters
{
    public NoiseParameters NoiseParameters { get; set; } = new();
    public int Width { get; set; } = 64;
    public int Height { get; set; } = 64;
    public List<BiomeCategory> BiomeCategories { get; set; } = new();
}
