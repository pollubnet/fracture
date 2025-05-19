using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.NoiseGenerator.Services;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapGeneratorService : IMapGeneratorService
{
    private readonly ILogger<MapGeneratorService> _logger;
    private readonly Random _rnd = new();
    private int _seed;

    public MapGeneratorService(ILogger<MapGeneratorService> logger)
    {
        _logger = logger;
    }

    public async Task<Map> GetMap(MapParameters? mapParameters)
    {
        return await Task.FromResult(GenerateMap(mapParameters));
    }

    private Node[,] GenerateGrid(MapParameters? mapParameters)
    {
        var noiseParameters = mapParameters?.NoiseParameters;
        noiseParameters.Seed = noiseParameters.UseRandomSeed
            ? _rnd.Next(-100000, 100000)
            : noiseParameters.Seed;
        var width = mapParameters.Width;
        var height = mapParameters.Height;
        var useFalloff = true;

        var grid = new Node[width, height];

        var falloffMap = FalloffGenerator.GenerateCustom(width); // Choose falloff type here

        var heightMap = CustomPerlin.GenerateNoiseMap(
            width,
            noiseParameters.Seed,
            noiseParameters.Octaves,
            noiseParameters.Persistence,
            noiseParameters.Lacunarity,
            noiseParameters.Scale
        );

        var temperatureMap = CustomPerlin.GenerateNoiseMap(
            width,
            noiseParameters.Seed + 1,
            noiseParameters.Octaves,
            noiseParameters.Persistence,
            noiseParameters.Lacunarity,
            noiseParameters.Scale
        );

        var biomeCategories = mapParameters.BiomeCategories;
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            var heightValue = 1 - Math.Pow(1 - heightMap[x, y], noiseParameters.Sharpness);
            heightMap[x, y] = (float)Math.Clamp(heightValue + noiseParameters.Boost, 0, 1);

            var temperatureValue =
                1 - Math.Pow(1 - temperatureMap[x, y], noiseParameters.Sharpness);
            Math.Clamp(temperatureValue + noiseParameters.Boost, 0, 1);

            if (useFalloff)
            {
                heightMap[x, y] = Math.Clamp(
                    noiseParameters.FalloffType
                        ? CustomPerlin.Lerp(
                            heightMap[x, y],
                            falloffMap[x, y],
                            noiseParameters.Falloff
                        )
                        : Math.Clamp(
                            heightMap[x, y] - (1 - falloffMap[x, y]) * noiseParameters.Falloff,
                            0,
                            1
                        ),
                    0,
                    1
                );
                temperatureMap[x, y] = Math.Clamp(
                    noiseParameters.FalloffType
                        ? CustomPerlin.Lerp(
                            temperatureMap[x, y],
                            falloffMap[x, y],
                            noiseParameters.Falloff
                        )
                        : Math.Clamp(
                            temperatureMap[x, y] - (1 - falloffMap[x, y]) * noiseParameters.Falloff,
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
                _logger.LogError(
                    string.Format(
                        "No biome category found for height {0} at ({1}, {2}).",
                        heightMap[x, y],
                        x,
                        y
                    )
                );

            Biome biome = null;
            if (biomeCategory != null)
            {
                biome = biomeCategory.Biomes.FirstOrDefault(sb =>
                    temperatureMap[x, y] >= sb.MinTemperature
                    && temperatureMap[x, y] < sb.MaxTemperature
                )!;
                if (biome == null)
                    _logger.LogError(
                        string.Format(
                            "No biome found for temperature {0} at ({1}, {2}) within category {3}",
                            temperatureMap[x, y],
                            x,
                            y,
                            biomeCategory.TerrainType
                        )
                    );
            }

            grid[x, y] = new Node(x, y, biome)
            {
                NoiseValue = heightMap[x, y],
                Walkable = biome?.Walkable == true,
                TerrainType = biomeCategory!.TerrainType,
            };
        }

        return grid;
    }

    private Map GenerateMap(MapParameters? mapParameters)
    {
        if (mapParameters == null)
        {
            _logger.LogError("MapParameters is null.");
            throw new ArgumentNullException(nameof(mapParameters));
        }
        var grid = GenerateGrid(mapParameters);
        return new Map()
        {
            Grid = grid,
            LocationType = mapParameters.LocationType,
            Width = mapParameters.Width,
            Height = mapParameters.Height,
        };
    }
}
