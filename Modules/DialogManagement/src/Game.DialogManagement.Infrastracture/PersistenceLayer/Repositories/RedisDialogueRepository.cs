using System;
using System.Text.Json;
using Game.DialogManagement.Domain.Data.Entities;
using Game.DialogManagement.Domain.Repositories;
using StackExchange.Redis;

namespace Game.DialogManagement.Infrastracture.PersistenceLayer.Repositories
{
    /// <summary>
    /// A dialogue repository utilizing Redis for persistence.
    /// </summary>
    public class RedisDialogueRepository : IDialogueRepository
    {
        /// <summary>
        /// The Redis connection multiplexer.
        /// </summary>
        private readonly IConnectionMultiplexer _multiplexer;

        /// <summary>
        /// Constructs a new redis-backed dialogue repository.
        /// </summary>
        /// <param name="multiplexer">The connection multiplexer.</param>
        public RedisDialogueRepository(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        /// <summary>
        /// Converts a player id and an npc id to the key for a dialogue inside of Redis.
        /// </summary>
        /// <param name="playerId">The player's guid.</param>
        /// <param name="npcId">The npc's guid.</param>
        /// <returns>The constructed key.</returns>
        private static string IdPairToRedisKey(Guid playerId, Guid npcId) => $"{playerId}{npcId}";

        /// <inheritdoc/>
        public async Task<bool> Create(Dialogue dialogue)
        {
            var db = _multiplexer.GetDatabase();
            var key = IdPairToRedisKey(dialogue.PlayerId, dialogue.NpcId);

            // Check if we aren't duplicating an entry.
            if (await db.KeyExistsAsync(key))
                return false;

            var serializedDialogue = JsonSerializer.Serialize(dialogue);
            return await db.StringSetAsync(key, serializedDialogue);
        }

        /// <inheritdoc/>
        public async Task<Dialogue?> Get(Guid npcId, Guid playerId)
        {
            var db = _multiplexer.GetDatabase();
            var key = IdPairToRedisKey(npcId, playerId);

            // Check if we aren't duplicating an entry.
            if (!await db.KeyExistsAsync(key))
                return null;

            var redisValue = await db.StringGetAsync(key);
            var maybeDialogue = JsonSerializer.Deserialize<Dialogue>((string)redisValue!);
            return maybeDialogue;
        }

        /// <inheritdoc/>
        public async Task<bool> Update(Dialogue dialogue)
        {
            var db = _multiplexer.GetDatabase();
            var key = IdPairToRedisKey(dialogue.PlayerId, dialogue.NpcId);

            // Ensure we're actually updating something that exists.
            if (!await db.KeyExistsAsync(key))
                return false;

            var serializedDialogue = JsonSerializer.Serialize(dialogue);
            return await db.StringSetAsync(key, serializedDialogue);
        }
    }
}
