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
}
