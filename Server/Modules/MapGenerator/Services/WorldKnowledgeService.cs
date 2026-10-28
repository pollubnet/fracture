namespace Fracture.Server.Modules.MapGenerator.Services
{
    public class WorldKnowledgeService
    {
        public string Knowledge { get; private set; } = string.Empty;

        public void Update(string knowledge)
        {
            Knowledge += knowledge + "\n";
        }
    }
}
