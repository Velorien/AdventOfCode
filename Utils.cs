namespace AdventOfCode;

public static class Utils
{
    public static IReadOnlyCollection<(int x, int y)> AdjacentNeighbours { get; } =
        [(-1, 0), (1, 0), (0, -1), (0, 1)];

    public static IReadOnlyCollection<(int x, int y)> AllNeighbours { get; } =
    [
        (-1, 1), (0, 1), (1, 1),
        (-1, 0), /* - */ (1, 0),
        (-1, -1), (0, -1), (1, -1)
    ];

    public static void Iterate<T>(this T[,] array, Action<int, int, T> action, Action? rowAction = null)
    {
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                action(x, y, array[x, y]);
            }

            rowAction?.Invoke();
        }
    }

    public static bool IsEdge<T>(this T[,] array, int x, int y) =>
        x == 0 || y == 0 || x == array.GetLength(0) - 1 || y == array.GetLength(1) - 1;

    public static bool ContainsPosition<T>(this T[,] array, int x, int y) =>
        x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(
        this IReadOnlyCollection<T> collection,
        Func<T, bool> predicate)
    {
        int start = 0, current = 0;
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                yield return collection.Skip(start).Take(current);
                start += ++current;
            }
            else current++;
        }

        yield return collection.Skip(start).Take(current);
    }
}