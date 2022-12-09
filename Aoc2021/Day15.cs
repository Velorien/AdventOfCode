using System.Diagnostics;
using static AdventOfCode.Utils;

namespace AdventOfCode.Aoc2021;

public class Day15 : IDay
{
    public void Run1(string[] data)
    {
        var nodes = BuildNodes(data);
        var end = nodes[nodes.GetLength(0) - 1, nodes.GetLength(1) - 1];
        var sw = new Stopwatch();
        sw.Start();
        Traverse(nodes);
        sw.Stop();
        Console.WriteLine($"Distance is {end.Distance} in {sw.ElapsedMilliseconds}");
    }

    public void Run2(string[] data)
    {
        var initialNodes = BuildNodes(data);
        var sizeX = initialNodes.GetLength(0);
        var sizeY = initialNodes.GetLength(1);
        var nodes = new Node[sizeX * 5, sizeY * 5];

        Iterate(initialNodes, (x, y, node) =>
        {
            for (int mulX = 0; mulX < 5; mulX++)
            {
                for (int mulY = 0; mulY < 5; mulY++)
                {
                    var currX = mulX * sizeX + x;
                    var currY = mulY * sizeY + y;
                    nodes[currX, currY]
                        = node with
                        {
                            X = currX,
                            Y = currY,
                            Cost = (node.Cost - 1 + mulX + mulY) % 9 + 1
                        };
                }
            }
        });

        var end = nodes[sizeX * 5 - 1, sizeY * 5 - 1];
        var sw = new Stopwatch();
        sw.Start();
        Traverse(nodes);
        sw.Stop();
        Console.WriteLine($"Distance is {end.Distance} in {sw.ElapsedMilliseconds}");
    }

    void Traverse(Node[,] nodes)
    {
        var start = nodes[0, 0];
        var queue = new PriorityQueue<Node, int>();
        queue.Enqueue(start, 0);

        while (queue.Count != 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbour in current.GetNeighbours(nodes))
            {
                var distance = neighbour.Cost + current.Distance;
                if (distance < neighbour.Distance)
                {
                    neighbour.Distance = distance;
                    queue.Enqueue(neighbour, distance);
                }
            }
        }
    }

    private static Node[,] BuildNodes(string[] data)
    {
        var nodes = new Node[data.Length, data[0].Length];
        Iterate(nodes, (x, y, _) => nodes[x, y] = new Node
        {
            Cost = data[x][y] - '0',
            X = x, Y = y
        });

        nodes[0, 0].Distance = 0;
        return nodes;
    }

    record Node : IComparable<Node>
    {
        private static readonly List<(int dx, int dy)> Neighbours;

        static Node()
        {
            var offsets = new[] { -1, 0, 1 };
            Neighbours = (
                from dx in offsets
                from dy in offsets
                where dx + dy is -1 or 1
                select (dx, dy)
            ).ToList();
        }

        public required int X { get; init; }
        public required int Y { get; init; }
        public required int Cost { get; init; }
        public int Distance { get; set; } = int.MaxValue;

        public IEnumerable<Node> GetNeighbours(Node[,] nodes)
        {
            foreach (var (dx, dy) in Neighbours)
            {
                var x = dx + X;
                var y = dy + Y;
                if (x < 0 || x >= nodes.GetLength(0) ||
                    y < 0 || y >= nodes.GetLength(1))
                {
                    continue;
                }
                
                yield return nodes[X + dx, Y + dy];
            }
        }

        public int CompareTo(Node? other) => Distance.CompareTo(other.Distance);

        public override int GetHashCode() => HashCode.Combine(X, Y, Cost);
    }
}