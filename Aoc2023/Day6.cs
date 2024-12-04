using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;

internal class Day6 : IDay
{
    public void Run1(string[] data)
    {
        var results = data.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).Skip(1).ToList()).ToList();

        int total = 1;
        for (int i = 0; i < results[0].Count; i++)
        {
            int waysToBeatRecord = 0;
            ulong raceTime = results[0][i];
            ulong distanceToBeat = results[1][i];
            for (ulong t = 1; t < raceTime; t++)
            {
                if (t * (raceTime - t) > distanceToBeat)
                {
                    waysToBeatRecord++;
                }
            }

            total *= waysToBeatRecord;
        }

        Console.WriteLine($"The value is {total}");
    }

    public void Run2(string[] data)
    {
        var time = ulong.Parse(data[0].Replace(" ", "").Split(':')[1]);
        var distance = ulong.Parse(data[1].Replace(" ", "").Split(':')[1]);

        int waysToBeatRecord = 0;
        for (ulong t = 1; t < time; t++)
        {
            if (t * (time - t) > distance)
            {
                waysToBeatRecord++;
            }
        }

        Console.WriteLine($"There are {waysToBeatRecord} ways to beat the record");
    }
}
