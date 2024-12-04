using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;
internal class Day4 : IDay
{
    public void Run1(string[] data)
    {
        var sum = 0;

        foreach (var s in data)
        {
            var cardData = s.Split(':', '|');
            var winning = cardData[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            var current = cardData[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();

            winning.IntersectWith(current);
            var count = winning.Count;
            if (count > 0)
            {
                sum += 1 << (count - 1);
            }
        }

        Console.WriteLine($"The sum is {sum}");
    }

    public void Run2(string[] data)
    {
        var multipliers = Enumerable.Range(0, data.Length).ToDictionary(x => x, _ => 1);
        for (int i = 0; i < data.Length; i++)
        {
            var s = data[i];
            var cardData = s.Split(':', '|');
            var winning = cardData[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            var current = cardData[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            winning.IntersectWith(current);

            foreach (var index in Enumerable.Range(i + 1, winning.Count))
            {
                multipliers[index] += multipliers[i];
            }
        }

        Console.WriteLine($"Total card count is {multipliers.Values.Sum()}");
    }
}
