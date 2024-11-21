using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.UI;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChanger;

public class BacgroundImageChanger
{
    public string _bgImg { get; set; } =
        "https://i.pinimg.com/originals/44/6e/3b/446e3b79395a287ca32f7977dd83b290.jpg";

    private MapData? _map;

    public void BgImg()
    {
        var biome = _map?.Grid[MapView.CharacterXX, MapView.CharacterYY].Biome!.BiomeType;
        switch (biome)
        {
            case BiomeType.Forest:
            {
                _bgImg = "../assets/background/river.jpg";
                break;
            }
            case BiomeType.Grassland:
            {
                _bgImg = "../assets/background/river.jpg";
                break;
            }
            case BiomeType.Mountains:
            {
                _bgImg = "../assets/background/mountains.jpg";
                break;
            }
            case BiomeType.Beach:
            {
                _bgImg =
                    "https://i.pinimg.com/originals/44/6e/3b/446e3b79395a287ca32f7977dd83b290.jpg";
                break;
            }
            case BiomeType.HighMountains:
            {
                _bgImg = "../assets/background/mountainsRiver.jpg";
                break;
            }
            case BiomeType.DeepOcean:
                _bgImg =
                    "https://i.pinimg.com/originals/44/6e/3b/446e3b79395a287ca32f7977dd83b290.jpg";
                break;
            case BiomeType.ShallowWater:
                _bgImg =
                    "https://i.pinimg.com/originals/44/6e/3b/446e3b79395a287ca32f7977dd83b290.jpg";
                break;
            default:
                _bgImg =
                    "https://i.pinimg.com/originals/44/6e/3b/446e3b79395a287ca32f7977dd83b290.jpg";
                break;
        }
    }
}
