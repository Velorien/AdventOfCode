namespace AdventOfCode.Aoc2024;

public class Day16 : IDay
{
    private MazeNode[,] _map = null!;
    private int _startX, _startY, _endX, _endY;

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

        Console.WriteLine($"Cost is {_map[_endX, _endY].Cost.Values.Min()}");
    }

    public void Run2(string[] data)
    {
        Run1(data);
        var seats = Backtrack();
        Console.WriteLine($"There are {seats} good seats");
    }

    private void Traverse()
    {
        _map[_startX, _startY].Cost[(1, 0)] = 0;
        var queue = new Queue<(MazeNode node, int x, int y)>();
        queue.Enqueue((_map[_startX, _startY], 1, 0));

        while (queue.TryDequeue(out var current))
        {
            var (node, dx, dy) = current;
            if (node.Value == 'E') continue;

            foreach (var (nx, ny) in Utils.CardinalDirections)
            {
                if ((nx + dx, ny + dy) == (0, 0)) continue;

                var cost = nx == dx && ny == dy ? 1 : 1001;
                var next = _map[node.X + nx, node.Y + ny];

                if (next.Value == '#') continue;

                if (next.Cost[(nx, ny)] >= node.Cost[(dx, dy)] + cost)
                {
                    next.Cost[(nx, ny)] = cost + node.Cost[(dx, dy)];
                    queue.Enqueue((next, nx, ny));
                }
            }
        }
    }

    private int Backtrack()
    {
        var set = new HashSet<MazeNode>();
        var queue = new Queue<MazeNode>();

        queue.Enqueue(_map[_endX, _endY]);

        while (queue.TryDequeue(out var current))
        {
            set.Add(current);
            if (current.Value == 'S') continue;

            var isEnd = current.Value == 'E';
            if (isEnd)
            {
                var minCost = current.Cost.Values.Min();
                foreach (var ((dx, dy), _) in current.Cost.Where(c => c.Value == minCost))
                {
                    queue.Enqueue(_map[current.X - dx, current.Y - dy]);
                }
            }
            else
            {
                foreach (var (dx, dy) in current.BestTrackNeighbours())
                {
                    var next = _map[current.X - dx, current.Y - dy];
                    if (next.Value == '#') continue;
                    queue.Enqueue(_map[current.X - dx, current.Y - dy]);
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

        public Dictionary<(int, int), int> Cost { get; } =
            Utils.CardinalDirections.ToDictionary(k => k, _ => int.MaxValue);

        public IEnumerable<(int, int)> BestTrackNeighbours()
        {
            var min = Cost.MinBy(x => x.Value);
            //if (min.Value == int.MaxValue) return [];
            return Cost.Where(c => c.Value == min.Value || c.Value == min.Value + 1000).Select(c => c.Key);
        }
    }
}