namespace AdventOfCode.Pathfinding;

public record Node<TNode> where TNode : Node<TNode>
{
    protected static readonly List<(int dx, int dy)> Neighbours;

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

    public int X { get; init; }
    public int Y { get; init; }
    public int Cost { get; init; }
    public int Distance { get; set; } = int.MaxValue;

    public virtual IEnumerable<TNode> GetNeighbours(TNode[,] nodes)
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
}