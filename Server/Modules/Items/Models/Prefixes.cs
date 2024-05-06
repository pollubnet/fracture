using Fracture.Server.Modules.Shared;
using System;

namespace Fracture.Server.Modules.Items.Models
{
    public class Prefixes
    {
        Dictionary<string, int> PerfectionNames = new Dictionary<string, int>()
        {
            { "Eldritch", 61 },
            { "Efficient", 41 },
            { "Versatile", 21 },
            { "Balanced", 0 },
            { "Unsteady", -20 },
            { "Poisoned", -40 },
            { "Toxic", -60 },
            { "Cursed", -80 },
            { "Demonic", -100 }
        };

        Dictionary<ItemStat, Dictionary<string, int>> StatNames = new Dictionary<
            ItemStat,
            Dictionary<string, int>
        >()
        {
            {
                ItemStat.Luck,
                new Dictionary<string, int>()
                {
                    { "Destined", 81 },
                    { "Predestined", 61 },
                    { "Prosperous", 41 },
                    { "Fortunate", 21 },
                    { "Lucky", 0 },
                    { "Unlucky", -20 },
                    { "Unfortunate", -40 },
                    { "Clumsy", -60 },
                    { "Fateful", -80 },
                    { "Misfortune-laden", -100 }
                }
            },
            {
                ItemStat.Health,
                new Dictionary<string, int>()
                {
                    { "Unbreakable", 81 },
                    { "Vital", 61 },
                    { "Sturdy", 41 },
                    { "Durable", 21 },
                    { "Stable", 0 },
                    { "Delicate", -20 },
                    { "Feeble", -40 },
                    { "Flimsy", -60 },
                    { "Fragile", -80 },
                    { "Ethereal", -100 }
                }
            },
            {
                ItemStat.Armor,
                new Dictionary<string, int>()
                {
                    { "Specialized", 81 },
                    { "Reliable", 61 },
                    { "Reinforced", 41 },
                    { "Bulky", 21 },
                    { "Simple", 0 },
                    { "Crude", -20 },
                    { "Plain", -40 },
                    { "Rusty", -60 },
                    { "Unreliable", -80 },
                    { "Primitive", -100 }
                }
            },
            {
                ItemStat.Power,
                new Dictionary<string, int>()
                {
                    { "Lethal", 81 },
                    { "Brutal", 61 },
                    { "Sharp", 41 },
                    { "Precise", 21 },
                    { "Reasonable", 0 },
                    { "Blunt", -20 },
                    { "Dull", -40 },
                    { "Brittle", -60 },
                    { "Weak", -80 },
                    { "Wasteful", -100 }
                }
            },
        };

        private void addPerfectionPrefix(Item item, float statsAverage)
        {
            foreach (var perfectionIteration in PerfectionNames)
            {
                if (statsAverage >= perfectionIteration.Value)
                {
                    item.Name = perfectionIteration.Key + " " + item.Name;
                    return;
                }
            }
        }

        private ItemStat findMaxAbsoluteStat(Item item)
        {
            int maxAbsoluteStatValue = -1;
            ItemStat maxAbsoluteStatName = ItemStat.Luck;

            foreach (var stat in Enum.GetValues<ItemStat>())
            {
                int absoluteStatValue = Math.Abs(item.Statistics.GetStatFromItemStat(stat));
                if (absoluteStatValue > maxAbsoluteStatValue)
                {
                    maxAbsoluteStatValue = absoluteStatValue;
                    maxAbsoluteStatName = stat;
                }
            }
            return maxAbsoluteStatName;
        }

        private void addStatPrefix(Item item, ItemStat stat)
        {
            // TODO: findStatPrefxi(item, stat)

            if (stat == ItemStat.Luck)
            {
                findStatPrefix(item, ItemStat.Luck);
            }
            else if (stat == ItemStat.Health)
            {
                findStatPrefix(item, ItemStat.Health);
            }
            else if (stat == ItemStat.Armor)
            {
                findStatPrefix(item, ItemStat.Armor);
            }
            else if (stat == ItemStat.Power)
            {
                findStatPrefix(item, ItemStat.Power);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void findStatPrefix(Item item, ItemStat itemStat)
        {
            int statValue = item.Statistics.GetStatFromItemStat(itemStat);
            foreach (var iteration in StatNames[itemStat])
            {
                if (statValue >= iteration.Value)
                {
                    item.Name = iteration.Key + " " + item.Name;
                    return;
                }
            }
        }

        public void addPrefixes(Item item)
        {
            int totalStats = Enum.GetValues<ItemStat>()
                .Aggregate(0, (acc, y) => acc + item.Statistics.GetStatFromItemStat(y));
            int statsNumber = Enum.GetNames(typeof(ItemStat)).Length;
            float statsAverage = (float)totalStats / statsNumber;
            if (statsAverage >= 81)
            {
                // TODO: special name generator
                return;
            }
            ItemStat stat = findMaxAbsoluteStat(item);
            addStatPrefix(item, stat);
            addPerfectionPrefix(item, statsAverage);
        }
    }
}
