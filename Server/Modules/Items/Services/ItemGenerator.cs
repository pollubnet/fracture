using System.Text.Json;
using Fracture.Server.Modules.AI.Services;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;
using Fracture.Server.Modules.Shared;
using Fracture.Server.Modules.Shared.Configuration;
using Microsoft.FeatureManagement;

namespace Fracture.Server.Modules.Items.Services
{
    public class ItemGenerator : IItemGenerator
    {
        private readonly Random _rnd;
        private readonly List<RarityModifier> _modifiers;
        private readonly INameGenerator _nameGenerator;
        private readonly PrefixesGenerator _prefixes;
        private readonly IAIInstructionProvider? _ai;
        private readonly IFeatureManager _featureManager;

        public ItemGenerator(
            INameGenerator nameGenerator,
            PrefixesGenerator prefixes,
            NoiseParameters noiseParameters,
            IAIInstructionProvider? ai = null
        )
        {
            _nameGenerator = nameGenerator;
            _prefixes = prefixes;
            _ai = ai;

            _rnd = new Random(noiseParameters.Seed);

            var configData = File.ReadAllText("itemgeneratorconfig.json");
            _modifiers = JsonSerializer.Deserialize<List<RarityModifier>>(configData)!;
        }

        private async Task<string> GenerateDescription(Item item)
        {
            var prompt =
                $"You are generating descriptions for a fantasy game. Limit your responses to two sentences. Do not give your opinions.\n\nGenerate an interesting description of the item '{item.Name}' with the following stats: ";

            var stats = "";
            foreach (var itemStat in Enum.GetValues<ItemStat>())
            {
                stats += $"{itemStat}: {item.Statistics.GetStatFromItemStat(itemStat)}\n";
            }

            List<string> biomes = new List<string>()
            {
                "Snow",
                "Mountains",
                "Forest",
                "Tropics",
                "Grass",
                "Taiga",
                "Tundra",
                "Swamp",
                "Savanna",
                "Desert",
            };
            List<string> enemies = new List<string>()
            {
                "Giant spider",
                "Dragon",
                "Skeleton",
                "Harpy",
                "Vampire",
                "Knight",
            };

            string biome = biomes[_rnd.Next(biomes.Count)];
            string enemy = enemies[_rnd.Next(enemies.Count)];

            prompt += stats;
            prompt +=
                $"Item belongs to {biome} biome, consider using it to describe materials or characteristics of item. Imagine fantasy place related to this biome. \n";
            prompt +=
                $"Item stats are in range of -100 to 100, they can affect positivly or negativly on user. "
                + $"Do not refer to item stats directly, but describe item using fantasy style. Take into account that some stats can be negative. \n"
                + $"Do not use numbers to describe item stats.";

            prompt += $"Item is a reward for defeating {enemy}.";

            return await _ai.GenerateInstructionResponse(prompt);
        }

        public async Task<Item> Generate()
        {
            var value = _rnd.NextSingle();
            var modifier = _modifiers.First(m => m.ValueBelow > value);
            var item = new Item { Rarity = modifier.Rarity, CreatedAt = DateTime.UtcNow };

            var stats = new ItemStatistics { Item = item };

            foreach (var stat in Enum.GetValues<ItemStat>())
            {
                stats.SetStatFromItemStat(stat, modifier.StatRanges[stat].GenerateStat(_rnd));
            }

            item.Statistics = stats;

            var rndType = _rnd.Next(0, Enum.GetValues<ItemType>().Length);
            item.Type = (ItemType)rndType;

            await _prefixes.AddPrefixes(item);

            if (_ai is not null)
            {
                item.History = await GenerateDescription(item);
            }

            return item;
        }
    }
}
