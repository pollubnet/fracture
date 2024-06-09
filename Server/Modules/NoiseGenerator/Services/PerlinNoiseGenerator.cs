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
        float persistance,
        float lacunarity,
        Vector2 offset
    )
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        Random prng = new Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.X;
            float offsetY = prng.Next(-100000, 100000) + offset.Y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        Perlin perlin = new Perlin();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                float scalarX = (x - halfWidth) / scale;
                float scalarY = (y - halfHeight) / scale;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = scalarX * frequency + octaveOffsets[i].X;
                    float sampleY = scalarY * frequency + octaveOffsets[i].Y;

                    float perlinValue = (float)perlin.perlin(sampleX, sampleY, 0) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = MathUtils.InverseLerp(
                    minNoiseHeight,
                    maxNoiseHeight,
                    noiseMap[x, y]
                );
            }
        }

        return noiseMap;
    }
}
