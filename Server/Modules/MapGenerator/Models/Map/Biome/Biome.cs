﻿using System.Drawing;

namespace Fracture.Server.Modules.MapGenerator.Models.Map.Biome;

public class Biome
{
    public string Name { get; set; } = null!;
    public Color Color { get; set; }
    public float MinTemperature { get; set; }
    public float MaxTemperature { get; set; }
    public bool Walkable { get; set; } = true;

    public List<Location> Locations { get; set; } = new();
}
