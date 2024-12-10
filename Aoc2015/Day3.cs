namespace AdventOfCode.Aoc2015;

using Point = (int x, int y);

public class Day3 : IDay
{
    private Dictionary<char, Point> _points = new()
    {
        { '^', (0, -1) },
        { '<', (-1, 0) },
        { '>', (1, 0) },
        { 'v', (0, 1) }
    };

    public void Run1(string[] data)
    {
        var visited = new HashSet<Point>() { (0, 0) };
        int x = 0, y = 0;
        foreach (var c in data[0])
        {
            x += _points[c].x;
            y += _points[c].y;
            visited.Add((x, y));
        }

        Console.WriteLine($"Visited {visited.Count}");
    }

    public void Run2(string[] data)
    {
        var visited = new HashSet<Point> { (0, 0) };
        for (int isRobot = 0; isRobot < 2; isRobot++)
        {
            int x = 0, y = 0;
            for (int i = 0; i < data[0].Length; i++)
            {
                if (i % 2 == isRobot)
                {
                    var c = data[0][i];
                    x += _points[c].x;
                    y += _points[c].y;
                    visited.Add((x, y));
                }
            }
        }

        Console.WriteLine($"Visited {visited.Count}");
    }
}