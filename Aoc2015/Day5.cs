using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2015;

public class Day5 : IDay
{
    private readonly char[] _vowels = ['a', 'e', 'i', 'o', 'u'];
    private readonly string[] _forbiddenSubstrings = ["ab", "cd", "pq", "xy"];

    public void Run1(string[] data)
    {
        Console.WriteLine($"There are {data.Count(IsNice1)} nice strings");
    }

    public void Run2(string[] data)
    {
        Console.WriteLine($"There are {data.Count(IsNice2)} nice strings");
    }

    bool IsNice1(string input) =>
        ContainsAtLeastThreeVowels(input) &&
        HasTwoSameLettersInARow(input) &&
        DoesNotContainForbiddenSubstrings(input);
    bool IsNice2(string input) =>
        Regex.IsMatch(input, @"\w*(\w)\w\1\w*") && Regex.IsMatch(input, @"\w*(\w\w)\w*\1\w*");

    private bool ContainsAtLeastThreeVowels(string input)
    {
        int vowelCount = 0;
        for (int i = 0; i < _vowels.Length; i++)
        {
            vowelCount += input.Count(x => x == _vowels[i]);
        }

        return vowelCount >= 3;
    }

    private bool HasTwoSameLettersInARow(string input)
    {
        bool hasTwoSameLettersInARow = false;
        input.Aggregate((p, n) =>
        {
            hasTwoSameLettersInARow |= p == n;
            return n;
        });

        return hasTwoSameLettersInARow;
    }

    private bool DoesNotContainForbiddenSubstrings(string input) =>
        _forbiddenSubstrings.All(x => !input.Contains(x));
}