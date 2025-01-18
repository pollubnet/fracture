using System.Drawing;
using System.Text.Json.Serialization;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class Biome
{
    public string Name { get; set; } = null!;
    public Color Color { get; set; }
    public float MinTemperature { get; set; }
    public float MaxTemperature { get; set; }
}
