using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.NoiseGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface IMapGeneratorService
{
    Task<Map> GetMap(MapParameters? mapParameters);
    Map Map { get; }
}
