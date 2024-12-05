namespace AdventOfCode.Aoc2022;

public class Day3 : IDay
{
    public void Run1(string[] data)
    {
        var priorities = GetPriorities();
        var sum = data
            .Select(x =>
            {
                var compartments = x.Chunk(x.Length / 2).ToList();
                var sharedItems = compartments[0].Intersect(compartments[1]);
                return sharedItems.Select(x => priorities[x]).Sum();
            })
            .Sum();

        Console.WriteLine($"Sum of priorities is {sum}");
    }

    public void Run2(string[] data)
    {
        var priorities = GetPriorities();
        var sum = data
            .Chunk(3)
            .Select(x =>
            {
                var sharedItems = x
                    .Select(x => x.ToCharArray())
                    .Aggregate((c, n) => c.Intersect(n).ToArray());

                return sharedItems.Select(x => priorities[x]).Sum();
            })
            .Sum();

        Console.WriteLine($"Sum of priorities is {sum}");
    }

    static Dictionary<char, int> GetPriorities()
    {
        var priorities = new Dictionary<char, int>();
        int priority = 1;

        for (int i = 'a'; i <= 'z'; i++)
        {
            priorities[(char)i] = priority++;
        }

        for (int i = 'A'; i <= 'Z'; i++)
        {
            priorities[(char)i] = priority++;
        }

        return priorities;
    }
}