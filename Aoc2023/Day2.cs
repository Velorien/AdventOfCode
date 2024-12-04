using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;
internal class Day2 : IDay
{
    public void Run1(string[] data)
    {
        int gameId = 0, sum = 0;
        Dictionary<char, int> cutoffValues = new()
        {
            ['r'] = 12,
            ['g'] = 13,
            ['b'] = 14,
        };

        foreach (var s in data)
        {
            gameId++;
            int currentNumber = 0;
            bool parsingNumber = false, parsingColor = false, shouldAddId = true;
            for (var i = s.IndexOf(':') + 2; i < s.Length; i++)
            {
                var c = s[i];
                if (c is ' ')
                {
                    parsingColor = parsingNumber = false;
                    continue;
                }

                if (c is >= '0' and <= '9')
                {
                    if (parsingNumber)
                    {
                        currentNumber *= 10;
                        currentNumber += c - '0';
                    }
                    else
                    {
                        parsingNumber = true;
                        currentNumber = c - '0';
                    }
                }

                if (!parsingColor && c is 'r' or 'g' or 'b')
                {
                    parsingColor = true;

                    if (currentNumber > cutoffValues[c])
                    {
                        shouldAddId = false;
                        break;
                    }

                    continue;
                }
            }

            if (shouldAddId) sum += gameId;
        }

        Console.WriteLine($"Sum is {sum}");
    }

    public void Run2(string[] data)
    {
        int sum = 0;
        foreach (var s in data)
        {
            Dictionary<char, int> minCounts = new()
            {
                ['r'] = 0,
                ['g'] = 0,
                ['b'] = 0,
            };

            int currentNumber = 0;
            bool parsingNumber = false, parsingColor = false;
            for (var i = s.IndexOf(':') + 2; i < s.Length; i++)
            {
                var c = s[i];
                if (c is ' ') parsingColor = parsingNumber = false;

                if (c is >= '0' and <= '9')
                {
                    if (parsingNumber)
                    {
                        currentNumber *= 10;
                        currentNumber += c - '0';
                    }
                    else
                    {
                        parsingNumber = true;
                        currentNumber = c - '0';
                    }
                }

                if (!parsingColor && c is 'r' or 'g' or 'b')
                {
                    parsingColor = true;

                    if (currentNumber > minCounts[c])
                        minCounts[c] = currentNumber;
                }

            }

            sum += minCounts['r'] * minCounts['g'] * minCounts['b'];
        }

        Console.WriteLine($"Sum is {sum}");
    }
}
