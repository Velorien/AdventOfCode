namespace AdventOfCode;

public static class Utils
{
    public static void Iterate<T>(T[,] array, Action<int, int, T> action, Action? rowAction = null)
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

    public static bool IsEdge<T>(T[,] array, int x, int y) =>
        x == 0 || y == 0 || x == array.GetLength(0) - 1 || y == array.GetLength(1) - 1;
}