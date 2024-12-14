using System.Diagnostics;

namespace AdventOfCode.Aoc2021;

using PairCount = Dictionary<(char, char), long>;

public class Day14 : IDay
{
    public void Run1(string[] data) => Run(data, 10);

    public void Run2(string[] data) => Run(data, 40);

    private static void Run(string[] data, int runs)
    {
        var rules = data.Skip(2)
            .ToDictionary(x => (x[0], x[1]), x => x[6]);

        var pairs = new PairCount();
        var polymer = data[0];
        for (int i = 0; i < polymer.Length - 1; i++)
        {
            var pair = (polymer[i], polymer[i + 1]);
            if (!pairs.ContainsKey(pair)) pairs[pair] = 0;
            pairs[pair] += 1;
        }

        var sw = new Stopwatch();
        sw.Start();
        var results = Calculate(rules, pairs, runs);
        sw.Stop();

        var counts = results.Keys.Select(x => x.Item1).Distinct()
            .ToDictionary(k => k, _ => 0L);
        foreach (var item in results)
        {
            counts[item.Key.Item1] += item.Value;
            counts[item.Key.Item2] += item.Value;
        }

        counts[polymer.First()]++;
        counts[polymer.Last()]++;

        var result = counts.MaxBy(x => x.Value).Value - counts.MinBy(x => x.Value).Value;
        Console.WriteLine($"Result is {result / 2} in {sw.ElapsedMilliseconds}");
    }

    static PairCount Calculate(Dictionary<(char, char), char> rules, PairCount pairs, int repeat)
    {
        if (repeat == 0) return pairs;

        var newPairs = new PairCount();
        foreach (var pair in pairs)
        {
            var first = (pair.Key.Item1, rules[pair.Key]);
            var second = (rules[pair.Key], pair.Key.Item2);

            if (!newPairs.ContainsKey(first)) newPairs.Add(first, 0);
            if (!newPairs.ContainsKey(second)) newPairs.Add(second, 0);

            newPairs[first] += pair.Value;
            newPairs[second] += pair.Value;
        }

        return Calculate(rules, newPairs, repeat - 1);
    }
}