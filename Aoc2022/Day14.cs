using System.Drawing;

namespace AdventOfCode.Aoc2022;

public class Day14 : IDay
{
    private readonly List<(int dx, int dy)> _nextStep = new() { (0, 1), (-1, 1), (1, 1) };
    
    public void Run1(string[] data)
    {
        var cave = GetCaveMap(data);
        var maxY = cave.GetLength(1) - 1;
        int cycles = 0;
        
        while (true)
        {
            cycles++;
            var (sx, sy) = SimulateSand(cave);

            if (sy == maxY) break;
            cave[sx, sy] = true;
        }

        Console.WriteLine($"It takes {cycles - 1} sand to fill the cave");
    }

    public void Run2(string[] data)
    {
        var cave = GetCaveMap(data);
        for (int i = 0; i < cave.GetLength(0); i++)
        {
            cave[i, cave.GetLength(1) - 1] = true;
        }
        
        int cycles = 0;
        
        while (true)
        {
            cycles++;
            var (sx, sy) = SimulateSand(cave);

            if (sy == 0) break;
            cave[sx, sy] = true;
        }
        
        Console.WriteLine($"It takes {cycles} sand to fill the cave");
    }

    (int x, int y) SimulateSand(bool[,] cave)
    {
        int sx = 500, sy = 0;
        bool canMove = false;
        do
        {
            for (int i = 0; i < 3; i++)
            {
                var s = _nextStep[i];
                var nx = sx + s.dx;
                var ny = sy + s.dy;

                if (!cave[nx, ny])
                {
                    canMove = true;
                    sx = nx;
                    sy = ny;
                    break;
                }
                    
                canMove = false;
            }
        } while (canMove && sy < cave.GetLength(1) - 1);

        return (sx, sy);
    }

    bool[,] GetCaveMap(string[] data)
    {
        (int x, int y) ParsePoint(string s)
        {
            var pts = s.Split(',');
            return (int.Parse(pts[0]), int.Parse(pts[1]));
        }

        var lines = data.Select(d => d.Split(" -> ").Select(ParsePoint));
        var maxX = lines.Max(p => p.Max(p => p.x));
        var maxY = lines.Max(p => p.Max(p => p.y));

        var caveMap = new bool[maxX + 500, maxY + 3];
        foreach (var line in lines)
        {
            var (x, y) = line.Aggregate((p, n) =>
            {
                int x = p.x;
                int y = p.y;
                int dx = int.Sign(n.x - p.x);
                int dy = int.Sign(n.y - p.y);
                do
                {
                    do
                    {
                        caveMap[x, y] = true;
                        y += dy;
                    } while (y != n.y);
                    x += dx;
                } while (x != n.x);
                
                return n;
            });

            caveMap[x, y] = true;
        }

        return caveMap;
    }
}