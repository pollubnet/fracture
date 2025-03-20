using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.Biome;
using Fracture.Server.Modules.MapGenerator.UI;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImageChanger
{
    [Parameter]
    public required BackgroundImage? BackgroundImage { get; set; }

    [Parameter]
    public required Map Map { get; set; }

    private ILogger<BackgroundImageChanger> logger;

    public Task ChangeBackgroundImageAsync()
    {
        var biome = Map.Grid[MapView.CharacterXX, MapView.CharacterYY].TerrainType;

        if (BackgroundImage is null)
        {
            logger.LogError("BackgroundImage is missing");
            return Task.CompletedTask;
        }

        switch (biome)
        {
            case TerrainType.Forest:
            {
                BackgroundImage.ImagePath = "../assets/background/river.jpg";
                break;
            }
            case TerrainType.Grassland:
            {
                BackgroundImage.ImagePath = "../assets/background/river.jpg";
                break;
            }
            case TerrainType.Mountains:
            {
                BackgroundImage.ImagePath = "../assets/background/mountains.jpg";
                break;
            }
            case TerrainType.Beach:
            {
                BackgroundImage.ImagePath = "../assets/background/lava.jpg";
                break;
            }
            case TerrainType.HighMountains:
            {
                BackgroundImage.ImagePath = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case TerrainType.DeepOcean:
            {
                BackgroundImage.ImagePath = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case TerrainType.ShallowWater:
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

    public void NotifyListChanged(object? sender, EventArgs e) =>
        BackgroundImageChanged?.Invoke(sender, e);
}
