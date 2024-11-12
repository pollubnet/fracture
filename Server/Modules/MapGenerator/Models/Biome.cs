using System.Drawing;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class Biome
{
    public BiomeType BiomeType { get; set; }
    public Color Color { get; set; }

    public float MaxHeight { get; set; }
    public float MinHeight { get; set; }
}
