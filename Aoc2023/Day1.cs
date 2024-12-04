using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;

internal class Day1 : IDay
{
    public void Run1(string[] data)
    {
        int sum = 0;
        foreach (string s in data)
        {
            int? first = null, last = null;

            for (int i = 0; i < s.Length; i++)
            {
                if (first is null) TrySetValue(ref first, s[i]);

                if (last is null) TrySetValue(ref last, s[^(i + 1)]);

                if (first.HasValue && last.HasValue) break;
            }

            sum += first!.Value * 10 + last!.Value;
        }

        Console.WriteLine($"The sum is {sum}");

        void TrySetValue(ref int? target, char c)
        {
            if (c is >= '0' and <= '9')
            {
                target = c - '0';
            }
        }
    }

    public void Run2(string[] data)
    {
        int sum = 0;
        List<string> validSubstrings = [
            "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
            .. Enumerable.Range(0, 10).Select(x => x.ToString())];

        foreach (string s in data)
        {
            var results = validSubstrings.Select(x => (c: x, f: s.IndexOf(x), l: s.LastIndexOf(x))).ToList();
            var first = results.Where(x => x.f >= 0).MinBy(x => x.f);
            var last = results.MaxBy(x => x.l);

            sum += ToNumber(first.c) * 10 + ToNumber(last.c);
        }

        Console.WriteLine($"The sum is {sum}");

        int ToNumber(string s) => s.Length == 1 ? s[0] - '0' : validSubstrings.IndexOf(s) + 1;
    }
}
