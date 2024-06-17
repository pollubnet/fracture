namespace Fracture.Server.Modules.NoiseGenerator.Models;

public class NoiseParameters
{
    public int Seed { get; set; }
    public bool UseRandomSeed { get; set; } = true;
    public bool GenerateNew { get; set; }
}
