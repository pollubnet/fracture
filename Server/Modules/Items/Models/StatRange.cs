namespace Fracture.Server.Modules.Items.Models;

public record StatRange(int MinValue, int MaxValue)
{
    public int GenerateStat(Random rnd)
    {
        return rnd.Next(MinValue, MaxValue + 1);
    }
}
