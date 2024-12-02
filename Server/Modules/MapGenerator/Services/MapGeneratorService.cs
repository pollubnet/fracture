using System.Drawing;
using System.Numerics;
using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Services;
using Microsoft.JSInterop;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapGeneratorService : IMapGeneratorService
{
    private MapData _mapData = default!;

    private readonly float _persistence = 0.5f; // How much consecutive octaves contribute to the noise
    private readonly float _lacunarity = 2f; // How fast the frequency increases for each octave
    private readonly int _octaves = 3; // Number of octaves
    private readonly float _scale = 1f; // Scale of the noise, bigger scale = more zoomed in
    private readonly float _falloff = 0.3f; // How much the falloff map affects the heightmap
    private readonly float _sharpness = 1f; // How "sharp" heightmap is. Just a power function
    private readonly float _boost = 0.2f; // Flat boost to heightmap. Adds, then clamps
    private readonly bool _falloffType = true; // true = lerp, false = subtract

    private Random _rnd = new Random();

    private int _seed;

    public MapData MapData
    {
        get => _mapData;
    }

    public Task<MapData> GetMap(NoiseParameters noiseParameters)
    {
        _mapData = GenerateMap(noiseParameters);
        return Task.FromResult(_mapData);
    }

    private MapData GenerateMap(NoiseParameters noiseParameters)
    {
        int width = 80;
        int height = 80;
        bool useFalloff = true;
        _seed = noiseParameters.UseRandomSeed ? _rnd.Next(int.MaxValue) : noiseParameters.Seed;

        var grid = new Node[width, height];

        var falloffMap = FalloffGenerator.GenerateEuclideanSquared(width); // Choose falloff type here

        var heightMap = CustomPerlin.GenerateNoiseMap(
            width,
            _seed,
            _octaves,
            _persistence,
            _lacunarity,
            _scale
        );

        var temperatureMap = PerlinNoiseGenerator.Generate(
            width,
            height,
            _seed + 1,
            _scale,
            _octaves,
            _persistence,
            _lacunarity,
            Vector2.Zero
        );

        var biomes = BiomeFactory.GetBiomes();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heightMap[x, y] = (float)
                    Math.Clamp(Math.Pow(heightMap[x, y], _sharpness) + _boost, 0, 1);

                if (useFalloff)
                {
                    heightMap[x, y] = (float)
                        Math.Clamp(
                            _falloffType
                                ? CustomPerlin.Lerp(heightMap[x, y], falloffMap[x, y], _falloff)
                                : Math.Clamp(
                                    heightMap[x, y] - ((1 - falloffMap[x, y]) * _falloff),
                                    0,
                                    1
                                ),
                            0,
                            1
                        );
                }

                var biome = biomes.FirstOrDefault(b =>
                    heightMap[x, y] >= b.MinHeight && heightMap[x, y] < b.MaxHeight
                ); //type biome in perlin range

                grid[x, y] = new Node(x, y, biome)
                {
                    NoiseValue = heightMap[x, y],
                    Walkable = !(biome.BiomeType is BiomeType.DeepOcean or BiomeType.ShallowWater),
                };

                //grid[x, y].Biome.Color = biome != null ? grid[x, y].Biome.Color = biome.Color : biome.Color = Color.White;
            }
        }

        return new MapData(grid);
    }
}
