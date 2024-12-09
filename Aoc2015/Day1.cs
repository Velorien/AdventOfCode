namespace AdventOfCode.Aoc2015;

public class Day1 : IDay
{
    public void Run1(string[] data)
    {
        var floor = data[0].Select(x => x is '(' ? 1 : -1).Sum();
        Console.WriteLine($"Floor is {floor}");
    }

    public void Run2(string[] data)
    {
        int floor = 0, i;
        for (i = 0; i < data[0].Length; i++)
        {
            floor += data[0][i] == '(' ? 1 : -1;
            if (floor == -1) break;
        }

        Console.WriteLine($"Position: {i + 1}");
    }
}