namespace AdventOfCode;

public static class ArrayUtils
{
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

    public static bool ContainsPosition<T>(this T[,] array, (int x, int y) pos) =>
        ContainsPosition(array, pos.x, pos.y);

    public static bool ContainsPosition<T>(this T[,] array, int x, int y) =>
        x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);

    public static T[,] To2DArray<T>(this string[] array, Func<char, int, int, T> converter)
    {
        var charArray = new T[array[0].Length, array.Length];
        for (int y = 0; y < array.Length; y++)
        {
            for (int x = 0; x < array[0].Length; x++)
            {
                charArray[x, y] = converter(array[y][x], x, y);
            }
        }

        return charArray;
    }

    public static T[,] To2DArray<T>(this string[] array, Func<char, T> converter)
    {
        var charArray = new T[array[0].Length, array.Length];
        for (int y = 0; y < array.Length; y++)
        {
            for (int x = 0; x < array[0].Length; x++)
            {
                charArray[x, y] = converter(array[y][x]);
            }
        }

        return charArray;
    }

    public static void Print<T>(this T[,] array, Func<T, string>? toString = null)
    {
        for (int y = 0; y < array.GetLength(1); y++)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                if (toString != null) Console.Write(toString(array[x, y]));
                else Console.Write(array[x, y]);
            }

            Console.WriteLine();
        }
    }
}