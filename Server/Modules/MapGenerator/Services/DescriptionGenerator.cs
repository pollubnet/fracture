using Fracture.Server.Modules.AI.Services;
using Fracture.Server.Modules.MapGenerator.Models;
using Microsoft.AspNetCore.Components;

namespace Fracture.Server.Modules.MapGenerator.Services;

public class DescriptionGenerator
{
    [Parameter]
    public required MapData Map { get; set; }
    private readonly IAIInstructionProvider? _ai;

    public async Task<String> GenerateDescription(int posX, int posY)
    {
        var promt =
            $"You are generating description of a fantasy game. Limit your responses to two sentences. Do not give opinions. \n \n "
            + $"Generate interesting description of a location which biome is: {Map.Grid[posX, posY].Biome.BiomeType.ToString()}. Take into consideration nearby biomes which are: "
            + $"{Map.Grid[posX - 1, posY].Biome.BiomeType.ToString()} \n {Map.Grid[posX + 1, posY].Biome.BiomeType.ToString()}\n {Map.Grid[posX, posY + 1].Biome.BiomeType.ToString()}\n {Map.Grid[posX, posY - 1].Biome.BiomeType.ToString()}"
            + $"Do not forget these should only be an addition and your main focus is on biome: {Map.Grid[posX, posY].Biome.BiomeType.ToString()}.";
        return await _ai.GenerateInstructionResponse(promt);
    }
}
