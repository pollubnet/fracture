namespace Fracture.Server.Modules.MapGenerator.Models.Map.Biome;

public class Location
{
    public string? Name { get; set; } // name of the location what you want to generate for example town
    public int Weight { get; set; }
    public float Mult { get; set; }
}
