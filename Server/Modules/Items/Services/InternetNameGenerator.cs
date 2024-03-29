using System.Text.Json;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.Items.Services;

public class InternetNameGenerator : INameGenerator
{
    public async Task<string> GenerateName()
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(
            "https://random-word-api.herokuapp.com/word"
        );

        var nameList = JsonSerializer.Deserialize<string[]>(response);
        if (nameList is null)
            throw new InvalidOperationException("Nazwa zwrocona przez api jest nullem.");
        return nameList[0];
    }
}
