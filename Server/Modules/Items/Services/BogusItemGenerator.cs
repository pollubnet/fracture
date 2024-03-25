using Bogus.DataSets;
using System.Text.Json;
using Fracture.Server.Modules.Items.Models;

namespace Fracture.Server.Modules.Items.Services
{
    public class BogusItemGenerator : IItemGenerator
    {
        private readonly Random _rnd;
        private readonly List<RarityModifier> _modifiers;
        private readonly Lorem lorem;
        private readonly Hacker hacker;

        public BogusItemGenerator()
        {
            _rnd = new Random();

            var configData = File.ReadAllText("itemgeneratorconfig.json");
            _modifiers = JsonSerializer.Deserialize<List<RarityModifier>>(configData)!;
            lorem = new Lorem();
            hacker = new Hacker();
        }

        public Task<Item> Generate()
        {
            var value = _rnd.NextSingle();
            var modifier = _modifiers.First(m => m.ValueBelow > value);

            var item = new Item
            {
                Name = hacker.Noun(),
                Rarity = modifier.Rarity,
                History = lorem.Paragraph()
            };

            foreach (var itemStat in Enum.GetValues<ItemStat>())
                item.SetStat(itemStat, modifier.StatRanges[itemStat].GenerateStat(_rnd));

            return Task.FromResult(item);
        }
    }
}
