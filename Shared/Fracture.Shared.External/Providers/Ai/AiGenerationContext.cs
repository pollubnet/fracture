using System;
using System.Diagnostics.CodeAnalysis;

namespace Fracture.Shared.External.Providers.Ai
{
    /// <summary>
    /// The neccessary thing for generating AI responses.
    /// </summary>
    public class AiGenerationContext
    {
        /// <summary>
        /// The prompt for generation.
        /// </summary>
        public required string Prompt { get; init; }

        /// <summary>
        /// The stop tokens.
        /// </summary>
        public required string[] StopTokens { get; init; }

        /// <summary>
        /// The token count.
        /// </summary>
        public int TokenCount { get; set; } = 128;

        /// <summary>
        /// Constructs a new ai generation context with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt we're generating with.</param>
        /// <param name="stopTokens">The tokens the AI model should stop on.</param>
        /// <param name="tokenCount">The amount of tokens to generate.</param>
        [SetsRequiredMembers]
        public AiGenerationContext(string prompt, string[] stopTokens, int tokenCount = 128)
        {
            Prompt = prompt;
            StopTokens = stopTokens;
            TokenCount = tokenCount;
        }
    }
}
