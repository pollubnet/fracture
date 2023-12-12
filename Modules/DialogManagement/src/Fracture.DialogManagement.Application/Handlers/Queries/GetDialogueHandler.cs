using System;
using Fracture.DialogManagement.Application.Contracts.Queries;
using Fracture.DialogManagement.Domain.Data.Entities;
using Fracture.DialogManagement.Domain.Repositories;
using MediatR;

namespace Fracture.DialogManagement.Application.Handlers.Queries
{
    /// <summary>
    /// The handler for the GetDialogue query.
    /// </summary>
    public class GetDialogueHandler : IRequestHandler<GetDialogueQuery, Dialogue?>
    {
        /// <summary>
        /// The dialogue repository.
        /// </summary>
        private readonly IDialogueRepository _dialogueRepository;

        /// <summary>
        /// Constructs a new GetDialogue handler, with a given dialogue repository.
        /// </summary>
        /// <param name="dialogueRepository">The dialogue repository.</param>
        public GetDialogueHandler(IDialogueRepository dialogueRepository)
        {
            _dialogueRepository = dialogueRepository;
        }

        /// <inheritdoc/>
        public async Task<Dialogue?> Handle(
            GetDialogueQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _dialogueRepository.Get(request.NpcId, request.PlayerId);
        }
    }
}
