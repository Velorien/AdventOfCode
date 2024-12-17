namespace AdventOfCode.Aoc2024;

using WallWithDirection = (int sideX, int sideY, Day12.GardenTile tile);

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

            List<HashSet<WallWithDirection>> walls = [];
            HashSet<GardenTile> currentArea = [];

            MapRegion(input, x, y, currentArea);

            foreach (var tile in currentArea)
            {
                foreach (var (sideX, sideY) in tile.GetWallSides())
                {
                    if (walls.Any(w => w.Contains((sideX, sideY, tile)))) continue;

                    HashSet<WallWithDirection> currentWall = [(sideX, sideY, tile)];

                    FindWallTiles(sideX, sideY, tile, currentWall, 1);
                    FindWallTiles(sideX, sideY, tile, currentWall, -1);

                    walls.Add(currentWall);
                }
            }

            totalCost += walls.Count * currentArea.Count;
        });

        Console.WriteLine($"Total cost: {totalCost}");

        void FindWallTiles(int sideX, int sideY, GardenTile tile, HashSet<WallWithDirection> currentWall, int direction)
        {
            int currentX = tile.X, currentY = tile.Y;
            while (true)
            {
                currentX += sideY * direction;
                currentY += sideX * direction;
                if (!input.ContainsPosition(currentX, currentY) ||
                    input[currentX, currentY].Value != tile.Value) break;

                if (!input.ContainsPosition(currentX + sideX, currentY + sideY) ||
                    input[currentX + sideX, currentY + sideY].Value != tile.Value)
                {
                    currentWall.Add((sideX, sideY, input[currentX, currentY]));
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

        foreach (var (nx, ny) in input.CardinalDirectionsWithinBounds(x, y))
        {
            var neighbour = input[nx, ny];
            if (current.Value == neighbour.Value) current.Neighbors.Add(neighbour);
            if (neighbour.IsVisited || current.Value != neighbour.Value) continue;
            MapRegion(input, nx, ny, visited);
        }
    }

    internal class GardenTile(char value, int x, int y)
    {
        public char Value { get; } = value;
        public int X { get; } = x;
        public int Y { get; } = y;
        public bool IsVisited { get; set; }
        public List<GardenTile> Neighbors { get; } = new(4);

        public IEnumerable<(int sideX, int sideY)> GetWallSides() => Neighbors.Count < 4
            ? Utils.CardinalDirections.Where(side => !Neighbors.Any(n => n.X == side.x + X && n.Y == side.y + Y))
            : [];
    }
}