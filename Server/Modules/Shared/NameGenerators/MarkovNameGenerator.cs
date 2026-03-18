using Fracture.Server.Modules.Shared.Configuration;
using Markov;
using Microsoft.Extensions.Options;

namespace Fracture.Server.Modules.Shared.NameGenerators;

/// <summary>
///     A simple Markov-chains-based name generator providing names similar
///     to the defined database
/// </summary>
public class MarkovNameGenerator : INameGenerator
{
    private readonly MarkovChain<char> _chain;
    private HashSet<string> _names = new();

    /// <summary>
    ///     Initializes a new instance of a generator
    /// </summary>
    /// <param name="options">Name generator options</param>
    public MarkovNameGenerator(IOptions<NameGeneratorConfig> options)
    {
        if (options.Value.DefaultNameBase is null)
            throw new ArgumentNullException(
                nameof(options.Value.DefaultNameBase),
                "A base set of names is required"
            );

        _chain = new MarkovChain<char>(2);

        foreach (var name in options.Value.DefaultNameBase)
            _chain.Add(name);
    }

    /// <inheritdoc />
    public Task<string> GenerateNameAsync()
    {
        string s;
        do
        {
            var chains = _chain.Chain(Random.Shared).ToArray();
            s = new string(chains);
        } while (_names.Contains(s));
        _names.Add(s);

        return Task.FromResult(s);
    }
}
