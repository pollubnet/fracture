using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;
using Fracture.Server.Modules.MapGenerator.Models.Map.Town;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;

namespace Fracture.Server.Modules.MapGenerator.Services;

/// <summary>
/// Service responsible for generating the world map.
/// </summary>
public class WorldGenerationService : IWorldGenerationService
{
    private readonly IFloodFillService<Node> _floodFillService;
    private readonly ILocationGeneratorService _locationGenerator;
    private readonly ILocationWeightGeneratorService _locationWeightGenerator;
    private readonly ILogger<WorldGenerationService> _logger;
    private readonly MapDataImportService _mapDataImportService;
    private readonly IMapGeneratorService _mapGeneratorService;
    private readonly Random _rnd = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldGenerationService"/> class.
    /// </summary>
    /// <param name="mapDataImportService">Service for importing map data.</param>
    /// <param name="logger">Logger instance.</param>
    /// <param name="mapGeneratorService">Service for generating maps.</param>
    /// <param name="locationGenerator">Service for generating locations.</param>
    /// <param name="locationWeightGenerator">Service for generating location weights.</param>
    /// <param name="floodFillService">Service for flood fill operations.</param>
    public WorldGenerationService(
        MapDataImportService mapDataImportService,
        ILogger<WorldGenerationService> logger,
        IMapGeneratorService mapGeneratorService,
        ILocationGeneratorService locationGenerator,
        ILocationWeightGeneratorService locationWeightGenerator,
        IFloodFillService<Node> floodFillService
    )
    {
        _mapDataImportService = mapDataImportService;
        _logger = logger;
        _mapGeneratorService = mapGeneratorService;
        _locationGenerator = locationGenerator;
        _locationWeightGenerator = locationWeightGenerator;
        _floodFillService = floodFillService;
    }

    /// <summary>
    /// Generates the world map asynchronously.
    /// </summary>
    /// <returns>The generated world map.</returns>
    public async Task<Map> GenerateWorldMapAsync()
    {
        var parametersDictionary = await _mapDataImportService.ImportMapParametersAsync();

        var mainParameter = SelectRandomParameter(parametersDictionary, LocationType.MainLocation);
        var map = await _mapGeneratorService.GetMap(mainParameter);
        var grid = map.Grid;

        /*Important things:
         appsettings.json must have the following structure:
         1.   "MapSettings": {
            "MapFolderPath": "Config/MapParameters/", // Path to the map parameters
            "MapFolderStructure": { // Structure of the map parameters
              "MainLocation": "MainLocation", // Value needs to be the same as the Folder name its requred for scanning the folder
              "Towns": "Town",
              "Cave": "Cave",
              ...
            }
        2. file.json
        {
      "LocationType": "MainLocation",
      "SubMapAssignmentLocations": [
        "Town" -> needs to be the same as locationType enum it allows us to describe what type of location we want to assign to the MainLocation or other location
        imo it will be usefull later to generate towns or other dungeons
      ],
      
      
  }*/



        await AddSubMapsToLocationGroups(map, parametersDictionary);

        map.Grid = grid;

        return map;
    }

    /// <summary>
    /// Adds submaps to location groups in the map.
    /// </summary>
    /// <param name="map">The main map.</param>
    /// <param name="parametersDictionary">Dictionary of map parameters.</param>
    private async Task AddSubMapsToLocationGroups(
        Map map,
        Dictionary<string, MapParameters> parametersDictionary
    )
    {
        var mainParameters = parametersDictionary
            .Where(p => p.Value.LocationType == LocationType.MainLocation)
            .Select(p => p.Value)
            .ToList();

        foreach (var mainParameter in mainParameters)
        {
            if (mainParameter.SubMapAssignmentLocations == null)
                continue;

            foreach (var subMapLocation in mainParameter.SubMapAssignmentLocations)
            {
                var locationType = (LocationType)Enum.Parse(typeof(LocationType), subMapLocation);

                // Tylko submapy o tym typie
                var subMapCandidates = parametersDictionary
                    .Where(p => p.Value.LocationType == locationType)
                    .Select(p => p.Value)
                    .ToList();

                if (!subMapCandidates.Any())
                    continue;
                if (mainParameter.SubMapAssignmentLocations != null)
                    foreach (var submap in mainParameter.SubMapAssignmentLocations)
                    {
                        (
                            _locationWeightGenerator as LocationBiomeWeightGenService
                        )?.SetLocationParameters(mainParameter, locationType.ToString());
                    }
                else
                {
                    Console.WriteLine("Submap assignment locations are null");
                }
                var groups = GenerateLocationGroups(map.Grid, map.Width, map.Height, locationType);

                // _logger.Logormation($"[{mainParameter ?? "Unnamed"}] Submap of type {locationType} generated for {groups.Count} group(s)");

                foreach (var group in groups)
                {
                    var subMapParameter = subMapCandidates[_rnd.Next(subMapCandidates.Count)];

                    var subMap = await _mapGeneratorService.GetMap(subMapParameter);

                    if (subMap == null)
                    {
                        _logger.LogError($"Failed to generate submap for group: {group.GroupName}");
                        continue;
                    }

                    foreach (var node in group.Nodes)
                    {
                        node.MapObject = new LocationMapObject
                        {
                            IsInteractive = true,
                            SubMap = subMap,
                        };
                    }

                    map.LocationGroups.Add(group);
                }
            }
        }
    }

    /// <summary>
    /// Adds a specific location type to the map.
    /// </summary>
    /// <param name="map">The main map.</param>
    /// <param name="parameters">Dictionary of map parameters.</param>
    /// <param name="locationType">The type of location to add.</param>
    private async Task AddLocationTypeToMap(
        Map map,
        Dictionary<string, MapParameters> parameters,
        LocationType locationType
    )
    {
        var parameterPairs = parameters.Where(p => p.Value.LocationType == locationType).ToList();

        var groups = GenerateLocationGroups(map.Grid, map.Width, map.Height, locationType);
        await AssignSubMapToGroups(groups, parameterPairs, locationType);

        map.LocationGroups.AddRange(groups);
    }

    /// <summary>
    /// Generates location groups based on the specified location type.
    /// </summary>
    /// <param name="grid">The map grid.</param>
    /// <param name="width">The width of the map.</param>
    /// <param name="height">The height of the map.</param>
    /// <param name="locationType">The type of location to generate.</param>
    /// <returns>A list of generated location groups.</returns>
    private List<LocationGroup> GenerateLocationGroups(
        Node[,] grid,
        int width,
        int height,
        LocationType locationType
    )
    {
        var count = _rnd.Next(5, 9);
        var weights = _locationWeightGenerator.GenerateWeights(grid, height, width);

        var updatedGrid = _locationGenerator.Generate(
            grid,
            weights,
            height,
            width,
            _rnd,
            count,
            locationType
        );

        var groups = _floodFillService.FindGroups(
            updatedGrid,
            n => n.LocationType == locationType,
            (a, b) => a.LocationType == b.LocationType,
            (_, id) => $"{locationType}{id}"
        );

        AssignGroupNamesToGrid(updatedGrid, groups);
        int countWithLocation = 0;
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            if (updatedGrid[x, y].LocationType == locationType)
                countWithLocation++;

        return groups
            .Select(g => new LocationGroup
            {
                GroupName = g.Key,
                LocationType = locationType,
                Nodes = g.Value.Select(coord => updatedGrid[coord.Item1, coord.Item2]).ToList(),
            })
            .ToList();
    }

    /// <summary>
    /// Assigns submaps to the specified location groups.
    /// </summary>
    /// <param name="groups">The location groups.</param>
    /// <param name="parameterPairs">List of map parameters.</param>
    /// <param name="locationType">The type of location.</param>
    private async Task AssignSubMapToGroups(
        List<LocationGroup> groups,
        List<KeyValuePair<string, MapParameters>> parameterPairs,
        LocationType locationType
    )
    {
        foreach (var group in groups)
        {
            var index = _rnd.Next(parameterPairs.Count);
            var parameter = parameterPairs[index].Value;
            _logger.LogInformation(
                $"Assigning submap for group {group.GroupName} with type {group.LocationType}"
            );

            var subMap = await _mapGeneratorService.GetMap(parameter);

            foreach (var node in group.Nodes)
                node.MapObject = new LocationMapObject { IsInteractive = true, SubMap = subMap };

            _logger.LogInformation(
                $"Assigned submap ({locationType}) to {group.Nodes.Count} nodes in group {group.GroupName}"
            );
        }
    }

    /// <summary>
    /// Assigns group names to the grid nodes.
    /// </summary>
    /// <param name="grid">The map grid.</param>
    /// <param name="groups">Dictionary of group names and their coordinates.</param>
    private void AssignGroupNamesToGrid(Node[,] grid, Dictionary<string, List<(int, int)>> groups)
    {
        foreach (var (groupName, coords) in groups)
        foreach (var (x, y) in coords)
            grid[x, y].GroupName = groupName;
    }

    /// <summary>
    /// Selects a random map parameter of the specified location type.
    /// </summary>
    /// <param name="parameters">Dictionary of map parameters.</param>
    /// <param name="type">The location type.</param>
    /// <returns>The selected map parameter.</returns>
    private MapParameters SelectRandomParameter(
        Dictionary<string, MapParameters> parameters,
        LocationType type
    )
    {
        var filtered = parameters.Where(p => p.Value.LocationType == type).ToList();

        if (!filtered.Any())
        {
            _logger.LogError($"No parameters found for {type}");
            throw new Exception($"No parameters found for {type}");
        }

        return filtered[_rnd.Next(filtered.Count)].Value;
    }
}
