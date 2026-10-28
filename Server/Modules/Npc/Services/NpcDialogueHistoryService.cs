namespace Fracture.Server.Modules.Npc.Services;

public class NpcDialogueHistoryService
{
    private readonly Dictionary<int, List<string>> _dialogues = [];

    public List<string> GetDialogue(int npcId, out bool isReturningEncounter)
    {
        isReturningEncounter = _dialogues.TryGetValue(npcId, out var messages);
        if (!isReturningEncounter)
        {
            messages = ["NPC: Hello! What would you like to talk about?"];
            _dialogues.Add(npcId, messages);
        }

        return messages!;
    }
}
