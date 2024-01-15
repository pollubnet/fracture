using System;

namespace ItemGeneratorModels
{
    public record StatRange(int MinValue, int MaxValue)
    {
        public int GenerateStat(Random rnd)
        {
            return rnd.Next(MinValue, MaxValue + 1);
        }
    }
}
