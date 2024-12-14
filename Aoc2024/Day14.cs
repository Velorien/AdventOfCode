namespace AdventOfCode.Aoc2024;

public class Day14 : IDay
{
    private const int SizeX = 101;
    private const int SizeY = 103;

    public void Run1(string[] data)
    {
        var robots = data.Select(ParseInput).ToArray();
        foreach (var r in robots)
        {
            r.Move(100);
        }

        int safetyFactor = 1;
        foreach (var g in robots.Where(x => x.Quadrant != -1).GroupBy(x => x.Quadrant))
        {
            safetyFactor *= g.Count();
        }

        Console.WriteLine($"Safety Factor: {safetyFactor}");
    }

    public void Run2(string[] data)
    {
        var robots = data.Select(ParseInput).ToArray();
        var map = new int[SizeX, SizeY];

        for (int i = 0; i < 10000; i++)
        {
            map.Iterate((x, y, _) => map[x, y] = 0);
            robots.Iterate(r =>
            {
                r.Move(1);
                map[r.X, r.Y]++;
            });

            var byX = robots.CountBy(r => r.X);
            var byY = robots.CountBy(r => r.Y);

            if (byX.Any(r => r.Value >= 33) && byY.Any(r => r.Value >= 31))
            {
                Console.WriteLine($"{i + 1} s elapsed:");
                map.Print(c => c == 0 ? "." : c.ToString());
                Console.WriteLine();
                return;
            }
        }
    }

    private Robot ParseInput(string input)
    {
        var data = input.Split(' ');
        var pos = data[0][2..].Split(',').Select(int.Parse).ToArray();
        var vel = data[1][2..].Split(',').Select(int.Parse).ToArray();
        return new Robot { X = pos[0], Y = pos[1], VelocityX = vel[0], VelocityY = vel[1] };
    }

    class Robot
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int VelocityX { get; init; }
        public int VelocityY { get; init; }

        public void Move(int times)
        {
            X += VelocityX * times;
            Y += VelocityY * times;
            X %= SizeX;
            if (X < 0) X = SizeX + X;
            Y %= SizeY;
            if (Y < 0) Y = SizeY + Y;
        }

        public int Quadrant => (X, Y) switch
        {
            (SizeX / 2, _) or (_, SizeY / 2) => -1,
            (< SizeX / 2, < SizeY / 2) => 0,
            (> SizeX / 2, < SizeY / 2) => 1,
            (< SizeX / 2, > SizeY / 2) => 2,
            (> SizeX / 2, > SizeY / 2) => 3,
        };
    }
}