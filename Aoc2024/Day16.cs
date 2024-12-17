namespace AdventOfCode.Aoc2024;

using Dir = (int x, int y);

public class Day16 : IDay
{
    private static MazeNode[,] _map = null!;
    private int _startX, _startY, _endX, _endY, _minimalCost;

    public void Run1(string[] data)
    {
        _map = data.To2DArray((c, x, y) =>
        {
            if (c == 'S')
            {
                _startX = x;
                _startY = y;
            }
            else if (c == 'E')
            {
                _endX = x;
                _endY = y;
            }

            return new MazeNode(c, x, y);
        });

        Traverse();

        _minimalCost = Utils.CardinalDirections.Select(d => _map[_endX, _endY].NeighbourOpposite(d).CostOfLeaving[d])
            .Min();
        Console.WriteLine($"Cost is {_minimalCost}");
    }

    public void Run2(string[] data)
    {
        Run1(data);
        var seats = Backtrack();
        Console.WriteLine($"There are {seats} good seats");
    }

    private void Traverse()
    {
        var queue = new Queue<(MazeNode node, Dir d)>();
        var start = _map[_startX, _startY];
        foreach (var d in Utils.CardinalDirections)
        {
            if (start.Neighbour(d).Value != '#')
            {
                start.CostOfLeaving[d] = d == (1, 0) ? 1 : 1001;
                queue.Enqueue((start.Neighbour(d), d));
            }
        }

        while (queue.TryDequeue(out var q))
        {
            var (current, d) = q;
            if (current.Value == 'E') continue;

            foreach (var n in Utils.CardinalDirections)
            {
                if ((n.x + d.x, n.y + d.y) == (0, 0)) continue;
                var prev = current.NeighbourOpposite(d);

                var cost = (n == d ? 1 : 1001) + prev.CostOfLeaving[d];

                var next = current.Neighbour(n);
                if (next.Value == '#') continue;

                if (current.CostOfLeaving[n] >= cost)
                {
                    current.CostOfLeaving[n] = cost;
                    queue.Enqueue((next, n));
                }
            }
        }
    }

    private int Backtrack()
    {
        var set = new HashSet<MazeNode>();
        var queue = new Queue<(MazeNode node, Dir d)>();

        var end = _map[_endX, _endY];
        foreach (var d in Utils.CardinalDirections)
        {
            if (end.NeighbourOpposite(d).CostOfLeaving[d] == _minimalCost)
            {
                queue.Enqueue((end.NeighbourOpposite(d), d));
            }
        }

        set.Add(end);

        while (queue.TryDequeue(out var q))
        {
            var (current, dir) = q;

            if (!set.Add(current)) continue;

            foreach (var d in Utils.CardinalDirections)
            {
                var next = current.NeighbourOpposite(d);
                if (next.Value == '#') continue;

                var cost = dir == d ? 1 : 1001;

                if (next.CostOfLeaving[d] + cost == current.CostOfLeaving[dir])
                {
                    queue.Enqueue((next, d));
                }
            }
        }

        return set.Count;
    }

    class MazeNode(char value, int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public char Value { get; } = value;

        public MazeNode Neighbour((int x, int y) d) => _map[X + d.x, Y + d.y];
        public MazeNode NeighbourOpposite((int x, int y) d) => _map[X - d.x, Y - d.y];

        public Dictionary<(int, int), int> CostOfLeaving { get; } =
            Utils.CardinalDirections.ToDictionary(k => k, _ => int.MaxValue);
    }
}