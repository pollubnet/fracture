namespace Fracture.Server.Modules.Shared.Configuration;

/// <summary>
///     A configuration of a name generator
/// </summary>
public class NameGeneratorConfig
{
    /// <summary>
    ///     Default set of names which will be used as a base for name
    ///     generator
    /// </summary>
    public string[]? DefaultNameBase { get; set; }
}
