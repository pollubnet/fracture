using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface ISubMapAssignmentService
{
    Task AssignSubMapsAsync(Map map, Dictionary<string, MapParameters> parameters);
}
