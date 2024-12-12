namespace AdventOfCode.Aoc2024;

public class Day12 : IDay
{
    public void Run1(string[] data)
    {
        var input = data.To2DArray((c, x, y) => new GardenTile(c, x, y));

        int totalCost = 0;
        input.Iterate((x, y, t) =>
        {
            if (t.IsVisited) return;
            var currentArea = new HashSet<GardenTile>();
            MapRegion(input, x, y, currentArea);

            totalCost += currentArea.Sum(c => 4 - c.Neighbors.Count) * currentArea.Count;
        });

        Console.WriteLine($"Total cost: {totalCost}");
    }

    public void Run2(string[] data)
    {
        var input = data.To2DArray((c, x, y) => new GardenTile(c, x, y));

        int totalCost = 0;

        input.Iterate((x, y, t) =>
        {
            if (t.IsVisited) return;

            List<HashSet<(int sx, int sy, GardenTile tile)>> walls = [];
            var currentArea = new HashSet<GardenTile>();
            MapRegion(input, x, y, currentArea);

            foreach (var tile in currentArea)
            {
                foreach (var (sx, sy) in tile.GetWallSides())
                {
                    if (walls.Any(w => w.Contains((sx, sy, tile)))) continue;
                    var currentWall = new HashSet<(int sx, int sy, GardenTile tile)> { (sx, sy, tile) };
                    FindWallTiles(sx, sy, tile, currentWall, 1);
                    FindWallTiles(sx, sy, tile, currentWall, -1);
                    walls.Add(currentWall);
                }
            }

            totalCost += walls.Count * currentArea.Count;
        });

        Console.WriteLine($"Total cost: {totalCost}");

        void FindWallTiles(int sideX, int sideY, GardenTile tile,
            HashSet<(int sx, int sy, GardenTile tile)> currentWall, int direction)
        {
            int cx = tile.X, cy = tile.Y;
            while (true)
            {
                cx += sideY * direction;
                cy += sideX * direction;
                if (!input.ContainsPosition(cx, cy) ||
                    input[cx, cy].Value != tile.Value) break;

                if (!input.ContainsPosition(cx + sideX, cy + sideY) ||
                    input[cx + sideX, cy + sideY].Value != tile.Value)
                {
                    currentWall.Add((sideX, sideY, input[cx, cy]));
                }
                else break;
            }
        }
    }

    private void MapRegion(GardenTile[,] input, int x, int y, HashSet<GardenTile> visited)
    {
        var current = input[x, y];
        current.IsVisited = true;
        visited.Add(current);

        foreach (var (nx, ny) in input.AdjacentPositionsWithinBounds(x, y))
        {
            var neighbour = input[nx, ny];
            if (current.Value == neighbour.Value) current.Neighbors.Add(neighbour);
            if (neighbour.IsVisited || current.Value != neighbour.Value) continue;
            MapRegion(input, nx, ny, visited);
        }
    }

    class GardenTile(char value, int x, int y)
    {
        public char Value { get; } = value;
        public int X { get; } = x;
        public int Y { get; } = y;
        public bool IsVisited { get; set; }
        public List<GardenTile> Neighbors { get; } = new(4);

        public IEnumerable<(int x, int y)> GetWallSides()
        {
            if (Neighbors.Count == 4) yield break;

            foreach (var side in Utils.AdjacentNeighbours)
            {
                if (!Neighbors.Any(n => n.X == side.x + x && n.Y == side.y + y))
                {
                    yield return side;
                }
            }
        }
    }
}