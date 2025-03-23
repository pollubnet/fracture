namespace Fracture.Server.Modules.NoiseGenerator.Services;

public static class CustomPerlin
{
    // Technical functions
    private static float Fade(float t) // Remaps the value of t to a smoother curve, removes artifacts
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    public static float Lerp(float a, float b, float t) // Linear interpolation
    {
        // Lerp sounds like a noob minecraft player
        return a + t * (b - a);
    }

    private static float Grad(int hash, float x, float y) // Gradient function
    {
        // DO NOT TRY TO UNDERSTAND THIS. You will just waste your time.
        // Me trying to read this:
        // https://youtu.be/qx8hrhBZJ98?si=G3bLR81CujWEzlFy&t=35
        var h = hash & 15;
        var u = h < 8 ? x : y;
        var v = h < 4 ? y : x;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        // If you really want to understand this, understand that this just a bunch of bitwise operations
        // that are used to determine the gradient of the noise. Yeah, it's "gradient", but nos as
        // smooth as the gradient with sinusoid functions. That's why I strongly advise to use sin functions
        // for biome(temperature, humidity, etc.) generation.
    }

    private static int[] GeneratePermutation(int seed) // Generates a permutation of 256 values
    {
        // Same seed will always generate the same permutation. This is important.
        var random = new Random(seed);
        var permutation = Enumerable.Range(0, 256).ToArray();
        permutation = permutation.OrderBy(_ => random.Next()).ToArray();
        return permutation.Concat(permutation).ToArray();
    }

    // Main functions
    public static float Perlin(float x, float y, int seed) // Seed must be provided
    {
        var permutation = GeneratePermutation(seed);
        var p = new int[permutation.Length * 2];
        permutation.CopyTo(p, 0);
        permutation.CopyTo(p, permutation.Length);

        var xi = (int)x & 255; // & 255 is the same as % 256, but faster
        var yi = (int)y & 255;

        var xf = x - (int)x; // Get the decimal part of the number
        var yf = y - (int)y;

        var u = Fade(xf); // Get the fade of the decimal part
        var v = Fade(yf);

        var aa = p[p[xi] + yi]; // Get the hash values
        var ab = p[p[xi] + yi + 1]; // The hash values are used to get the gradient
        var ba = p[p[xi + 1] + yi];
        var bb = p[p[xi + 1] + yi + 1];

        var x1 = Lerp(Grad(aa, xf, yf), Grad(ba, xf - 1, yf), u); // Get the gradients
        var x2 = Lerp(Grad(ab, xf, yf - 1), Grad(bb, xf - 1, yf - 1), u);

        return Lerp(x1, x2, v);
    }

    public static float PerlinOctaves(
        float x,
        float y,
        int seed,
        int octaves = 1,
        float persistence = 0.5f,
        float lacunarity = 2.0f
    ) // Seed must be provided
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (var i = 0; i < octaves; i++)
        {
            total += Perlin(x * frequency, y * frequency, seed) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity; // Lacunarity is the factor by which the frequency is multiplied each octave
            // No more memes about what is lacunarity!
        }

        return total / maxValue;
    }

    public static float[,] GenerateNoiseMap(
        int size,
        int? seed = null,
        int octaves = 1,
        float persistence = 0.5f,
        float lacunarity = 2.0f,
        float scale = 1.0f
    )
    {
        // Noise map generation
        // can be called with a seed or without it
        var actualSeed = seed ?? new Random().Next();

        var map = new float[size, size];

        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            var scaledX = x / (float)Math.PI / scale;
            var scaledY = y / (float)Math.PI / scale;
            map[x, y] = PerlinOctaves(
                scaledX,
                scaledY,
                actualSeed,
                octaves,
                persistence,
                lacunarity
            );
        }

        return map;
    }
}
