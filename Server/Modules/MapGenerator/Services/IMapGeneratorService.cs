using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface IMapGeneratorService
{
    Task<Map> GetMap(MapParameters? mapParameters);
}
