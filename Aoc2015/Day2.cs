namespace AdventOfCode.Aoc2015;

public class Day2 : IDay
{
    public void Run1(string[] data)
    {
        var totalArea = data.Select(l => l.Split('x').Select(int.Parse).ToArray())
            .Select(GetWrappingArea)
            .Sum();

        Console.WriteLine($"Total Area: {totalArea}");
    }

    public void Run2(string[] data)
    {
        var ribbonLength = data.Select(l => l.Split('x').Select(int.Parse).ToList())
            .Select(GetRibbonLength)
            .Sum();

        Console.WriteLine($"Total Ribbon: {ribbonLength}");
    }

    private int GetWrappingArea(int[] sides)
    {
        var s1 = sides[0] * sides[1];
        var s2 = sides[1] * sides[2];
        var s3 = sides[2] * sides[0];

        return (s1 + s2 + s3) * 2 + Math.Min(Math.Min(s1, s2), s3);
    }

    private int GetRibbonLength(List<int> sides)
    {
        sides.Sort();
        return (sides[0] + sides[1]) * 2 + sides[0] * sides[1] * sides[2];
    }
}