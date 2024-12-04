using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;

internal class Day3 : IDay
{
    public void Run1(string[] data)
    {
        var symbolPositions = new List<(int x, int y)>();
        var numbers = FindNumbers(data, (x, y, _) => symbolPositions.Add((x, y)));

        int sum = 0;
        foreach (var n in numbers)
        {
            if (symbolPositions.Any(p =>
            {
                var minX = n.PositionX - 1;
                var maxX = n.PositionX + n.Length;
                var minY = n.PositionY - 1;
                var maxY = n.PositionY + 1;

                return p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY;
            }))
            {
                sum += n.Value;
            }
        }

        Console.WriteLine($"The sum is {sum}");
    }

    public void Run2(string[] data)
    {
        var gearPositions = new List<(int x, int y)>();
        var numbers = FindNumbers(data, (x, y, c) =>
        {
            if (c is '*') gearPositions.Add((x, y));
        });

        int sum = 0;
        foreach (var (x, y) in gearPositions)
        {
            var matchingNumbers = numbers.Where(n =>
            {
                var minX = n.PositionX - 1;
                var maxX = n.PositionX + n.Length;
                var minY = n.PositionY - 1;
                var maxY = n.PositionY + 1;

                return x >= minX && x <= maxX && y >= minY && y <= maxY;
            }).ToList();

            if (matchingNumbers.Count == 2)
            {
                sum += matchingNumbers[0].Value * matchingNumbers[1].Value;
            }
        }

        Console.WriteLine($"The sum is {sum}");
    }

    record Number(int Value, int Length, int PositionX, int PositionY);

    delegate void OnSymbol(int x, int y, char c);

    List<Number> FindNumbers(string[] data, OnSymbol onSymbol)
    {
        var numbers = new List<Number>();

        for (int y = 0; y < data.Length; y++)
        {
            var s = data[y];
            bool isNumber = false;
            int startX = 0;
            int currentNumber = 0;

            for (int x = 0; x < s.Length; x++)
            {
                var c = s[x];
                if (c is >= '0' and <= '9')
                {
                    if (isNumber)
                    {
                        currentNumber *= 10;
                        currentNumber += c - '0';
                    }
                    else
                    {
                        currentNumber = c - '0';
                        isNumber = true;
                        startX = x;
                    }
                }

                if (isNumber && (c is < '0' or > '9' || x == s.Length - 1))
                {
                    isNumber = false;
                    int lengthExtension = x == s.Length - 1 ? 1 : 0;
                    numbers.Add(new Number(currentNumber, x - startX + lengthExtension, startX, y));
                }

                if (c is not '.' and (< '0' or > '9'))
                {
                    onSymbol(x, y, c);
                }
            }

        }

        return numbers;
    }
}
