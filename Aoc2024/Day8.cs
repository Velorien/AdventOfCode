namespace AdventOfCode.Aoc2024;
using Point = (int x, int y);

public class Day8 : IDay
{
    public void Run1(string[] data)
    {
        var input = data.To2DCharArray();
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
    }

    public void Run2(string[] data)
    {
        throw new NotImplementedException();
    }
}