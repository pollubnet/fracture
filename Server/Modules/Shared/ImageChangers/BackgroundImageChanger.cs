using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.UI;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImageChanger
{
    [Parameter]
    public required BackgroundImage? BackgroundImage { get; set; }

    [Parameter]
    public required MapData Map { get; set; }

    private ILogger<BackgroundImageChanger> logger;

    public Task ChangeBgImgAsync()
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
                BackgroundImage.bgImg = "../assets/background/river.jpg";
                break;
            }
            case TerrainType.Grassland:
            {
                BackgroundImage.bgImg = "../assets/background/river.jpg";
                break;
            }
            case TerrainType.Mountains:
            {
                BackgroundImage.bgImg = "../assets/background/mountains.jpg";
                break;
            }
            case TerrainType.Beach:
            {
                BackgroundImage.bgImg = "../assets/background/lava.jpg";
                break;
            }
            case TerrainType.HighMountains:
            {
                BackgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case TerrainType.DeepOcean:
            {
                BackgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case TerrainType.ShallowWater:
            {
                BackgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            default:
                BackgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
        }

        return Task.CompletedTask;
    }

    public event EventHandler<EventArgs>? BgImgChanged;

    public void NotifyListChanged(object? sender, EventArgs e) => BgImgChanged?.Invoke(sender, e);
}
