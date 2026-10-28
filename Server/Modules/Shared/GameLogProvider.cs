using System.Collections.ObjectModel;
using Fracture.Server.Modules.AI.Services;

namespace Fracture.Server.Modules.Shared
{
    public class GameLogProvider(
        SingleResponseProvider? singleResponseProvider,
        ILogger<GameLogProvider> logger
    )
    {
        /// <summary>
        /// Logs a new message to the user-facing, fantasy-themed game log. If a <see cref="SingleResponseProvider"/> is provided, the message will be processed through it to generate a more immersive response.
        ///
        /// Either the <paramref name="logType"/> or the <paramref name="prompt"/> can be used to influence the response generation. If both are provided, the <paramref name="prompt"/> takes precedence.
        /// </summary>
        /// <param name="message">A message to be added</param>
        /// <param name="logType">The type of the log entry (predefined LLM prompts)</param>
        /// <param name="prompt">An optional prompt for generating an immersive response</param>
        /// <returns></returns>
        public async Task LogAsync(
            string message,
            GameLogType? logType = null,
            string? prompt = null
        )
        {
            string systemPrompt =
                "You are a fantasy world narrator. You will be given a message and you will generate a poetic, immersive, and fantasy-themed description of the message. Limit yourself to two sentences. Use the form \"you are\": ";

            if (prompt is null && logType is not null)
            {
                prompt = logType switch
                {
                    GameLogType.Born => "Teleported into a new world",
                    _ => null,
                };
            }

            string response;
            if (singleResponseProvider is not null)
            {
                response =
                    await singleResponseProvider.GenerateResponseAsync(
                        systemPrompt + prompt + message
                    ) ?? message;
            }
            else
            {
                response = message;
            }

            Logs.Add(new GameLog(DateTime.Now, response, prompt + message));

            if (logType is not null)
            {
                logger.LogInformation(
                    "New Game Log Entry: Canned/{logType} {message}",
                    logType,
                    message
                );
            }
            else
            {
                logger.LogInformation("New Game Log Entry: {prompt} {message}", prompt, message);
            }
        }

        /// <summary>
        /// List of all user-facing, fantasy-themed game log entries.
        /// </summary>
        public ObservableCollection<GameLog> Logs { get; } = [];
    }

    /// <summary>
    /// A single game log
    /// </summary>
    /// <param name="Timestamp">Timestamp of the message (server time)</param>
    /// <param name="Message">The message to be shown to the user</param>
    /// <param name="Original">The original (debug) message</param>
    public record GameLog(DateTime Timestamp, string Message, string Original);

    /// <summary>
    /// Predefined types of log prompts to generate user-facing immersive descriptions
    /// </summary>
    public enum GameLogType
    {
        /// <summary>
        /// When the user is born in a new world (a new world is generated)
        /// </summary>
        Born,
    }
}
