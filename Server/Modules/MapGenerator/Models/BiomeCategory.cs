using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Fracture.Server.Modules.MapGenerator.Models;

public class BiomeCategory
{
    public BiomeType BiomeType { get; set; }
    public TerrainType TerrainType { get; set; }

    public float MaxHeight { get; set; }
    public float MinHeight { get; set; }

    [Required]
    public List<Biome> Biomes { get; set; }
}
