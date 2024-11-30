using System.Drawing;
using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public sealed class BiomeFactory
{
    private BiomeFactory() { }

    private static List<Biome> biomes = new List<Biome>
    {
        CreateBiome(BiomeType.DeepOcean, "#21618C", 0.0f, 0.02f, null),
        CreateBiome(BiomeType.ShallowWater, "#2E86C1", 0.02f, 0.2f, null),
        CreateBiome(BiomeType.Beach, "#F9E79F", 0.2f, 0.4f, null),
        CreateBiome(BiomeType.Grassland, "#28B463", 0.4f, 0.55f, null),
        CreateBiome(
            BiomeType.Forest,
            "#1D8348",
            0.55f,
            0.7f,
            new List<SubBiome>
            {
                new SubBiome
                {
                    Name = "Tropical Forest",
                    Color = Color.DarkGreen,
                    MinTemperature = 0.7f,
                    MaxTemperature = 1.0f,
                },
                new SubBiome
                {
                    Name = "Temperate Forest",
                    Color = Color.DarkOliveGreen,
                    MinTemperature = 0.4f,
                    MaxTemperature = 0.7f,
                },
                new SubBiome
                {
                    Name = "Default",
                    Color = Color.Green,
                    MinTemperature = 0.4f,
                    MaxTemperature = 0.7f,
                },
            }
        ),
        CreateBiome(
            BiomeType.Mountains,
            "#616A6B",
            0.7f,
            0.85f,
            new List<SubBiome>
            {
                new SubBiome
                {
                    Name = "Rainy Mountains",
                    Color = Color.SlateGray,
                    MinTemperature = 0.5f,
                    MaxTemperature = 0.8f,
                },
                new SubBiome
                {
                    Name = "Snowy Mountains",
                    Color = Color.Snow,
                    MinTemperature = 0.0f,
                    MaxTemperature = 0.1f,
                },
                new SubBiome
                {
                    Name = "Default",
                    Color = ColorTranslator.FromHtml("#616A6B"),
                    MinTemperature = 0.1f,
                    MaxTemperature = 1.0f,
                },
            }
        ),
        CreateBiome(BiomeType.HighMountains, "#515A5A", 0.85f, 1.0f, null),
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
        float maxHeight,
        List<SubBiome> subBiomes
    )
    {
        return new Biome
        {
            BiomeType = type,
            Color = ColorTranslator.FromHtml(colorHex),
            MinHeight = minHeight,
            MaxHeight = maxHeight,
            SubBiomes = subBiomes,
        };
    }
}
