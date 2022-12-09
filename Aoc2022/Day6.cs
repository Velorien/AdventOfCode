namespace AdventOfCode.Aoc2022;

public class Day6 : IDay
{
    public void Run1(string[] data) => Run(data[0], 4);

    public void Run2(string[] data) => Run(data[0], 14);

    private static void Run(string data, int length)
    {
        for (int i = 0; i < data.Length - length; i++)
        {
            var span = data.AsSpan(i, length);
            if (new HashSet<char>(span.ToArray()).Count == length)
            {
                Console.WriteLine($"Found sequence starting at {i + length}");
                return;
            }
        }
    }
}