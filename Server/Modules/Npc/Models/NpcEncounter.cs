using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Models.Map.MapObjects;

namespace Fracture.Server.Modules.Npc.Models
{
    public record NpcEncounter(NpcMapObject mapObject, NpcEntity Entity, Position Position);
}
