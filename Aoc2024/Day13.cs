using System.Diagnostics;

namespace AdventOfCode.Aoc2024;

using Point = (long x, long y);

public class Day13 : IDay
{
    public void Run1(string[] data)
    {
        var minCost = data.ChunkBy(string.IsNullOrWhiteSpace)
            .Select(input => ParseInput(input))
            .Select(input => FindCost(input.a, input.b, input.prize))
            .Sum();

        Console.WriteLine($"Minimum cost: {minCost}");
    }

    public void Run2(string[] data)
    {
        var minCost = data.ChunkBy(string.IsNullOrWhiteSpace)
            .Select(input => ParseInput(input, 10000000000000))
            .Select(input => FindCost(input.a, input.b, input.prize))
            .Sum();

        Console.WriteLine($"Minimum cost: {minCost}");
    }

    private (Point a, Point b, Point prize) ParseInput(string[] input, long offset = 0)
    {
        return (Parse(input[0]), Parse(input[1]), Parse(input[2], offset));

        Point Parse(string s, long o = 0)
        {
            int ex = s.IndexOf('X') + 2;
            int comma = s.IndexOf(',');
            return (long.Parse(s[ex..comma]) + o, long.Parse(s[(comma + 4)..]) + o);
        }
    }

    private long FindCost(Point a, Point b, Point prize)
    {
        var inter = LineIntersection((0, 0), a, (prize.x + b.x, prize.y + b.y), prize);
        if (IsValid(inter.x, inter.y))
        {
            return (long)inter.x / a.x * 3 + (prize.y - (long)inter.y) / b.y;
        }

        return 0;

        bool IsValid(decimal x, decimal y) => decimal.IsInteger(x) && decimal.IsInteger(y);
    }

    private (decimal x, decimal y) LineIntersection(Point p1, Point p2, Point p3, Point p4)
    {
        // Line 1 represented as a1x + b1y = c1
        decimal a1 = p2.y - p1.y;
        decimal b1 = p1.x - p2.x;
        decimal c1 = a1 * p1.x + b1 * p1.y;

        // Line 2 represented as a2x + b2y = c2
        decimal a2 = p4.y - p3.y;
        decimal b2 = p3.x - p4.x;
        decimal c2 = a2 * p3.x + b2 * p3.y;

        decimal determinant = a1 * b2 - a2 * b1;
        //if (determinant == 0) return null;

        decimal x = (b2 * c1 - b1 * c2) / determinant;
        decimal y = (a1 * c2 - a2 * c1) / determinant;
        return (x, y);
    }
}