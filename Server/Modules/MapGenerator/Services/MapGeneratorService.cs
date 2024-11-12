using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Services;
using System.Numerics;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapGeneratorService : IMapGeneratorService
{
    private MapData _mapData = default!;

    private readonly float _persistence = 0.5f; // How much consecutive octaves contribute to the noise
    private readonly float _lacunarity = 2f; // How fast the frequency increases for each octave
    private readonly int _octaves = 3; // Number of octaves
    private readonly float _scale = 3f; // Scale of the noise, bigger scale = more zoomed in
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
        int width = 32;
        int height = 32;
        bool useFalloff = true;
        _seed = noiseParameters.UseRandomSeed ? _rnd.Next(int.MaxValue) : noiseParameters.Seed;

        var grid = new Node[width, height];

        var falloffMap = FalloffGenerator.GenerateCustom(width);

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

                grid[x, y] = new Node(x, y, null);
                grid[x, y].NoiseValue = heightMap[x, y];
                grid[x, y].Walkable = grid[x, y].NoiseValue > 0.2f && grid[x, y].NoiseValue < 0.7f;
                HeightToColor(grid, heightMap, y, x);
            }
        }

        return new MapData(grid);
    }

    private static void HeightToColor(Node[,] grid, float[,] heightMap, int y, int x)
    {
        if (heightMap[x, y] < 0.02f)
        {
            grid[x, y].Color = "#21618C";
        }
        else if (heightMap[x, y] < 0.2f)
        {
            grid[x, y].Color = "#2E86C1";
        }
        else if (heightMap[x, y] < 0.40f)
        {
            grid[x, y].Color = "#F9E79F";
        }
        else if (heightMap[x, y] < 0.55f)
        {
            grid[x, y].Color = "#28B463";
        }
        else if (heightMap[x, y] < 0.7f)
        {
            grid[x, y].Color = "#1D8348";
        }
        else if (heightMap[x, y] < 0.85f)
        {
            grid[x, y].Color = "#616A6B";
        }
        else
        {
            grid[x, y].Color = "#515A5A";
        }
    }
}
