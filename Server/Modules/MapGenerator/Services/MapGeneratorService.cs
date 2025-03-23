using System.Runtime.CompilerServices;
using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;
using Fracture.Server.Modules.NoiseGenerator.Services;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class MapGeneratorService : IMapGeneratorService
{
    private readonly Random _rnd = new();
    public Map Map { get; private set; } = default!;

    private readonly ILocationGeneratorService _locationGenerator;
    private readonly ILocationWeightGeneratorService _locationWeightGenerator;
    private ILogger<MapGeneratorService> _logger;
    private IMapRepository _mapRepository;
    private readonly IFloodFillService<Node> _floodFillService;

    private int _seed;

    public MapGeneratorService(
        ILocationGeneratorService locationGenerator,
        ILocationWeightGeneratorService locationWeightGenerator,
        ILogger<MapGeneratorService> logger,
        IFloodFillService<Node> floodFillService
    )
    {
        _locationWeightGenerator = locationWeightGenerator;
        _locationGenerator = locationGenerator;
        _logger = logger;
        _floodFillService = floodFillService;
    }

    public async Task<Map> GetMap(MapParameters? mapParameters)
    {
        Map = GenerateMap(mapParameters);
        return await Task.FromResult(Map);
    }

    private Node[,] GenerateGrid(MapParameters? mapParameters)
    {
        var noiseParameters = mapParameters.NoiseParameters;
        noiseParameters.Seed = noiseParameters.UseRandomSeed
            ? _rnd.Next(-100000, 100000)
            : noiseParameters.Seed;
        int width = 64;
        int height = 64;
        bool useFalloff = true;

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
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
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
            {
                _logger.LogError(
                    string.Format(
                        "No biome category found for height {0} at ({1}, {2}).",
                        heightMap[x, y],
                        x,
                        y
                    )
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

        var gridWithTowns = GenerateTowns(grid, mapParameters.Width, mapParameters.Height);
        var groups = FindTownGroups(gridWithTowns);
        groups
            .Values.ToList()
            .ForEach(list =>
                list.ForEach(coord => Console.WriteLine($"({coord.Item1}, {coord.Item2})"))
            );

        foreach (var (groupName, coords) in groups)
        {
            foreach (var (x, y) in coords)
            {
                gridWithTowns[x, y].GroupName = groupName;
            }
        }
        Map map = new Map(gridWithTowns)
        {
            Grid = gridWithTowns,
            LocationType = mapParameters.LocationType,
            Width = mapParameters.Width,
            Height = mapParameters.Height,
        };

        foreach (var (key, value) in groups)
        {
            Console.WriteLine(key + " " + value.Count);
        }

        return map;
    }

    private Dictionary<string, List<(int x, int y)>> FindTownGroups(Node[,] gridWithTowns)
    {
        return _floodFillService.FindGroups(
            gridWithTowns,
            n => n.LocationType == LocationType.Town,
            (a, b) => a.LocationType == b.LocationType,
            (n, id) => $"Town{id}"
        );
    }

    private Node[,] GenerateTowns(Node[,] grid, int height, int width)
    {
        var townCount = _rnd.Next(5, 30);
        var weights = _locationWeightGenerator.GenerateWeights(grid, height, width);
        return _locationGenerator.Generate(
            grid,
            weights,
            height,
            width,
            _rnd,
            townCount,
            LocationType.Town
        );
    }
}
