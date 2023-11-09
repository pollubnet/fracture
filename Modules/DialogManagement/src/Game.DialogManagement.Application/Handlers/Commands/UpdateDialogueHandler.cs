﻿using System;
using MediatR;
using Game.DialogManagement.Application.Contracts.Commands;
using Game.DialogManagement.Domain.Repositories;

namespace Game.DialogManagement.Application.Handlers.Commands
{
    /// <summary>
    /// The handler for the UpdateDialogue command.
    /// </summary>
    public class UpdateDialogueHandler : IRequestHandler<UpdateDialogueCommand>
    {
        /// <summary>
        /// The repository holding all the dialogues.
        /// </summary>
        private readonly IDialogueRepository _dialogueRepository;

        /// <summary>
        /// Creates a new UpdateDialogue handler with a given dialogue repo.
        /// </summary>
        /// <param name="dialogueRepository">The dialogue repo.</param>
        public UpdateDialogueHandler(IDialogueRepository dialogueRepository)
        {
            _dialogueRepository = dialogueRepository;
        }

        /// <inheritdoc/>
        public async Task Handle(UpdateDialogueCommand request, CancellationToken cancellationToken)
        {
            await _dialogueRepository.Update(request.Dialogue);
        }
    }
}
