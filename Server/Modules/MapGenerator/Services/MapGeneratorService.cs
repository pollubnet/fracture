using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Services;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapGeneratorService : IMapGeneratorService
{
    private readonly Random _rnd = new();

    private int _seed;

    public MapData MapData { get; private set; } = default!;

    public Task<MapData> GetMap(NoiseParameters noiseParameters)
    {
        MapData = GenerateMap(noiseParameters);
        return Task.FromResult(MapData);
    }

    private MapData GenerateMap(NoiseParameters noiseParameters)
    {
        float _boost = 0.29f; // Flat boost to heightmap. Adds, then clamps
        float _falloff = 0.45f; // How much the falloff map affects the heightmap
        bool _falloffType = true; // true = lerp, false = subtract
        float _lacunarity = 1.6f; // How fast the frequency increases for each octave
        int _octaves = 5; // Number of octaves

        float _persistence = 0.7f; // How much consecutive octaves contribute to the noise
        float _scale = 2.9f; // Scale of the noise, bigger scale = more zoomed in
        float _sharpness = 1f; // How "sharp" heightmap is. Just a power function

        int width = 64;
        int height = 64;
        bool useFalloff = true;
        _seed = noiseParameters.UseRandomSeed ? _rnd.Next(int.MaxValue) : noiseParameters.Seed;

        var grid = new Node[width, height];

        var falloffMap = FalloffGenerator.GenerateCustom(width); // Choose falloff type here

        var heightMap = CustomPerlin.GenerateNoiseMap(
            width,
            _seed,
            _octaves,
            _persistence,
            _lacunarity,
            _scale
        );

        var temperatureMap = CustomPerlin.GenerateNoiseMap(
            width,
            _seed + 1,
            _octaves,
            _persistence,
            _lacunarity,
            _scale
        );

        var biomeCategories = BiomeFactory.GetBiomes();
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            heightMap[x, y] = (float)
                Math.Clamp(Math.Pow(heightMap[x, y], _sharpness) + _boost, 0, 1);
            Math.Clamp(Math.Pow(temperatureMap[x, y], _sharpness) + _boost, 0, 1);

            if (useFalloff)
            {
                heightMap[x, y] = Math.Clamp(
                    _falloffType
                        ? CustomPerlin.Lerp(heightMap[x, y], falloffMap[x, y], _falloff)
                        : Math.Clamp(heightMap[x, y] - (1 - falloffMap[x, y]) * _falloff, 0, 1),
                    0,
                    1
                );
                temperatureMap[x, y] = Math.Clamp(
                    _falloffType
                        ? CustomPerlin.Lerp(temperatureMap[x, y], falloffMap[x, y], _falloff)
                        : Math.Clamp(
                            temperatureMap[x, y] - (1 - falloffMap[x, y]) * _falloff,
                            0,
                            1
                        ),
                    0,
                    1
                );
            }

            var biomeCategory = biomeCategories.FirstOrDefault(b =>
                heightMap[x, y] >= b.MinHeight && heightMap[x, y] < b.MaxHeight
            );

            // If no biome category is found, log it
            if (biomeCategory == null)
            {
                Console.WriteLine(
                    $"No biome category found for height {heightMap[x, y]} at ({x}, {y})."
                );
            }

            Biome biome = null;
            if (biomeCategory != null)
            {
                biome = biomeCategory.Biomes.FirstOrDefault(sb =>
                    temperatureMap[x, y] >= sb.MinTemperature
                    && temperatureMap[x, y] < sb.MaxTemperature
                )!;

                // If no biome is found, log it it's really good if biome data are invalid its easy to find where
                if (biome == null)
                {
                    Console.WriteLine(
                        $"No biome found for temperature {temperatureMap[x, y]} at ({x}, {y}) within category {biomeCategory.TerrainType}"
                    );
                }
            }

            grid[x, y] = new Node(x, y, biome)
            {
                NoiseValue = heightMap[x, y],
                Walkable =
                    biomeCategory != null
                    && !(
                        biomeCategory.TerrainType
                        is TerrainType.DeepOcean
                            or TerrainType.ShallowWater
                    ),
            };
        }

        return new MapData(grid);
    }
}
