using Fracture.Server.Modules.MapGenerator.Models.Map;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.MapGenerator.UI;
using Fracture.Server.Modules.Users.Services;

namespace Fracture.Server.Modules.Shared.ImageChangers;

public class BackgroundImageChanger(
    ILogger<BackgroundImageChanger> logger,
    MovementService movementService
) { }
