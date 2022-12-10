namespace AdventOfCode.Aoc2022;

public class Day10 : IDay
{
    public void Run1(string[] data)
    {
        var sum = 0;
        ProcessCycles(data, (cycle, value) =>
        {
            if (cycle == 20 || (cycle - 20) % 40 == 0) sum += value * cycle;
        });

        Console.WriteLine($"Signal strength sum is {sum}");
    }

    public void Run2(string[] data)
    {
        var crt = new bool[240];
        ProcessCycles(data, (cycle, value) =>
        {
            bool draw = false;
            for (int i = -1; i <= 1; i++)
            {
                draw |= (cycle - 1) % 40 + i == value;
            }

            crt[cycle - 1] = draw;
        });

        for (int i = 0; i < crt.Length; i++)
        {
            if (crt[i]) Console.Write("#");
            else Console.Write(" ");
            if (i % 40 == 39) Console.WriteLine();
        }
    }

    void ProcessCycles(string[] data, Action<int, int> onCycle)
    {
        var index = 0;
        var value = 1;
        var cycle = 1;
        bool adding = false;
        while (index < data.Length)
        {
            onCycle(cycle, value);

            if (data[index] == "noop") index++;
            else if (adding)
            {
                value += int.Parse(data[index].Substring(5));
                adding = false;
                index++;
            }
            else if (data[index].StartsWith("addx")) adding = true;

            cycle++;
        }
    }
}