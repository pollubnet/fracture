public static class MathUtils
{
    public static float InverseLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }
}
