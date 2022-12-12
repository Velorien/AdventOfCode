using System.Diagnostics;
using AdventOfCode.Pathfinding;
using static AdventOfCode.Utils;
using static AdventOfCode.Pathfinding.Pathfinder;

namespace AdventOfCode.Aoc2021;

public class Day15 : IDay
{
    public void Run1(string[] data)
    {
        var nodes = BuildNodes<OceanNode>(data, (c, _, _) => c - '0');
        var end = nodes[nodes.GetLength(0) - 1, nodes.GetLength(1) - 1];
        var sw = new Stopwatch();
        sw.Start();
        Traverse(nodes, nodes[0, 0], (cost, distance) => cost + distance);
        sw.Stop();
        Console.WriteLine($"Distance is {end.Distance} in {sw.ElapsedMilliseconds}");
    }

    public void Run2(string[] data)
    {
        var initialNodes = BuildNodes<OceanNode>(data, (c, _, _) => c - '0');
        var sizeX = initialNodes.GetLength(0);
        var sizeY = initialNodes.GetLength(1);
        var nodes = new OceanNode[sizeX * 5, sizeY * 5];

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
        Traverse(nodes, nodes[0, 0], (cost, distance) => cost + distance);
        sw.Stop();
        Console.WriteLine($"Distance is {end.Distance} in {sw.ElapsedMilliseconds}");
    }

    record OceanNode : Node<OceanNode>;
}