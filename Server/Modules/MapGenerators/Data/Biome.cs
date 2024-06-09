using System.Drawing;

namespace MapGenerator.MapGenerators.Data;

public class Biome
{
    public BiomeType BiomeType { get; set; }
    public Color Color { get; set; }
    public float MaxTemperature { get; set; }
    public float MaxHeight { get; set; }
}
