using AdventOfCode.Pathfinding;

namespace AdventOfCode.Aoc2022;

using static AdventOfCode.Utils;
using static AdventOfCode.Pathfinding.Pathfinder;

public class Day12 : IDay
{
    public void Run1(string[] data)
    {
        int startX = 0, startY = 0, endX = 0, endY = 0;

        var nodes = BuildNodes<MountainNode>(data, (c, x, y) =>
        {
            switch (c)
            {
                case 'S':
                    startX = x;
                    startY = y;
                    return 0;
                case 'E':
                    endX = x;
                    endY = y;
                    return 'z' - 'a';
                default:
                    return c - 'a';
            }
        });
        
        Traverse(nodes, nodes[startX, startY], (cost, distance) => distance + 1);

        int d = nodes[endX, endY].Distance;

        Console.WriteLine($"Reached goal in {d} steps");
    }

    public void Run2(string[] data)
    {
        List<(int x, int y)> starts = new();
        int endX = 0, endY = 0;

        var nodes = BuildNodes<MountainNode>(data, (c, x, y) =>
        {
            switch (c)
            {
                case 'S':
                case 'a':
                    starts.Add((x, y));
                    return 0;
                case 'E':
                    endX = x;
                    endY = y;
                    return 'z' - 'a';
                default:
                    return c - 'a';
            }
        });

        var distances = new List<int>();
        foreach (var (x, y) in starts)
        {
            nodes.Iterate((_, _, node) => node.Distance = int.MaxValue);
            Traverse(nodes, nodes[x, y], (cost, distance) => distance + 1);
            distances.Add(nodes[endX, endY].Distance);
        }

        Console.WriteLine($"Reached goal in {distances.Min()} steps");
    }

    record MountainNode : Node<MountainNode>
    {
        public override IEnumerable<MountainNode> GetNeighbours(MountainNode[,] nodes)
        {
            foreach (var (dx, dy) in Neighbours)
            {
                var x = dx + X;
                var y = dy + Y;
                if (x < 0 || x >= nodes.GetLength(0) ||
                    y < 0 || y >= nodes.GetLength(1) ||
                    Cost - nodes[x, y].Cost < -1)
                {
                    continue;
                }

                yield return nodes[X + dx, Y + dy];
            }
        }
    }
}