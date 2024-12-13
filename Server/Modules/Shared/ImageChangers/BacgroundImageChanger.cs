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
        var biome = Map.Grid[MapView.CharacterXX, MapView.CharacterYY].Biome.BiomeType;
        Console.WriteLine(biome);
        switch (biome)
        {
            case BiomeType.Forest:
            {
                _bacgroundImage.bgImg = "../assets/background/river.jpg";
                Console.WriteLine(_bacgroundImage.bgImg);
                break;
            }
            case BiomeType.Grassland:
            {
                _bacgroundImage.bgImg = "../assets/background/river.jpg";
                break;
            }
            case BiomeType.Mountains:
            {
                _bacgroundImage.bgImg = "../assets/background/mountains.jpg";
                break;
            }
            case BiomeType.Beach:
            {
                _bacgroundImage.bgImg = "../assets/background/lava.jpg";
                break;
            }
            case BiomeType.HighMountains:
            {
                _bacgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case BiomeType.DeepOcean:
            {
                _bacgroundImage.bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case BiomeType.ShallowWater:
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
