namespace AdventOfCode.Aoc2024;

using Point = (int x, int y);

public class Day8 : IDay
{
    public void Run1(string[] data)
    {
        var input = data.To2DArray(x => x);
        var antennas = GetAntennas(input);
        var antinodes = new HashSet<Point>();

        foreach (var c in antennas.Values.SelectMany(x => x.Pairs()))
        {
            Point delta = (c.first.x - c.second.x, c.first.y - c.second.y);
            var first = c.first.Plus(delta);
            var second = c.second.Minus(delta);

            AddAntinodeIfPositionExists(input, first, antinodes);
            AddAntinodeIfPositionExists(input, second, antinodes);
        }

        Console.WriteLine($"Sum: {antinodes.Count}");
    }

    public void Run2(string[] data)
    {
        var input = data.To2DArray(x => x);
        var antennas = GetAntennas(input);
        var antinodes = new HashSet<Point>();

        foreach (var positions in antennas.Values)
        {
            antinodes.UnionWith(positions);
        }

        foreach (var c in antennas.Values.SelectMany(x => x.Pairs()))
        {
            Point delta = (c.first.x - c.second.x, c.first.y - c.second.y);
            int i = 1;

            while (true)
            {
                var current = c.first.Plus(delta.Times(i++));
                if (AddAntinodeIfPositionExists(input, current, antinodes) is false)
                {
                    break;
                }
            }

            i = 1;
            while (true)
            {
                var current = c.second.Minus(delta.Times(i++));
                if (AddAntinodeIfPositionExists(input, current, antinodes) is false)
                {
                    break;
                }
            }
        }

        Console.WriteLine($"Sum: {antinodes.Count}");
    }

    private static Dictionary<char, List<Point>> GetAntennas(char[,] input)
    {
        var antennas = new Dictionary<char, List<Point>>();
        input.Iterate((x, y, c) =>
        {
            if (c is not '.')
            {
                if (!antennas.ContainsKey(c))
                {
                    antennas.Add(c, []);
                }

                antennas[c].Add((x, y));
            }
        });

        return antennas;
    }

    private bool AddAntinodeIfPositionExists(char[,] input, Point position, HashSet<Point> antinodes)
    {
        bool inputContainsPosition = input.ContainsPosition(position);
        if (inputContainsPosition)
        {
            antinodes.Add(position);
        }

        return inputContainsPosition;
    }
}

file static class Day8Extensions
{
    public static Point Plus(this Point a, Point b) => (a.x + b.x, a.y + b.y);
    public static Point Minus(this Point a, Point b) => (a.x - b.x, a.y - b.y);
    public static Point Times(this Point a, int multiplier) => (a.x * multiplier, a.y * multiplier);
}