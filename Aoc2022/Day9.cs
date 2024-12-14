using System.Diagnostics;

namespace AdventOfCode.Aoc2022;

public class Day9 : IDay
{
    public void Run1(string[] data) => SimulateRope(2, data);

    public void Run2(string[] data) => SimulateRope(10, data);

    private void SimulateRope(int ropeLength, string[] data)
    {
        var moves = ParseMoves(data);
        var rope = Enumerable.Repeat((x: 0, y: 0), ropeLength).ToArray();
        HashSet<(int x, int y)> tailPositions = new() { (0, 0) };

        foreach (var (x, y, d) in moves)
        {
            for (int i = 0; i < d; i++)
            {
                rope[0] = (rope[0].x + x, rope[0].y + y);
                for (int knot = 1; knot < rope.Length; knot++)
                {
                    var current = rope[knot];
                    var dx = rope[knot - 1].x - current.x;
                    var dy = rope[knot - 1].y - current.y;

                    if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                    {
                        rope[knot] = (current.x + int.Sign(dx), current.y + int.Sign(dy));
                        if (knot == rope.Length - 1)
                            tailPositions.Add(rope[knot]);
                    }
                }
            }
        }

        Console.WriteLine($"Tail visited {tailPositions.Count} positions");
    }

    private List<(int x, int y, int d)> ParseMoves(string[] data) => data.Select(x =>
    {
        var items = x.Split(' ');
        return items switch
        {
            ["L", var count] => (x: -1, y: 0, d: int.Parse(count)),
            ["R", var count] => (x: 1, y: 0, d: int.Parse(count)),
            ["U", var count] => (x: 0, y: -1, d: int.Parse(count)),
            ["D", var count] => (x: 0, y: 1, d: int.Parse(count)),
            _ => throw new UnreachableException()
        };
    }).ToList();
}