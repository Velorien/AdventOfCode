namespace AdventOfCode.Aoc2024;

public class Day6 : IDay
{
    private readonly (int dx, int dy)[] _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private int _dirIndex = 0;
    private int _startX;
    private int _startY;

    public void Run1(string[] data)
    {
        var map = data.To2DCharArray();
        FindStart(map);
        Console.WriteLine($"Visited {CountVisitedTiles(map)} tiles");
    }

    public void Run2(string[] data)
    {
        var map = data.To2DCharArray();
        FindStart(map);
        HashSet<(int x, int y)> visited = [];
        CountVisitedTiles(map, visited);
        var candidates = visited.Except([(_startX, _startY)]);
        int possibleLocations = 0;
        foreach (var (cx, cy) in candidates)
        {
            map[cx, cy] = '#';
            _dirIndex = 0;
            if (CountVisitedTiles(map) == -1)
            {
                possibleLocations++;
            }

            map[cx, cy] = '.';
        }

        Console.WriteLine($"Obstacle can be placed on {possibleLocations} tiles");
    }

    private void FindStart(char[,] map)
    {
        map.Iterate((cx, cy, c) =>
        {
            if (c is '^')
            {
                _startX = cx;
                _startY = cy;
            }
        });
    }

    private int CountVisitedTiles(char[,] map, HashSet<(int x, int y)>? visited = null)
    {
        int x = _startX, y = _startY;
        HashSet<(int x, int y, int dir)> visitedWithDirection = [(x, y, _dirIndex)];
        visited ??= [];
        visited.Add((x, y));

        try
        {
            while (true)
            {
                var (dx, dy) = _directions[_dirIndex];
                if (map[x + dx, y + dy] == '#')
                {
                    _dirIndex++;
                    _dirIndex %= _directions.Length;
                }
                else
                {
                    x += dx;
                    y += dy;
                    if (!visitedWithDirection.Contains((x, y, _dirIndex)))
                    {
                        visited.Add((x, y));
                        visitedWithDirection.Add((x, y, _dirIndex));
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
        catch (IndexOutOfRangeException)
        {
            return visited.Count;
        }
    }
}