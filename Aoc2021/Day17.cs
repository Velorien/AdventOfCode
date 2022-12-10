using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2021;

public class Day17 : IDay
{
    public void Run1(string[] data)
    {
        var t = GetTarget(data);
        var maxHeight = 0;
        for (int i = 1; i < -t.y1; i++) maxHeight += i;
        Console.WriteLine($"Max height is {maxHeight}");
    }

    public void Run2(string[] data)
    {
        var t = GetTarget(data);
        var possibleX = new List<XVelocityData>();
        int xVelocity = 1;
        while (true)
        {
            var vData = GetXVelocityData(xVelocity, t.x1, t.x2);
            if (xVelocity > t.x2) break;
            if (vData.Hits.Any())
            {
                possibleX.Add(vData);
            }

            xVelocity++;
        }

        var possibleY = new List<YVelocityData>();
        for (int yVelocity = t.y1; yVelocity <= -t.y1; yVelocity++)
        {
            var vData = GetYVelocityData(yVelocity, t.y1, t.y2);
            if (vData.Hits.Any())
                possibleY.Add(vData);
        }

        var velocities =
            from vx in possibleX
            from vy in possibleY
            let xSteps = vx.Hits.Select(x => x.step)
            let ySteps = vy.Hits.Select(y => y.step).ToHashSet()
            where
                vx.StopsAt.HasValue && ySteps.Any(s => s >= vx.StopsAt) ||
                xSteps.Intersect(ySteps).Any()
            select (x: vx.Velocity, y: vy.Velocity);

        Console.WriteLine($"Possible velocities count is {velocities.Count()}");
    }

    XVelocityData GetXVelocityData(int velocity, int minX, int maxX)
    {
        int originalVelocity = velocity;
        var intersections = new List<(int, int)>();
        int position = 0;
        int step = 0;
        do
        {
            position += velocity--;
            step++;
            if (position >= minX && position <= maxX) intersections.Add((position, step));
        } while (velocity > 0 && position <= maxX);

        return new(intersections, originalVelocity, velocity == 0 ? step : null);
    }

    YVelocityData GetYVelocityData(int velocity, int minY, int maxY)
    {
        int originalVelocity = velocity;
        var intersections = new List<(int, int)>();
        int position = 0;
        int max = 0;
        int step = 0;

        while (true)
        {
            if (velocity > 0) max += velocity;

            position += velocity--;
            step++;

            if (position >= minY && position <= maxY)
                intersections.Add((position, step));

            if (position < minY) break;
        }

        return new(intersections, originalVelocity, max);
    }

    (int x1, int x2, int y1, int y2) GetTarget(string[] data)
    {
        var regex = new Regex(@"(-?\d+)");
        var p = regex.Matches(data[0]).Select(x => int.Parse(x.Value)).ToArray();
        return (p[0], p[1], p[2], p[3]);
    }

    record XVelocityData(List<(int x, int step)> Hits, int Velocity, int? StopsAt);

    record YVelocityData(List<(int y, int step)> Hits, int Velocity, int MaxAt);
}