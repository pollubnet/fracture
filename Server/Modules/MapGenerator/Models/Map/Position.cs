namespace Fracture.Server.Modules.MapGenerator.Models.Map;

public record struct Position(int X, int Y)
{
    public int X { get; init; } = X;
    public int Y { get; init; } = Y;
}
