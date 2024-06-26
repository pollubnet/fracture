﻿namespace Fracture.Server.Modules.NoiseGenerator.Services;

public static class FalloffGenerator
{
    public static float[,] Generate(int size)
    {
        var map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                var value = Math.Max(Math.Abs(x), Math.Abs(y));
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    private static float Evaluate(float value)
    {
        const float a = 3;
        const float b = 4.2f;

        return (float)(Math.Pow(value, a) / (Math.Pow(value, a) + Math.Pow(b - b * value, a)));
    }
}
