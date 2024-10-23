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
        return (float)Math.Sin(frequency * x * (Math.Pow(Math.Sin(y), 2)) + offset);
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
        return new Func<float, float, float, float, float>[]
        {
            CustomFunction1,
            CustomFunction2,
            CustomFunction3,
            CustomFunction4,
            CustomFunction5
        };
    }

    public static float[,] Rescale2dMap(float[,] map, int size)
    {
        int originalSize = map.GetLength(0);
        float[,] rescaledMap = new float[size, size];
        float scale = (float)originalSize / size;

        if (originalSize == size)
        {
            return map;
        }

        if (scale < 1)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float sum = 0;
                    int count = 0;

                    for (
                        int x = (int)(i * scale);
                        x < (int)((i + 1) * scale) && x < originalSize;
                        x++
                    )
                    { // Calculate the average of the corresponding cells in the larger grid
                        for (
                            int y = (int)(j * scale);
                            y < (int)((j + 1) * scale) && y < originalSize;
                            y++
                        )
                        {
                            sum += map[x, y];
                            count++;
                        }
                    }

                    rescaledMap[i, j] = sum / count;
                }
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float x = i * scale;
                    float y = j * scale;

                    int x1 = (int)x;
                    int y1 = (int)y;
                    int x2 = Math.Min(x1 + 1, originalSize - 1);
                    int y2 = Math.Min(y1 + 1, originalSize - 1);

                    float q11 = map[x1, y1];
                    float q12 = map[x1, y2];
                    float q21 = map[x2, y1];
                    float q22 = map[x2, y2];

                    float r1 = ((x2 - x) * q11) + ((x - x1) * q21);
                    float r2 = ((x2 - x) * q12) + ((x - x1) * q22);
                    float p = ((y2 - y) * r1) + ((y - y1) * r2);

                    rescaledMap[i, j] = p;
                }
            }
        }
        return rescaledMap;
    }
}
