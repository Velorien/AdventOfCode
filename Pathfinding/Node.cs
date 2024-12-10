namespace AdventOfCode.Pathfinding;

public record Node<TNode> where TNode : Node<TNode>
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Cost { get; init; }
    public int Distance { get; set; } = int.MaxValue;

    public virtual IEnumerable<TNode> GetNeighbours(TNode[,] nodes)
    {
        foreach (var (dx, dy) in Utils.AllNeighbours)
        {
            var x = dx + X;
            var y = dy + Y;
            if (!nodes.ContainsPosition(x, y))
            {
                continue;
            }

            yield return nodes[X + dx, Y + dy];
        }
    }
}