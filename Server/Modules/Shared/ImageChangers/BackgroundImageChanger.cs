using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.MapGenerator.UI;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImageChanger(
    ILogger<BackgroundImageChanger> logger,
    MapManagerService mapManagerService
)
{
    public required BackgroundImage? BackgroundImage { get; set; }
    public required MapManagerService MapManagerService { get; set; } = mapManagerService;

    public Task ChangeBackgroundImageAsync()
    {
        //if (BackgroundImage is null)
        //{
        //    logger.LogError("BackgroundImage is null");
        //    return Task.CompletedTask;
        //}
        //var currentMap = MapManagerService.CurrentMap;
        //if (currentMap.Grid is null)
        //{
        //    logger.LogError("Grid is null");
        //    return Task.CompletedTask;
        //}

        //if (x < 0 || y < 0 || x >= currentMap.Width || y >= currentMap.Height)
        //{
        //    logger.LogError("Character is out of map");
        //    return Task.CompletedTask;
        //}
        //var cell = currentMap.Grid[x, y];
        //var biome = cell.Biome;

        //if (biome is null)
        //{
        //    logger.LogError("Biome is null");
        //    return Task.CompletedTask;
        //}

        //string? imagePath = null;
        //if (cell.LocationType != LocationType.None)
        //{
        //    var currentLocationName = cell.LocationType.ToString();
        //    var location = biome.Locations.FirstOrDefault(l =>
        //        string.Equals(l.Name, currentLocationName, StringComparison.OrdinalIgnoreCase)
        //    );

        //    if (location is null)
        //    {
        //        logger.LogWarning(
        //            "No matching location config for LocationType {LocationType} at ({X},{Y}) in biome {BiomeName}. Available: {Locations}",
        //            cell.LocationType,
        //            x,
        //            y,
        //            biome.Name,
        //            string.Join(
        //                ", ",
        //                biome
        //                    .Locations.Where(l => !string.IsNullOrWhiteSpace(l.Name))
        //                    .Select(l => l.Name)
        //            )
        //        );
        //    }
        //    else
        //    {
        //        imagePath = location.BackgroundImage;
        //        logger.LogInformation(
        //            "Using location background image '{ImagePath}' for LocationType {LocationType} at ({X},{Y})",
        //            imagePath,
        //            cell.LocationType,
        //            x,
        //            y
        //        );
        //    }
        //}
        //if (string.IsNullOrWhiteSpace(imagePath))
        //{
        //    imagePath = biome.BackgroundImage;
        //    logger.LogInformation(
        //        "Using biome background image '{ImagePath}' for biome {BiomeName} at ({X},{Y})",
        //        imagePath,
        //        biome.Name,
        //        x,
        //        y
        //    );
        //}

        //if (string.IsNullOrWhiteSpace(imagePath))
        //{
        //    logger.LogError("Background image path is null or empty");
        //    return Task.CompletedTask;
        //}

        //BackgroundImage.ImagePath = imagePath;
        //NotifyListChanged(this, EventArgs.Empty);

        return Task.CompletedTask;
    }

    public event EventHandler<EventArgs>? BackgroundImageChanged;

    public void NotifyListChanged(object? sender, EventArgs e)
    {
        BackgroundImageChanged?.Invoke(sender, e);
    }
}
