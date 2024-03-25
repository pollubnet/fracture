using System;

namespace ExampleItemGenerator.Services.Generators
{
    public class TemporaryNameGenerator : INameGenerator
    {
        public async Task<string> GenerateName()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return "Cool Item";
        }
    }
}
