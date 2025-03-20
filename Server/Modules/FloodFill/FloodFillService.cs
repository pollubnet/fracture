namespace Fracture.Server.Modules.FloodFill;

public class FloodFillService<T> : IFloodFillService<T>
{
    private readonly int[,] directions;

    public FloodFillService(bool useDiagonals = false)
    {
        directions = useDiagonals
            ? new int[,]
            {
                { 0, 1 },
                { 1, 0 },
                { 0, -1 },
                { -1, 0 },
                { 1, 1 },
                { -1, -1 },
                { 1, -1 },
                { -1, 1 },
            }
            : new int[,]
            {
                { 0, 1 },
                { 1, 0 },
                { 0, -1 },
                { -1, 0 },
            };
    }

    public Dictionary<string, List<(int x, int y)>> FindGroups(
        T[,] grid,
        Func<T, bool> isValidElement,
        Func<T, T, bool> areEqual,
        Func<T, string, string> generateGroupName
    )
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        bool[,] visited = new bool[width, height];
        Dictionary<string, List<(int x, int y)>> groups = new();

        int groupId = 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (visited[x, y] || !isValidElement(grid[x, y]))
                    continue;

                string groupName = generateGroupName(grid[x, y], groupId.ToString());
                groupId++;

                var group = Flood(grid, visited, x, y, areEqual, isValidElement);
                groups[groupName] = group;
            }
        }

        return groups;
    }

    private List<(int x, int y)> Flood(
        T[,] grid,
        bool[,] visited,
        int startX,
        int startY,
        Func<T, T, bool> areEqual,
        Func<T, bool> isValidElement
    )
    {
        List<(int x, int y)> result = new();
        Queue<(int x, int y)> queue = new();
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        T target = grid[startX, startY];
        queue.Enqueue((startX, startY));

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            if (x < 0 || y < 0 || x >= width || y >= height)
                continue;
            if (visited[x, y] || !isValidElement(grid[x, y]) || !areEqual(grid[x, y], target))
                continue;

            visited[x, y] = true;
            result.Add((x, y));

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int nx = x + directions[i, 0];
                int ny = y + directions[i, 1];
                queue.Enqueue((nx, ny));
            }
        }

        return result;
    }
}
