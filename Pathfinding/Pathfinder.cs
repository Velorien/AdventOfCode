namespace AdventOfCode.Pathfinding;

public static class Pathfinder
{
    public delegate int CostCalculator(char c, int x, int y);

    public delegate int DistanceCalculator(int cost, int distance);

    public delegate T NodeFactory<out T>(char c, int x, int y);

    public static TNode[,] BuildNodes<TNode>(string[] data, CostCalculator getCost) where TNode : Node<TNode>, new()
    {
        var nodes = new TNode[data.Length, data[0].Length];
        nodes.Iterate((x, y, _) => nodes[x, y] = new TNode
        {
            Cost = getCost(data[x][y], x, y),
            X = x, Y = y
        });

        return nodes;
    }

    public static void Traverse<TNode>(TNode[,] nodes, TNode start, DistanceCalculator getDistance)
        where TNode : Node<TNode>
    {
        start.Distance = 0;
        var queue = new PriorityQueue<TNode, int>();
        queue.Enqueue(start, 0);
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbour in current.GetNeighbours(nodes))
            {
                var distance = getDistance(neighbour.Cost, current.Distance);
                if (distance < neighbour.Distance)
                {
                    neighbour.Distance = distance;
                    queue.Enqueue(neighbour, distance);
                }
            }
        }
    }
}