using System.Drawing;
using Fracture.Server.Modules.MapGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public sealed class BiomeFactory
{
    private static readonly List<BiomeCategory> biomes =
        new()
        {
            //collors are temporaily, in json it will be fine
            CreateBiome(
                BiomeType.Normal,
                TerrainType.DeepOcean,
                0.0f,
                0.02f,
                [
                    new Biome
                    {
                        Name = "Deep Ocean",
                        Color = Color.DarkBlue,
                        MinTemperature = 0.0f,
                        MaxTemperature = 1.0f,
                    },
                ]
            ), // here will be default biome with temp 0.0f - 1.0f and color
            CreateBiome(
                BiomeType.Normal,
                TerrainType.ShallowWater,
                0.02f,
                0.2f,
                [
                    new Biome
                    {
                        Name = "Shallow Water",
                        Color = Color.Blue, //lets talk about collors
                        MinTemperature = 0.0f,
                        MaxTemperature = 1.0f,
                    },
                ]
            ),
            CreateBiome(
                BiomeType.Normal,
                TerrainType.Beach,
                0.2f,
                0.4f,
                [
                    new Biome
                    {
                        Name = "Beach",
                        Color = Color.BurlyWood,
                        MinTemperature = 0.0f,
                        MaxTemperature = 1.0f,
                    },
                ]
            ),
            CreateBiome(
                BiomeType.Normal,
                TerrainType.Grassland,
                0.4f,
                0.55f,
                [
                    new Biome
                    {
                        Name = "Grassland",
                        Color = Color.ForestGreen,
                        MinTemperature = 0.0f,
                        MaxTemperature = 1.0f,
                    },
                ]
            ),
            CreateBiome(
                BiomeType.Normal,
                TerrainType.Forest,
                0.55f,
                0.7f,
                [
                    new Biome
                    {
                        Name = "Tropical Forest",
                        Color = Color.DarkGreen,
                        MinTemperature = 0.7f,
                        MaxTemperature = 1.0f,
                    },
                    new Biome
                    {
                        Name = "Temperate Forest",
                        Color = Color.DarkOliveGreen,
                        MinTemperature = 0.4f,
                        MaxTemperature = 0.7f,
                    },
                    new Biome
                    {
                        Name = "Forest",
                        Color = Color.Green,
                        MinTemperature = 0f,
                        MaxTemperature = 0.4f,
                    },
                ]
            ),
            CreateBiome(
                BiomeType.Normal,
                TerrainType.Mountains,
                0.7f,
                0.85f,
                [
                    new Biome
                    {
                        Name = "Rainy Mountains",
                        Color = Color.SlateGray,
                        MinTemperature = 0f,
                        MaxTemperature = 0.3f,
                    },
                    new Biome
                    {
                        Name = "Snowy Mountains",
                        Color = Color.Snow,
                        MinTemperature = 0.3f,
                        MaxTemperature = 0.7f,
                    },
                    new Biome
                    {
                        Name = "Default",
                        Color = ColorTranslator.FromHtml("#616A6B"),
                        MinTemperature = 0.7f,
                        MaxTemperature = 1.0f,
                    },
                ]
            ),
            CreateBiome(
                BiomeType.Normal,
                TerrainType.HighMountains,
                0.85f,
                1.0f,
                [
                    new Biome
                    {
                        Name = "High Mountains",
                        Color = Color.DarkGray,
                        MinTemperature = 0.0f,
                        MaxTemperature = 1.0f,
                    },
                ]
            ),
        };

    private static BiomeFactory _biomeFactory;

    private BiomeFactory() { }

    public static BiomeFactory GetBiomeFactory()
    {
        if (_biomeFactory == null)
            _biomeFactory = new BiomeFactory();
        return _biomeFactory;
    }

    public static List<BiomeCategory> GetBiomes()
    {
        return biomes;
    }

    private static BiomeCategory CreateBiome(
        BiomeType biomeType,
        TerrainType type,
        float minHeight,
        float maxHeight,
        List<Biome> biomeList
    )
    {
        return new BiomeCategory
        {
            BiomeType = biomeType,
            TerrainType = type,
            MinHeight = minHeight,
            MaxHeight = maxHeight,
            Biomes = biomeList,
        };
    }
}
