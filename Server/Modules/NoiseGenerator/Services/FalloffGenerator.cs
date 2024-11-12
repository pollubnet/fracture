namespace Fracture.Server.Modules.NoiseGenerator.Services;

public static class FalloffGenerator
{
    public static float[,] Generate(int size)
    {
        static float Evaluate(float value)
        {
            const float a = 2;
            const float b = 4.2f;

            return (float)(
                1 - Math.Pow(value, a) / (Math.Pow(value, a) + Math.Pow(b - b * value, a))
            );
        }

        var map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1; // Normalize to -1 to 1
                float y = j / (float)size * 2 - 1;

                var value = Math.Max(Math.Abs(x), Math.Abs(y));
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    public static float[,] GenerateEuclideanSquared(int size)
    {
        static float Evaluate(float value)
        {
            /*Euclidean²
              d = min(1, (nx² + ny²) / sqrt(2)), when you want the island to be round,
               and plan to embed it in a larger world.
               source: https://www.redblobgames.com/maps/terrain-from-noise/*/
            return (float)Math.Sqrt(1 - value * value);
        }

        var map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1; // Normalize to -1 to 1
                float y = j / (float)size * 2 - 1;

                var value = Math.Min(1.0, (float)Math.Sqrt(x * x + y * y)); // Euclidean distance from center
                map[i, j] = Evaluate((float)value);
            }
        }

        return map;
    }

    public static float[,] GenerateEuclidean(int size)
    {
        float Evaluate(float value)
        {
            return (float)Math.Sqrt(1 - value);
        }

        var map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1; // Normalize to -1 to 1
                float y = j / (float)size * 2 - 1;

                var value = Math.Min(1.0f, (float)Math.Sqrt(x * x + y * y));
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    public static float[,] GenerateSquareBump(int size)
    {
        /*Square Bump d = 1 - (1-nx²) * (1-ny²),
         when you have a square map and want to make the island fill as much of the space
         possible without reaching the borders.*/
        float Evaluate(float value)
        {
            return (float)(1 - Math.Pow(value, 2));
        }

        var map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1; // Normalize to -1 to 1
                float y = j / (float)size * 2 - 1;

                var value = Math.Min(1.0f, Math.Max(Math.Abs(x), Math.Abs(y))); // Square distance from center
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    public static float[,] GenerateCustom(int size)
    {
        float Evaluate(float value)
        {
            return (float)(1 - Math.Pow(value, 4));
        }

        var map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1; // Normalize to -1 to 1
                float y = j / (float)size * 2 - 1;

                var value = (float)Math.Sqrt(Math.Sqrt(Math.Pow(x, 4) + Math.Pow(y, 4)));
                map[i, j] = Evaluate(value);
            }
        }
        return map;
    }
}
