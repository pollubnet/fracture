using Fracture.Server.Modules.AI.Models;

namespace Fracture.Server.Modules.AI.Services;

/// <summary>
///     Provides the ability to generate the response to a prompt or instruction
/// </summary>
public interface IAIInstructionProvider
{
    /// <summary>
    ///     Generates the response to an instruction (for instruction-following models)
    /// </summary>
    /// <param name="instruction">The actual instruction for the model to follow.</param>
    /// <returns>The resulting generated response to said instruction.</returns>
    Task<string?> GenerateInstructionResponse(string instruction);

    /// <summary>
    ///     Generates the response for a generation context, which is including prompt
    ///     and more parameters.
    /// </summary>
    /// <param name="context">Generation context with prompt and generation parameters</param>
    /// <returns>The resulting generated response to the given context.</returns>
    Task<string?> GenerateResponse(AIGenerationContext context);
}
