namespace Fracture.Server.Modules.Shared.NameGenerators;

public class AsdfNameGenerator : INameGenerator
{
    public Task<string> GenerateNameAsync()
    {
        return Task.FromResult("Asdf");
    }
}
