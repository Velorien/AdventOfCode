using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2024;

public class Day3 : IDay
{
    public void Run1(string[] data)
    {
        var regex = new Regex(@"mul\((?<first>\d{1,3}),(?<second>\d{1,3})\)");
        int sum = 0;
        foreach (var line in data)
        {
            var matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                sum += int.Parse(match.Groups["first"].Value) * int.Parse(match.Groups["second"].Value);
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public void Run2(string[] data)
    {
            var regex = new Regex(@"don't\(\)|do\(\)|mul\((?<first>\d{1,3}),(?<second>\d{1,3})\)");
        int sum = 0;
        bool isEnabled = true;
        foreach (var line in data)
        {
            var matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                if (match.Value == "don't()")
                {
                    isEnabled = false;
                }
                else if (match.Value == "do()")
                {
                    isEnabled = true;
                }
                else if (isEnabled)
                {
                    sum += int.Parse(match.Groups["first"].Value) * int.Parse(match.Groups["second"].Value);
                }
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }
}