using System;
using System.Text.Json;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.Items.Services
{
    public class ItemGenerator : IItemGenerator
    {
        private readonly Random _rnd;
        private readonly List<RarityModifier> _modifiers;
        private readonly INameGenerator _nameGenerator;

        public ItemGenerator(INameGenerator nameGenerator)
        {
            _nameGenerator = nameGenerator;

            _rnd = new Random();

            var configData = File.ReadAllText("itemgeneratorconfig.json");
            _modifiers = JsonSerializer.Deserialize<List<RarityModifier>>(configData)!;
        }

        public async Task<Item> Generate()
        {
            var value = _rnd.NextSingle();
            var modifier = _modifiers.First(m => m.ValueBelow > value);

            var item = new Item
            {
                Name = await _nameGenerator.GenerateNameAsync(),
                Rarity = modifier.Rarity,
                CreatedAt = DateTime.UtcNow
            };

            var stats = new ItemStatistics { Item = item };

            foreach (var stat in Enum.GetValues<ItemStat>())
            {
                stats.SetStatFromItemStat(stat, modifier.StatRanges[stat].GenerateStat(_rnd));
            }

            item.Statistics = stats;

            var rndType = _rnd.Next(0, Enum.GetValues<ItemType>().Length);
            item.Type = (ItemType)rndType;

            return item;
        }
    }
}
