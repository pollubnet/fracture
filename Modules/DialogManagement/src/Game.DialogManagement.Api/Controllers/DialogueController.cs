using System;
using System.Runtime.InteropServices;
using Game.DialogManagement.Api.Contracts.Requests;
using Game.DialogManagement.Application.Contracts.Commands;
using Game.DialogManagement.Application.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Game.DialogManagement.Api.Controllers
{
    /// <summary>
    /// The API controller for the Dialogue module.
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    [Area("DialogManagement")]
    public class DialogueController : ControllerBase
    {
        /// <summary>
        /// The MediatR instance.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructs a new dialogue controller with a given MediatR instance.
        /// </summary>
        /// <param name="mediator">The MediatR instance.</param>
        public DialogueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// The route invoked when a player wants to speak with an NPC.
        /// </summary>
        /// <param name="npcId">The id of the NPC.</param>
        [HttpPost("Talk/{npcId}")]
        public async Task<IActionResult> Talk(
            [FromRoute] Guid npcId,
            [FromBody] TalkRequest request
        )
        {
            var dialogue = await _mediator.Send(new GetDialogueQuery(npcId, request.PlayerId));
            if (dialogue is null)
            {
                // TODO: Mock data, we need to somehow fetch the NPC stuff
                //       when we'll have the module ready.
                const string npcName = "Lotus";
                const string npcStory =
                    "You are living in the city named Torn, in the northern regions of the Kingdom. You are working in a tavern, but your dream is to become a famous writer.";
                const string playerName = "Adventurer";

                dialogue = await _mediator.Send(
                    new CreateDialogueCommand(
                        npcId,
                        request.PlayerId,
                        npcName,
                        npcStory,
                        playerName
                    )
                );
            }

            var npcResponse = await _mediator.Send(
                new DialogueChatCommand(dialogue, request.Message)
            );
            dialogue.AddMessageToBacklog(dialogue.Context.PlayerName, request.Message);
            dialogue.AddMessageToBacklog(dialogue.Context.NpcName, npcResponse);

            await _mediator.Send(new UpdateDialogueCommand(dialogue));

            return Ok(npcResponse);
        }
    }
}
