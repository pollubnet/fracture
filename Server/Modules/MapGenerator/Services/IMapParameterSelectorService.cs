using Fracture.Server.Modules.MapGenerator.Models.Map;

namespace Fracture.Server.Modules.MapGenerator.Services;

public interface IMapParameterSelectorService
{
    Task<Dictionary<string, MapParameters>> GetParametersAsync();
    MapParameters SelectRandom(Dictionary<string, MapParameters> parameters, LocationType type);
}
