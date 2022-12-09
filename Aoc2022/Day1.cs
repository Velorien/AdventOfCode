namespace AdventOfCode.Aoc2022;

public class Day1 : IDay
{
    public void Run1(string[] data)
    {
        List<int> sums = new();

        data.Aggregate(0, (p, c) =>
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                sums.Add(p);
                return 0;
            }
            
            return  p + int.Parse(c);
        });

        Console.WriteLine($"Max calories: {sums.Max()}");
    }
    
    public void Run2(string[] data)
    {
        List<int> sums = new();

        data.Aggregate(0, (p, c) =>
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                sums.Add(p);
                return 0;
            }
            
            return  p + int.Parse(c);
        });

        Console.WriteLine($"Max calories: {sums.OrderDescending().Take(3).Sum()}");
    }
}