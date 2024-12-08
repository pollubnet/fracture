using System.Drawing;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class Biome : BiomeCategory
{
    public string? Name { get; set; }
    public Color Color { get; set; }
    public float MinTemperature { get; set; }
    public float MaxTemperature { get; set; }
}
