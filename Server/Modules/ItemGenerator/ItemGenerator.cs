using System;
using System.Text.Json;
using ItemGeneratorModels;

namespace ExampleItemGenerator.Services.Generators
{
    public class ItemGenerator : IItemGenerator
    {
        private readonly Random _rnd;
        private readonly List<RarityModifier> _modifiers;
        private readonly INameGenerator _nameGenerator;

        public ItemGenerator()
        {
            _nameGenerator = new InternetNameGenerator();

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
                Name = await _nameGenerator.GenerateName(),
                Rarity = modifier.Rarity
            };

            foreach (var itemStat in Enum.GetValues<ItemStat>())
                item.SetStat(itemStat, modifier.StatRanges[itemStat].GenerateStat(_rnd));

            return item;
        }
    }
}
