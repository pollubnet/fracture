using System;

namespace ItemGeneratorModels
{
    public class Item
    {
        public string Name { get; set; } = null!;
        public string History { get; set; } = null!;

        public ItemRarity Rarity { get; set; } = ItemRarity.Common;

        public Dictionary<ItemStat, int> Statistics { get; init; } = new();

        public void SetStat(ItemStat stat, int value)
        {
            Statistics[stat] = value;
        }
    }
}
