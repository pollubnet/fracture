using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Services;
using System.Numerics;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapGeneratorService : IMapGeneratorService
{
    private MapData _mapData = default!;

    private readonly float _persistence = 0.5f;
    private readonly float _lacunarity = 2f;
    private readonly int _octaves = 3;
    private readonly float _scale = 10f;

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
        int width = 128;
        int height = 128;
        bool useFalloff = false;
        _seed = noiseParameters.UseRandomSeed ? _rnd.Next(int.MaxValue) : noiseParameters.Seed;

        var grid = new Node[width, height];

        var falloffMap = FalloffGenerator.Generate(width);
        // var heightMap = PerlinNoiseGenerator.Generate(
        //     width,
        //     height,
        //     _seed,
        //     _scale,
        //     _octaves,
        //     _persistence,
        //     _lacunarity,
        //     Vector2.Zero
        // );


        var heightMap = CustomPerlin.GenerateNoiseMap(
            width,
            _seed,
            _octaves,
            _persistence,
            _lacunarity,
            _scale
        );

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                heightMap[i, j] = Math.Clamp(0.3f + heightMap[i, j] * 0.8f, 0, 1);
            }
        }

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
                if (useFalloff)
                {
                    heightMap[x, y] = Math.Clamp(heightMap[x, y] - falloffMap[x, y], 0, 1);
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
