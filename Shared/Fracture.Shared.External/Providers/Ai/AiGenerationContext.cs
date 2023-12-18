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
        /// <para>Tokens, that will stop the further generation of the response,
        /// typically used to limit the response for example to not include the
        /// predicted next parts of the conversation.
        /// </para>
        /// </summary>
        public required string[] StopTokens { get; init; }

        /// <summary>
        /// The maximum count of the tokens predicted.
        /// <para>The number of the tokens predicted plus the number of the
        /// tokens in prompt must not exceed the model's context length.</para>
        /// </summary>
        public int MaxTokens { get; set; } = 128;

        /// <summary>
        /// Temperature, controls the randomness of the model
        /// <para>It should be between 0.0 and 2.0, higher values like 0.8 will
        /// make the output more random, while the lower values (e.g. 0.2) will
        /// make it more deterministic.</para>
        /// </summary>
        public double Temperature { get; set; } = 0.7;

        /// <summary>
        /// Nucleus sampling, controls probability of the tokens
        /// <para>0.1 will mean that only tokens from the top of 10% of
        /// probability are considered. Generally it is recommended to alter
        /// this or temperature, but not both.</para>
        /// </summary>
        public double TopP { get; set; } = 0.5;

        /// <summary>
        /// Constructs a new ai generation context with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt we're generating with.</param>
        /// <param name="stopTokens">The tokens the AI model should stop on.</param>
        /// <param name="tokenCount">The amount of tokens to generate.</param>
        [SetsRequiredMembers]
        public AiGenerationContext(
            string prompt,
            string[] stopTokens,
            int tokenCount = 128,
            double temperature = 0.7,
            double topP = 0.5
        )
        {
            Prompt = prompt;
            StopTokens = stopTokens;
            MaxTokens = tokenCount;
            Temperature = temperature;
            TopP = topP;
        }
    }
}
