using System;
using MediatR;
using Fracture.DialogManagement.Application.Contracts.Commands;
using Fracture.DialogManagement.Domain.Data.Entities;
using Fracture.DialogManagement.Domain.Data.ValueObjects;
using Fracture.DialogManagement.Domain.Repositories;

namespace Fracture.DialogManagement.Application.Handlers.Commands
{
    /// <summary>
    /// The handler for the CreateDialogue command.
    /// </summary>
    public class CreateDialogueHandler : IRequestHandler<CreateDialogueCommand, Dialogue>
    {
        /// <summary>
        /// The repository holding all the dialogues.
        /// </summary>
        private readonly IDialogueRepository _dialogueRepository;

        /// <summary>
        /// Creates a new CreateDialogue handler with a given dialogue repo.
        /// </summary>
        /// <param name="dialogueRepository">The dialogue repo.</param>
        public CreateDialogueHandler(IDialogueRepository dialogueRepository)
        {
            _dialogueRepository = dialogueRepository;
        }

        /// <summary>
        /// Creates a dialogue from the parameters supplied by the command.
        /// </summary>
        /// <param name="request">The creation command.</param>
        /// <returns>The created dialogue entity.</returns>
        private static Dialogue CreateDialogue(CreateDialogueCommand request)
        {
            var context = new DialogueContext()
            {
                NpcName = request.NpcName,
                NpcStory = request.NpcStory,
                PlayerName = request.PlayerName
            };
            var dialogue = new Dialogue(request.PlayerId, request.NpcId, context);

            return dialogue;
        }

        /// <inheritdoc/>
        public async Task<Dialogue> Handle(
            CreateDialogueCommand request,
            CancellationToken cancellationToken
        )
        {
            var dialogue = CreateDialogue(request);
            if (!await _dialogueRepository.Create(dialogue))
            {
                // TODO: Handle creation failures.
            }

            return dialogue;
        }
    }
}
