using Fracture.Server.Modules.Items.Models;

namespace Fracture.Server.Modules.Items.Services;

public class RarityModifier
{
    public RarityModifier(float valueBelow, ItemRarity rarity)
    {
        ValueBelow = valueBelow;
        Rarity = rarity;
    }

    public float ValueBelow { get; init; }
    public ItemRarity Rarity { get; init; } = ItemRarity.Common;

    public Dictionary<ItemStat, StatRange> StatRanges { get; init; } = new();

    public void SetStatRange(ItemStat stat, StatRange range)
    {
        StatRanges[stat] = range;
    }
}
