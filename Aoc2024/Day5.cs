namespace AdventOfCode.Aoc2024;

public class Day5 : IDay
{
    public void Run1(string[] data)
    {
        var (order, inputs) = ParseInput(data);
        int sum = inputs
            .Where(x => InputIsInOrder(order, x))
            .Sum(x => x.ElementAt(x.Length / 2));

        Console.WriteLine($"Sum: {sum}");
    }

    public void Run2(string[] data)
    {
        var (order, inputs) = ParseInput(data);
        var comparer = new PageComparer(order);
        int sum = inputs
            .Where(x => !InputIsInOrder(order, x))
            .Sum(x => x.Order(comparer).ElementAt(x.Length / 2));

        Console.WriteLine($"Sum: {sum}");
    }

    private (Dictionary<int, HashSet<int>> order, int[][] inputs) ParseInput(string[] data)
    {
        var chunks = data.ChunkBy(x => x == string.Empty).ToList();

        var order = chunks[0].Select(x => x.Split('|'))
            .Select(x => (page: int.Parse(x[0]), order: int.Parse(x[1])))
            .GroupBy(x => x.page)
            .ToDictionary(k => k.Key, v => v.Select(x => x.order).ToHashSet());

        var inputs = chunks[1].Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();

        return (order, inputs);
    }

    private bool InputIsInOrder(Dictionary<int, HashSet<int>> order, int[] input) => input.All((current, i) =>
    {
        if (!order.ContainsKey(current))
        {
            return current == input.Last();
        }

        return order[current].ContainsAll(input[(i + 1)..]);
    });

    private class PageComparer(Dictionary<int, HashSet<int>> order) : IComparer<int>
    {
        public int Compare(int x, int y) => (order.GetValueOrDefault(x), order.GetValueOrDefault(y)) switch
        {
            (null, null) => throw new ArgumentException("Both values cannot be null."),
            (null, _) => 1,
            (_, null) => -1,
            _ => order[x].Contains(y) ? -1 : 1
        };
    }
}