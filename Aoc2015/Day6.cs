using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2015;

using Point = (int x, int y);

public class Day6 : IDay
{
    public void Run1(string[] data)
    {
        var lights = new bool[1000, 1000];
        data.Select(ParseInput).Iterate(i =>
        {
            for (int x = i.from.x; x <= i.to.x; x++)
            {
                for (int y = i.from.y; y <= i.to.y; y++)
                {
                    lights[x, y] = i.instruction switch
                    {
                        Instruction.TurnOn => true,
                        Instruction.TurnOff => false,
                        Instruction.Toggle => !lights[x, y],
                        _ => throw new UnreachableException()
                    };
                }
            }
        });

        int litCount = 0;
        lights.Iterate((_, _, lit) => litCount += lit ? 1 : 0);

        Console.WriteLine($"Lit lights: {litCount}");
    }

    public void Run2(string[] data)
    {
        var lights = new short[1000, 1000];
        data.Select(ParseInput).Iterate(i =>
        {
            for (int x = i.from.x; x <= i.to.x; x++)
            {
                for (int y = i.from.y; y <= i.to.y; y++)
                {
                    lights[x, y] += i.instruction switch
                    {
                        Instruction.TurnOn => 1,
                        Instruction.TurnOff => -1,
                        Instruction.Toggle => 2,
                        _ => throw new UnreachableException()
                    };

                    if (lights[x, y] < 0) lights[x, y] = 0;
                }
            }
        });

        int totalBrightness = 0;
        lights.Iterate((_, _, level) => totalBrightness += level);

        Console.WriteLine($"Total brightness: {totalBrightness}");
    }

    public (Instruction instruction, Point from, Point to) ParseInput(string input)
    {
        var match = Regex.Match(input, @"(turn off|turn on|toggle) (\d+),(\d+) through (\d+),(\d+)");
        var instruction = match.Groups[1].Value switch
        {
            "turn on" => Instruction.TurnOn,
            "turn off" => Instruction.TurnOff,
            "toggle" => Instruction.Toggle,
            _ => throw new UnreachableException(),
        };

        return (instruction, (Parse(2), Parse(3)), (Parse(4), Parse(5)));

        int Parse(int index) => int.Parse(match.Groups[index].Value);
    }

    public enum Instruction
    {
        TurnOn,
        TurnOff,
        Toggle
    }
}