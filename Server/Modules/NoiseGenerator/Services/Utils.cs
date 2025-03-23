namespace Fracture.Server.Modules.NoiseGenerator.Services;

public class Utils
{
    public static float CustomFunction1(float x, float y, float frequency, float offset)
    {
        return (float)Math.Sin(frequency * (x + y) + offset);
    }

    public static float CustomFunction2(float x, float y, float frequency, float offset)
    {
        return (float)Math.Sin(frequency * Math.Sqrt(x * x + y * y) + offset);
    }

    public static float CustomFunction3(float x, float y, float frequency, float offset)
    {
        return (float)Math.Sin(frequency * x * Math.Pow(Math.Sin(y), 2) + offset);
    }

    public static float CustomFunction4(float x, float y, float frequency, float offset)
    {
        return (float)Math.Sin(frequency * x + y + offset);
    }

    public static float CustomFunction5(float x, float y, float frequency, float offset)
    {
        return ((float)Math.Sin(frequency * x + offset) + (float)Math.Sin(frequency * y + offset))
            / 2;
    }

    public static Func<float, float, float, float, float>[] GetCustomFunctions()
    {
        return new[]
        {
            CustomFunction1,
            CustomFunction2,
            CustomFunction3,
            CustomFunction4,
            CustomFunction5,
        };
    }

    public static float[,] Rescale2dMap(float[,] map, int size)
    {
        var originalSize = map.GetLength(0);
        var rescaledMap = new float[size, size];
        var scale = (float)originalSize / size;

        if (originalSize == size)
            return map;

        if (scale < 1)
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                float sum = 0;
                var count = 0;

                for (var x = (int)(i * scale); x < (int)((i + 1) * scale) && x < originalSize; x++)
                // Calculate the average of the corresponding cells in the larger grid
                for (var y = (int)(j * scale); y < (int)((j + 1) * scale) && y < originalSize; y++)
                {
                    sum += map[x, y];
                    count++;
                }

                rescaledMap[i, j] = sum / count;
            }
        else
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                var x = i * scale;
                var y = j * scale;

                var x1 = (int)x;
                var y1 = (int)y;
                var x2 = Math.Min(x1 + 1, originalSize - 1);
                var y2 = Math.Min(y1 + 1, originalSize - 1);

                var q11 = map[x1, y1];
                var q12 = map[x1, y2];
                var q21 = map[x2, y1];
                var q22 = map[x2, y2];

                var r1 = (x2 - x) * q11 + (x - x1) * q21;
                var r2 = (x2 - x) * q12 + (x - x1) * q22;
                var p = (y2 - y) * r1 + (y - y1) * r2;

                rescaledMap[i, j] = p;
            }

        return rescaledMap;
    }
}
