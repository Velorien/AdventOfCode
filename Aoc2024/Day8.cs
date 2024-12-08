namespace AdventOfCode.Aoc2024;

using Point = (int x, int y);

public class Day8 : IDay
{
    public void Run1(string[] data)
    {
        var input = data.To2DCharArray();
        var antennas = GetAntennas(input);
        var antinodes = new HashSet<Point>();

        antennas.Values
            .SelectMany(x => x.Pairs())
            .Iterate(c =>
            {
                Point delta = (c.first.x - c.second.x, c.first.y - c.second.y);
                var first = c.first.Plus(delta);
                var second = c.second.Minus(delta);

                if (input.ContainsPosition(first))
                {
                    antinodes.Add(first);
                }

                if (input.ContainsPosition(second))
                {
                    antinodes.Add(second);
                }
            });

        Console.WriteLine($"Sum: {antinodes.Count}");
    }

    public void Run2(string[] data)
    {
        var input = data.To2DCharArray();
        var antennas = GetAntennas(input);
        var antinodes = new HashSet<Point>();
        foreach (var positions in antennas.Values)
        {
            antinodes.UnionWith(positions);
        }

        antennas.Values
            .SelectMany(x => x.Pairs())
            .Iterate(c =>
            {
                Point delta = (c.first.x - c.second.x, c.first.y - c.second.y);
                int i = 1;

                while (true)
                {
                    var current = c.first.Plus(delta.Times(i));
                    if (input.ContainsPosition(current))
                    {
                        antinodes.Add(current);
                        i++;
                    }
                    else break;
                }

                i = 1;
                while (true)
                {
                    var current = c.second.Minus(delta.Times(i));
                    if (input.ContainsPosition(current))
                    {
                        antinodes.Add(current);
                        i++;
                    }
                    else break;
                }
            });

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
}

file static class Day8Extensions
{
    public static Point Plus(this Point a, Point b) => (a.x + b.x, a.y + b.y);
    public static Point Minus(this Point a, Point b) => (a.x - b.x, a.y - b.y);
    public static Point Times(this Point a, int multiplier) => (a.x * multiplier, a.y * multiplier);
}