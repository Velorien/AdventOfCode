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

    public static IEnumerable<T[]> ChunkBy<T>(
        this IReadOnlyCollection<T> collection,
        Func<T, bool> predicate,
        bool ignoreEmpty = false)
    {
        var buffer = new List<T>();
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                if (buffer.Count != 0 || !ignoreEmpty)
                {
                    yield return buffer.ToArray();
                }

                buffer.Clear();
            }
            else
            {
                buffer.Add(item);
            }
        }

        if (buffer.Count != 0 || !ignoreEmpty)
        {
            yield return buffer.ToArray();
        }
    }

    public static bool ContainsAll<T>(this IReadOnlyCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            if (!collection.Contains(item))
            {
                return false;
            }
        }

        return true;
    }

    public static int FirstIndexOf<T>(this IReadOnlyCollection<T> collection, Func<T, bool> predicate)
    {
        int index = 0;
        foreach (var item in collection)
        {
            if (predicate(item))
            {
                return index;
            }

            index++;
        }

        throw new InvalidOperationException("No matching item found");
    }

    public static int FirstIndexOf<T>(this IReadOnlyCollection<T> collection, T item) =>
        FirstIndexOf(collection, x => x?.Equals(item) ?? x == null && item == null);

    public static bool All<T>(this IReadOnlyCollection<T> collection, Func<T, int, bool> predicate)
    {
        int index = 0;
        foreach (var item in collection)
        {
            if (!predicate(item, index++)) return false;
        }

        return true;
    }
}