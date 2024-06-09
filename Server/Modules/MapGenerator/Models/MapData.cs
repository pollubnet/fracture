namespace Fracture.Server.Modules.MapGenerator.Models;

public class MapData
{
    public Node[,] Grid { get; set; }

    public MapData(Node[,] grid)
    {
        Grid = grid;
    }
}
