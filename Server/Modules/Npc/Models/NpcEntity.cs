namespace Fracture.Server.Modules.Npc.Models
{
    public class NpcEntity
    {
        /// <summary>
        /// NPC's id
        /// </summary>
        public required int Id { get; set; }

        /// <summary>
        /// NPC's name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Short description, visible when there is NPC encounter
        /// e.g. "A wandering bartender"
        /// </summary>
        public required string Role { get; set; } = string.Empty;

        /// <summary>
        /// Personal history notes
        /// </summary>
        public string History { get; set; } = string.Empty;
    }
}
