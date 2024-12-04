namespace AdventOfCode.Aoc2024;

public class Day4 : IDay
{
    static char[] _mas = ['M', 'A', 'S'];

    public void Run1(string[] data)
    {
        var xmasMap = ParseData(data);
        List<(int x, int y)> startPoints = [];
        xmasMap.Iterate((x, y, c) =>
        {
            if (c == 'X')
            {
                startPoints.Add((x, y));
            }
        });

        int xmasCount = 0;
        foreach (var (x, y) in startPoints)
        {
            foreach (var (dx, dy) in GetDirections())
            {
                if (IsXmasInDirection(x, y, dx, dy, xmasMap))
                {
                    xmasCount++;
                }
            }
        }

        Console.WriteLine($"Xmas: {xmasCount}");
    }

    public void Run2(string[] data)
    {
        var xmasMap = ParseData(data);
        List<(int x, int y)> startPoints = [];
        xmasMap.Iterate((x, y, c) =>
        {
            if (c == 'A')
            {
                startPoints.Add((x, y));
            }
        });

        int xmasCount = 0;
        foreach (var (x, y) in startPoints)
        {
            if (IsMasInAnX(x, y, xmasMap))
            {
                xmasCount++;
            }
        }

        Console.WriteLine($"MAS in X: {xmasCount}");
    }

    char[,] ParseData(string[] data)
    {
        var result = new char[data.Length, data[0].Length];
        for (int i = 0; i < data.Length; i++)
        {
            for (int j = 0; j < data[i].Length; j++)
            {
                result[i, j] = data[i][j];
            }
        }

        return result;
    }

    IEnumerable<(int x, int y)> GetDirections()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    yield return (i, j);
                }
            }
        }
    }

    bool IsXmasInDirection(int x, int y, int dx, int dy, char[,] data)
    {
        foreach (var c in _mas)
        {
            x += dx;
            y += dy;
            if (!data.ContainsPosition(x, y) || data[x, y] != c)
            {
                return false;
            }
        }

        return true;
    }

    bool IsMasInAnX(int x, int y, char[,] data)
    {
        if (data.ContainsPosition(x - 1, y - 1) &&
            data.ContainsPosition(x - 1, y + 1) &&
            data.ContainsPosition(x + 1, y - 1) &&
            data.ContainsPosition(x + 1, y + 1) &&
            (data[x - 1, y - 1], data[x + 1, y + 1]) is ('M', 'S') or ('S', 'M') &&
            (data[x + 1, y - 1], data[x - 1, y + 1]) is ('M', 'S') or ('S', 'M'))
        {
            return true;
        }

        return false;
    }
}