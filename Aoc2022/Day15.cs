using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2022;

public class Day15 : IDay
{
    public void Run1(string[] data)
    {
        int yPosition = 2000000;

        IEnumerable<int> GetCoveredXPositions(Sensor s, int y)
        {
            var d = Math.Abs(y - s.Y);
            if (d > s.Distance) yield break;
            d = s.Distance - d;
            for (int i = s.X - d; i <= s.X + d; i++)
            {
                if (s.BeaconY == y && s.BeaconX == i) continue;
                yield return i;
            }
        }

        HashSet<int> xPositions = new();
        var sensorData = GetSensors(data);

        foreach (var sensor in sensorData)
        {
            foreach (var x in GetCoveredXPositions(sensor, yPosition))
            {
                xPositions.Add(x);
            }
        }

        Console.WriteLine($"Beacon cannot be in {xPositions.Count} positions");
    }

    public void Run2(string[] data)
    {
        IEnumerable<(int x, int y)> GetNeighbors(Sensor s)
        {
            yield return (s.X, s.Y - s.Distance - 1);
            yield return (s.X, s.Y + s.Distance + 1);
            yield return (s.X - s.Distance - 1, s.Y);
            yield return (s.X + s.Distance + 1, s.Y);

            for (int i = 1; i <= s.Distance; i++)
            {
                yield return (s.X - s.Distance + i - 1, s.Y - i);
                yield return (s.X + s.Distance - i + 1, s.Y - i);

                yield return (s.X - s.Distance + i - 1, s.Y + i);
                yield return (s.X + s.Distance - i + 1, s.Y + i);
            }
        }

        var maxPos = 4_000_000;
        var sensorData = GetSensors(data);

        foreach (var sensor in sensorData)
        {
            foreach (var (x, y) in GetNeighbors(sensor))
            {
                if (x < 0 || x > maxPos || y < 0 || y > maxPos) continue;

                bool any = false;
                foreach (var s in sensorData)
                {
                    if (s == sensor) continue;
                    if (Math.Abs(s.X - x) + Math.Abs(s.Y - y) <= s.Distance)
                    {
                        any = true;
                        break;
                    }
                }
                
                if (any) continue;
                Console.WriteLine($"Tuning frequency is {x * (long)4000000 + y}");
                return;
            }
        }
    }

    List<Sensor> GetSensors(string[] data)
    {
        var regex = new Regex(@"-?\d+");
        return data.Select(x =>
        {
            var matches = regex.Matches(x).Select(m => int.Parse(m.Value)).ToArray();
            return new Sensor(matches[0], matches[1], matches[2], matches[3]);
        }).ToList();
    }

    record Sensor(int X, int Y, int BeaconX, int BeaconY)
    {
        public int Distance => Math.Abs(X - BeaconX) + Math.Abs(Y - BeaconY);
    }
}