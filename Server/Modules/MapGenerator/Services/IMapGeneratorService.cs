using Fracture.Server.Modules.MapGenerator.Models;
using Fracture.Server.Modules.NoiseGenerator.Models;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface IMapGeneratorService
{
    Task<MapData> GetMap(NoiseParameters noiseParameters);
    MapData MapData { get; }
}
