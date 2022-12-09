namespace AdventOfCode.Aoc2022;

public class Day4 : IDay
{
    public void Run1(string[] data)
    {
        var overlaps = data
            .Select(x => x.Split(',')
                .SelectMany(x => x.Split('-'))
                .Select(int.Parse).Chunk(2))
            .Select(x => x.OrderBy(x => x.First()).ThenByDescending(x => x.Last()).ToArray())
            .Count(x => x[0].First() <= x[1].First() && x[1].Last() <= x[0].Last());

        Console.WriteLine($"{overlaps} full overlaps found");
    }

    public void Run2(string[] data)
    {
        var overlaps = data
            .Select(x => x.Split(',')
                .SelectMany(x => x.Split('-'))
                .Select(int.Parse).Chunk(2))
            .Select(x => x.OrderBy(x => x.First()).ToArray())
            .Count(x => x[0].Last() >= x[1].First());

        Console.WriteLine($"{overlaps} overlaps found");
    }
}