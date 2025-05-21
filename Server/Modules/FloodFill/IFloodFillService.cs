namespace Fracture.Server.Modules.FloodFill;

public interface IFloodFillService<T>
{
    Dictionary<string, List<(int x, int y)>> FindGroups(
        T[,] grid,
        Func<T, bool> isValidElement,
        Func<T, T, bool> areEqual,
        Func<T, string, string> generateGroupName
    );
}
