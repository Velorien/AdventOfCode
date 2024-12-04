namespace AdventOfCode.Aoc2024;

public class Day1 : IDay
{
    public void Run1(string[] data)
    {
        var (left, right) = GetLocationIds(data);
        int sum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            sum += Math.Abs(left[i] - right[i]);
        }

        Console.WriteLine($"Sum is {sum}");
    }

    public void Run2(string[] data)
    {
        int score = 0;
        var (left, right) = GetLocationIds(data);
        foreach (var id in left)
        {
            score += id * right.Count(x => x == id);
        }

        Console.WriteLine($"Score is {score}");
    }

    (int[], int[]) GetLocationIds(string[] data)
    {
        int[] left = new int[data.Length], right = new int[data.Length];
        data.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Aggregate(0, (i, ids) =>
            {
                left[i] = int.Parse(ids[0]);
                right[i] = int.Parse(ids[1]);
                return i + 1;
            });
        
        Array.Sort(left);
        Array.Sort(right);
        
        return (left, right);
    }
}