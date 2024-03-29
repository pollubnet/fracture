using System;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.Items.Services
{
    public class ChristmasNameGenerator : INameGenerator
    {
        public async Task<string> GenerateNameAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return "Christmas Item :) 🎄";
        }
    }
}
