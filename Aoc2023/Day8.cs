using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;

internal class Day8 : IDay
{
    public void Run1(string[] data)
    {
        var directions = data[0];

        var map = new Dictionary<string, (string l, string r)>();
        foreach (var item in data.Skip(2))
        {
            map[item[0..3]] = (item[7..10], item[12..15]);
        }

        var current = "AAA";
        int index = 0, steps = 0;
        do
        {
            current = GetNext(directions[index], map[current]);
            steps++;
            index++;
            index %= directions.Length;
        }
        while (current != "ZZZ");


        Console.WriteLine($"Travel took {steps} steps");
    }

    public void Run2(string[] data)
    {
        var directions = data[0];

        var map = new Dictionary<string, (string l, string r)>();
        foreach (var item in data.Skip(2))
        {
            map[item[0..3]] = (item[7..10], item[12..15]);
        }

        var cycles = map.Keys.Where(x => x.EndsWith('A')).Select(x => new CycleInfo(x)).Take(1).ToList();
        int index = 0;
        int steps = 1;
        bool shouldStop;
        do
        {
            shouldStop = true;
            foreach (var cycle in cycles)
            {
                if (!cycle.IsCycleComplete)
                {
                    cycle.Current = GetNext(directions[index], map[cycle.Current]);

                    if (cycle.Current.EndsWith("Z"))
                    {
                        Debugger.Break();
                    }
                }

                shouldStop &= cycle.IsCycleComplete;
            }

            steps++;
            index++;
            index %= directions.Length;
        }
        while (!shouldStop);

        ulong total = 1;
        foreach (var cycle in cycles) total *= cycle.CycleLength;

        Console.WriteLine($"Travel took {total} steps");
    }

    string GetNext(char dir, (string, string) next) =>
        dir == 'R' ? next.Item2 : next.Item1;

    class CycleInfo(string start)
    {
        public string Start { get; } = start;
        public string Current { get; set; } = start;
        public ulong CycleLength { get; set; }
        public bool IsCycleComplete => CycleLength != 0;
    }
}
