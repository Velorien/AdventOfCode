using System.Numerics;

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

    public static void Iterate<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }

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

    public static IEnumerable<IReadOnlyList<T>> Combinations<T>(this IList<T> collection, int k)
    {
        ulong max = (ulong)Math.Pow(collection.Count, k);
        var buffer = new T[k];
        for (ulong i = 0; i <= max; i++)
        {
            SetCombination(i);
            yield return buffer;
        }

        void SetCombination(ulong value)
        {
            for (int i = 0; i < k; i++)
            {
                buffer[i] = collection[(int)(value % (ulong)collection.Count)];
                value /= (ulong)collection.Count;
            }
        }
    }

    public static IEnumerable<(T first, T second)> Pairs<T>(this IReadOnlyList<T> collection)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            for (int j = i + 1; j < collection.Count; j++)
            {
                yield return (collection[i], collection[j]);
            }
        }
    }

    public static T GetNumberOfDigits<T>(this T number)
        where T : INumber<T>, IComparisonOperators<T, T, bool>, IMultiplyOperators<T, T, T>, IAdditionOperators<T, T, T>
    {
        T length = T.CreateChecked(1), threshold = T.CreateChecked(9);
        T ten = T.CreateChecked(10);
        T nine = T.CreateChecked(9);

        while (number > threshold)
        {
            length++;
            threshold *= ten;
            threshold += nine;
        }

        return length;
    }
}