using System.Drawing;
using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public sealed class BiomeFactory
{
    private BiomeFactory() { }

    private static List<Biome> biomes = new List<Biome>
    {
        CreateBiome(BiomeType.DeepOcean, "#21618C", 0.0f, 0.02f),
        CreateBiome(BiomeType.ShallowWater, "#2E86C1", 0.02f, 0.2f),
        CreateBiome(BiomeType.Beach, "#F9E79F", 0.2f, 0.4f),
        CreateBiome(BiomeType.Grassland, "#28B463", 0.4f, 0.55f),
        CreateBiome(BiomeType.Forest, "#1D8348", 0.55f, 0.7f),
        CreateBiome(BiomeType.Mountains, "#616A6B", 0.7f, 0.85f),
        CreateBiome(BiomeType.HighMountains, "#515A5A", 0.85f, 1.0f),
    };
    private static BiomeFactory _biomeFactory;

    public static BiomeFactory GetBiomeFactory()
    {
        if (_biomeFactory == null)
        {
            _biomeFactory = new BiomeFactory();
        }
        return _biomeFactory;
    }

    public static List<Biome> GetBiomes()
    {
        return biomes;
    }

    private static Biome CreateBiome(
        BiomeType type,
        string colorHex,
        float minHeight,
        float maxHeight
    )
    {
        return new Biome
        {
            BiomeType = type,
            Color = ColorTranslator.FromHtml(colorHex),
            MinHeight = minHeight,
            MaxHeight = maxHeight,
        };
    }
}
