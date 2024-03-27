using System;

namespace Fracture.Server.Modules.Items.Models
{
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Insane,
        Epic,
        Legendary
    }
}

namespace Fracture.Server.EnumExtensions
{
    using Fracture.Server.Modules.Items.Models;

    public static class ItemRarityExtensions
    {
        public static string ToCssClassName(this ItemRarity rarity)
        {
            return rarity switch
            {
                ItemRarity.Common => "--rarity-common",
                ItemRarity.Uncommon => "--rarity-uncommon",
                ItemRarity.Rare => "--rarity-rare",
                ItemRarity.Insane => "--rarity-insane",
                ItemRarity.Epic => "--rarity-epic",
                ItemRarity.Legendary => "--rarity-legendary",
                _
                    => throw new ArgumentOutOfRangeException(
                        nameof(rarity),
                        rarity,
                        $"Rarity '{rarity}' does not have a corresponding CSS class."
                    )
            };
        }
    }
}
