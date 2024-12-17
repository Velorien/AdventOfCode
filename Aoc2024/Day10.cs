using AdventOfCode.Pathfinding;

namespace AdventOfCode.Aoc2024;

public class Day10 : IDay
{
    public void Run1(string[] data)
    {
        var map = data.To2DArray<TrailNode>((c, x, y) => new TrailNode(c - '0')
        {
            X = x, Y = y, Cost = 1
        });

        var trailheads = new List<TrailNode>();
        map.Iterate((_, _, tile) =>
        {
            if (tile.Value == 0) trailheads.Add(tile);
        });

        int score = 0;
        foreach (var trailhead in trailheads)
        {
            ZeroMap();
            Pathfinder.Traverse(map, trailhead, (_, c) => c.Distance + 1);
            map.Iterate((_, _, tile) =>
            {
                if (tile.Distance == 9) score++;
            });
        }

        Console.WriteLine($"Score: {score}");

        void ZeroMap() => map.Iterate((_, _, tile) => tile.Distance = int.MaxValue);
    }

    public void Run2(string[] data)
    {
        var map = data.To2DArray(x => x - '0');
        int rating = 0;

        map.Iterate((x, y, value) =>
        {
            if (value == 0) Traverse(x, y);
        });

        Console.WriteLine($"Rating: {rating}");

        void Traverse(int x, int y)
        {
            if (map[x, y] == 9)
            {
                rating++;
                return;
            }

            foreach (var n in Utils.CardinalDirections)
            {
                var nx = x + n.x;
                var ny = y + n.y;
                if (map.ContainsPosition(nx, ny) && map[nx, ny] - map[x, y] == 1)
                {
                    Traverse(nx, ny);
                }
            }
        }
    }

    record TrailNode(int Value) : Node<TrailNode>
    {
        public override IEnumerable<TrailNode> GetNeighbours(TrailNode[,] nodes)
        {
            foreach (var (dx, dy) in Utils.CardinalDirections)
            {
                var x = dx + X;
                var y = dy + Y;
                if (!nodes.ContainsPosition(x, y) || nodes[X + dx, Y + dy].Value - Value != 1)
                {
                    continue;
                }

                yield return nodes[X + dx, Y + dy];
            }
        }
    }
}