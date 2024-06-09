using MapGenerator.MapGenerators.Data;
using MapGenerator.NoiseGenerators.Data;

namespace MapGenerator.MapGenerators.Services;

public interface IMapGeneratorService
{
    Task<MapData> GetMap(NoiseParameters noiseParameters);
    MapData MapData { get; }
}
