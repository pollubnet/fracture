using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.Npc.Models;
using Fracture.Server.Modules.Shared;

namespace Fracture.Server.Modules.Npc.Services
{
    public class NpcProviderService(RandomProvider randomProviderService)
    {
        public List<NpcEntity> Npcs { get; } = [];

        public void InitializeNpcs(Map map)
        {
            // TODO: it should be done based on type of the map, and many more
            Npcs.Add(
                new NpcEntity()
                {
                    Id = 1,
                    Name = "Lotus",
                    Role = "A wandering bartender",
                }
            );
        }
    }
}
