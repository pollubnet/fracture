namespace Fracture.Server.Modules.Shared
{
    public class RandomProvider
    {
        public Random Random { get; private set; }

        public static int Seed { get; private set; }

        public RandomProvider(int seed)
        {
            Seed = seed;
            Random = new Random(seed);
        }
    }
}
