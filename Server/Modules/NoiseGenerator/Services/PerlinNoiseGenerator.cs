using System.Numerics;

namespace Fracture.Server.Modules.NoiseGenerator.Services;

public static class PerlinNoiseGenerator
{
    public static float[,] Generate(
        int mapWidth,
        int mapHeight,
        int seed,
        float scale,
        int octaves,
        float persistence,
        float lacunarity,
        Vector2 offset
    )
    {
        var noiseMap = new float[mapWidth, mapHeight];

        var prng = new Random(seed);
        var octaveOffsets = new Vector2[octaves];
        for (var i = 0; i < octaves; i++)
        {
            var offsetX = prng.Next(-100000, 100000) + offset.X;
            var offsetY = prng.Next(-100000, 100000) + offset.Y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
            scale = 0.0001f;

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        var halfWidth = mapWidth / 2f;
        var halfHeight = mapHeight / 2f;

        var perlin = new Perlin();
        for (var y = 0; y < mapHeight; y++)
        for (var x = 0; x < mapWidth; x++)
        {
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            var scalarX = (x - halfWidth) / scale;
            var scalarY = (y - halfHeight) / scale;
            for (var i = 0; i < octaves; i++)
            {
                var sampleX = scalarX * frequency + octaveOffsets[i].X;
                var sampleY = scalarY * frequency + octaveOffsets[i].Y;

                var perlinValue = (float)perlin.perlin(sampleX, sampleY, 0) * 2 - 1;

                noiseHeight += perlinValue * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            if (noiseHeight > maxNoiseHeight)
                maxNoiseHeight = noiseHeight;
            else if (noiseHeight < minNoiseHeight)
                minNoiseHeight = noiseHeight;
            noiseMap[x, y] = noiseHeight;
        }

        for (var y = 0; y < mapHeight; y++)
        for (var x = 0; x < mapWidth; x++)
            noiseMap[x, y] = MathUtils.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);

        return noiseMap;
    }
}
