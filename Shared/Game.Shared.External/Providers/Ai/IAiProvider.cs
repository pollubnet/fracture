using System;

namespace Game.Shared.External.Providers.Ai
{
    /// <summary>
    /// Provides the ability to generate the response to a prompt.
    /// </summary>
    public interface IAiProvider
    {
        /// <summary>
        /// Generates the response to a prompt.
        /// </summary>
        /// <param name="prompt">The context.</param>
        /// <returns>The resulting generated response to said prompt.</returns>
        Task<string> Generate(AiGenerationContext prompt);
    }
}
