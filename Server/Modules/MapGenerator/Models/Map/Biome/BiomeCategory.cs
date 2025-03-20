using System.ComponentModel.DataAnnotations;

namespace Fracture.Server.Modules.MapGenerator.Models.Map.Biome;

public class BiomeCategory
{
    public BiomeType BiomeType { get; set; }
    public TerrainType TerrainType { get; set; }

    public float MaxHeight { get; set; }
    public float MinHeight { get; set; }

    [Required]
    public List<Biome> Biomes { get; set; }
}
