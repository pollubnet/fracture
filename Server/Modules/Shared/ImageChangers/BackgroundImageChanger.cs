using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.MapGenerator.UI;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImageChanger
{
    private readonly ILogger<BackgroundImageChanger> logger;

    public BackgroundImageChanger(
        ILogger<BackgroundImageChanger> logger,
        MapManagerService mapManagerService
    )
    {
        this.logger = logger;
        MapManagerService = mapManagerService;
    }

    public required BackgroundImage? BackgroundImage { get; set; }
    public required MapManagerService MapManagerService { get; set; }

    public Task ChangeBackgroundImageAsync()
    {
        var biome = MapManagerService
            .CurrentMap
            .Grid[MapView.CharacterXX, MapView.CharacterYY]
            .TerrainType;

        if (BackgroundImage is null)
        {
            logger.LogError("BackgroundImage is missing");
            return Task.CompletedTask;
        }

        switch (biome)
        {
            case "Grassland":
            {
                BackgroundImage.ImagePath = "../assets/background/river.jpg";
                break;
            }
            case "Forest":
            {
                BackgroundImage.ImagePath = "../assets/background/river.jpg";
                break;
            }
            case "Mountains":
            {
                BackgroundImage.ImagePath = "../assets/background/mountains.jpg";
                break;
            }
            case "SandDunes":
            {
                BackgroundImage.ImagePath = "../assets/background/desert.jpg";
                break;
            }
            case "RockyDesert":
            {
                BackgroundImage.ImagePath = "../assets/background/desert.jpg";
                break;
            }
            case "Mesa":
            {
                BackgroundImage.ImagePath = "../assets/background/desert.jpg";
                break;
            }
            case "LavaLake":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "LavaFlow":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "AshPlains":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "ScorchedEarth":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "BasaltFormations":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "VolcanicPeaks":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "ObsidianSummit":
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case "HighMountains":
            {
                BackgroundImage.ImagePath = "../assets/background/mountainsRiver.jpg";
                break;
            }

            default:
                BackgroundImage.ImagePath = "../assets/background/mountainsRiver.jpg";
                break;
        }

        return Task.CompletedTask;
    }

    public event EventHandler<EventArgs>? BackgroundImageChanged;

    public void NotifyListChanged(object? sender, EventArgs e)
    {
        BackgroundImageChanged?.Invoke(sender, e);
    }
}
