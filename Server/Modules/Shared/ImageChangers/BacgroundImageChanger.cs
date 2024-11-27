using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.UI;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.Shared.ImageChanger;

public class BacgroundImageChanger
{
    public string _bgImg { get; set; } = null;

    private MapData? _map;

    public string BgImg()
    {
        var biome = _map?.Grid[MapView.CharacterXX, MapView.CharacterYY].Biome!.BiomeType;
        switch (biome)
        {
            case BiomeType.Forest:
            {
                return _bgImg = "../assets/background/river.jpg";
            }
            case BiomeType.Grassland:
            {
                return _bgImg = "../assets/background/river.jpg";
            }
            case BiomeType.Mountains:
            {
                return _bgImg = "../assets/background/mountains.jpg";
            }
            case BiomeType.Beach:
            {
                return _bgImg = "../assets/background/mountainsRiver.jpg";
            }
            case BiomeType.HighMountains:
            {
                return _bgImg = "../assets/background/mountainsRiver.jpg";
            }
            case BiomeType.DeepOcean:
                return _bgImg = "../assets/background/mountainsRiver.jpg";

            case BiomeType.ShallowWater:
                return _bgImg = "../assets/background/mountainsRiver.jpg";
            default:
                return _bgImg = "../assets/background/mountainsRiver.jpg";
        }
    }
}
