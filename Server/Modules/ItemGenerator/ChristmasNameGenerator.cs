using System;

namespace ExampleItemGenerator.Services.Generators
{
    public class ChristmasNameGenerator : INameGenerator
    {
        public async Task<string> GenerateName()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return "Christmas Item :) 🎄";
        }
    }
}
