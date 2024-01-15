using System;

namespace ItemGeneratorModels
{
    public class RarityModifier
    {
        public float ValueBelow { get; init; } = 0f;
        public ItemRarity Rarity { get; init; } = ItemRarity.Common;

        public Dictionary<ItemStat, StatRange> StatRanges { get; init; } = new();

        public RarityModifier(float valueBelow, ItemRarity rarity)
        {
            ValueBelow = valueBelow;
            Rarity = rarity;
        }

        public void SetStatRange(ItemStat stat, StatRange range)
        {
            StatRanges[stat] = range;
        }
    }
}
