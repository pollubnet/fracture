using System.Text.Json;

namespace Fracture.Server.Modules.NoiseGenerator.Models;

public class NoiseParameters
{
    public int Seed { get; set; }
    public float Scale { get; set; }
    public int Octaves { get; set; }
    public float Persistence { get; set; }
    public float Lacunarity { get; set; }
    public float Sharpness { get; set; }
    public float Boost { get; set; }
    public float Falloff { get; set; }
    public bool FalloffType { get; set; }
}
