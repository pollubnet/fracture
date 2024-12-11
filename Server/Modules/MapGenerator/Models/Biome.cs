using System.Drawing;
using System.Text.Json.Serialization;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class Biome : BiomeCategory
{
    public string? Name { get; set; }

    [JsonConverter(typeof(ColorJsonConverter))]
    public Color Color { get; set; }
    public float MinTemperature { get; set; }
    public float MaxTemperature { get; set; }
}
