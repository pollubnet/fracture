using System;
using Fracture.DialogManagement.Domain.Data.Entities;
using MediatR;

namespace Fracture.DialogManagement.Application.Contracts.Queries
{
    /// <summary>
    /// A query for fetching a dialogue by its player id and npc id.
    /// </summary>
    /// <param name="PlayerId">The player's id.</param>
    /// <param name="NpcId">The npc's id.</param>
    public record GetDialogueQuery(Guid PlayerId, Guid NpcId) : IRequest<Dialogue?> { }
}
