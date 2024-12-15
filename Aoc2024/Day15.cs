namespace AdventOfCode.Aoc2024;

using Point = (int x, int y);

public class Day15 : IDay
{
    public void Run1(string[] data)
    {
        var (map, directions) = ParseInput(data);
        int robotX = 0, robotY = 0, score = 0;
        map.Iterate((x, y, c) =>
        {
            if (c == '@')
            {
                robotX = x;
                robotY = y;
            }
        });

        foreach (var (dx, dy) in directions)
        {
            PushBoxes(dx, dy);
        }

        map.Iterate((x, y, c) =>
        {
            if (c == 'O') score += 100 * y + x;
        });

        Console.WriteLine($"Score is: {score}");

        void PushBoxes(int dx, int dy)
        {
            int cx = robotX + dx, cy = robotY + dy;
            bool canPush = false;
            while (map[cx, cy] != '#')
            {
                if (map[cx, cy] == '.')
                {
                    canPush = true;
                    break;
                }

                cx += dx;
                cy += dy;
            }

            if (!canPush) return;

            while ((cx, cy) != (robotX, robotY))
            {
                (map[cx, cy], map[cx - dx, cy - dy]) = (map[cx - dx, cy - dy], map[cx, cy]);
                cx -= dx;
                cy -= dy;
            }

            robotX += dx;
            robotY += dy;
        }
    }

    public void Run2(string[] data)
    {
        var (warehouse, (robotX, robotY), directions) = ParseInput2(data);
        foreach (var (dx, dy) in directions)
        {
            int nx = robotX + dx, ny = robotY + dy;
            var collidingObject = warehouse.FirstOrDefault(o => o.OccupiedTiles.Contains((nx, ny)));
            if (collidingObject is null)
            {
                robotX += dx;
                robotY += dy;
            }
            else if (collidingObject.CanBeMoved((dx, dy), warehouse))
            {
                collidingObject.Move((dx, dy), warehouse);
                robotX += dx;
                robotY += dy;
            }
        }

        Console.WriteLine($"Score is: {warehouse.Select(x => x.GetScore()).Sum()}");
    }

    private (char[,], Point[]) ParseInput(string[] data)
    {
        var divide = data.Index().First(x => string.IsNullOrWhiteSpace(x.Item)).Index;
        var map = data[..divide].To2DArray(x => x);
        var directions = data[(divide + 1)..].SelectMany(x => x.Select(ToDirection)).ToArray();

        return (map, directions);
    }


    private (IReadOnlyCollection<WarehouseObject> warehouse, Point robot, Point[] directions) ParseInput2(string[] data)
    {
        var divide = data.Index().First(x => string.IsNullOrWhiteSpace(x.Item)).Index;
        var directions = data[(divide + 1)..].SelectMany(x => x.Select(ToDirection)).ToArray();
        var walls = new List<Point>();
        var warehouse = new List<WarehouseObject> { new(true, walls) };
        int robotX = 0, robotY = 0;

        data[..divide].To2DArray(x => x).Iterate((x, y, c) =>
        {
            int x2 = 2 * x;
            if (c == '#')
            {
                walls.Add((x2, y));
                walls.Add((x2 + 1, y));
            }
            else if (c == '@')
            {
                robotX = x2;
                robotY = y;
            }
            else if (c == 'O')
            {
                warehouse.Add(new WarehouseObject(false, [(x2, y), (x2 + 1, y)]));
            }
        });

        return (warehouse, (robotX, robotY), directions);
    }

    private Point ToDirection(char c) => c switch
    {
        '^' => (0, -1),
        'v' => (0, 1),
        '<' => (-1, 0),
        '>' => (1, 0),
        _ => default,
    };

    record WarehouseObject(bool IsWall, List<Point> OccupiedTiles)
    {
        public bool CanBeMoved(Point direction, IReadOnlyCollection<WarehouseObject> objects)
        {
            if (IsWall) return false;

            var newOccupiedTiles = OccupiedTiles.Select(t => (t.x + direction.x, t.y + direction.y));
            var collidingObjects = objects.Where(o => o != this && o.OccupiedTiles.Intersect(newOccupiedTiles).Any());

            return collidingObjects.All(o => o.CanBeMoved(direction, objects));
        }

        public void Move(Point direction, IReadOnlyCollection<WarehouseObject> objects)
        {
            for (int i = 0; i < OccupiedTiles.Count; i++)
            {
                OccupiedTiles[i] = (OccupiedTiles[i].x + direction.x, OccupiedTiles[i].y + direction.y);
            }

            foreach (var o in objects.Where(o => o != this && o.OccupiedTiles.Intersect(OccupiedTiles).Any()))
            {
                o.Move(direction, objects);
            }
        }

        public int GetScore()
        {
            if (IsWall) return 0;
            var min = OccupiedTiles.MinBy(o => o.x * o.x + o.y * o.y);
            return min.y * 100 + min.x;
        }
    }
}