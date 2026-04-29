namespace Fracture.Server.Modules.Shared
{
    public class RandomProvider
    {
        public Random Random { get; private set; }

        public RandomProvider(int seed)
        {
            Random = new Random(seed);
        }
    }
}
