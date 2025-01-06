using Fracture.Server.Components.Pages;
using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.UI;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChanger;

public class BacgroundImageChanger
{
    [Parameter]
    public required BacgroundImage? _bacgroundImage { get; set; }

    [Parameter]
    public required MapData Map { get; set; }

    public Task changeBgImgAsync()
    {
        Console.WriteLine(MapView.CharacterXX);
        var biome = Map.Grid[MapView.CharacterXX, MapView.CharacterYY].TerrainType;
        Console.WriteLine(biome);
        switch (biome)
        {
            case TerrainType.Forest:
            {
                _bacgroundImage.bgImg = "../assets/background/river.jpg";
                Console.WriteLine(_bacgroundImage.bgImg);
                break;
            }
            case TerrainType.Grassland:
            {
                _bacgroundImage.bgImg = "../assets/background/river.jpg";
                break;
            }
            case TerrainType.Mountains:
            {
                _bacgroundImage.bgImg = "../assets/background/mountains.jpg";
                break;
            }
            case TerrainType.Beach:
            {
                _bacgroundImage.bgImg = "../assets/background/lava.jpg";
                break;
            }
            case TerrainType.HighMountains:
            {
                _bacgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case TerrainType.DeepOcean:
            {
                _bacgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case TerrainType.ShallowWater:
            {
                _bacgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            default:
                _bacgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
        }

        return Task.CompletedTask;
    }

    public event EventHandler<EventArgs> BgImgChanged;

    public void NotifyListChanged(object sender, EventArgs e) => BgImgChanged?.Invoke(sender, e);
}
