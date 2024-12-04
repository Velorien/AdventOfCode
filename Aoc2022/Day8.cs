using System.Diagnostics;

namespace AdventOfCode.Aoc2022;

using static AdventOfCode.Utils;

public class Day8 : IDay
{
    public void Run1(string[] data)
    {
        var forest = BuildForest(data);
        SetVisibility(forest);
        int visibleCount = 0;
        
        forest.Iterate((_, _, tree) =>
        {
            if (tree.Visible) visibleCount++;
        });

        Console.WriteLine($"Found {visibleCount} visible trees");
    }

    public void Run2(string[] data)
    {
        IEnumerable<Tree> GetTreeLine(Tree[,] forest, int startX, int startY, int dx, int dy)
        {
            int nextX = startX, nextY = startY;
            do
            {
                nextX += dx;
                nextY += dy;
                yield return forest[nextX, nextY];
            } while (!forest.IsEdge(nextX, nextY));
        }

        var directions = new List<(int dx, int dy)> { (-1, 0), (1, 0), (0, -1), (0, 1) };

        var forest = BuildForest(data);
        int maxScore = int.MinValue, px = 0, py = 0;
        forest.Iterate((x, y, tree) =>
        {
            if (forest.IsEdge(x, y)) return;

            int currentScore = 1;
            foreach (var (dx, dy) in directions)
            {
                int currentDirection = 0;
                foreach (var current in GetTreeLine(forest, x, y, dx, dy))
                {
                    if (tree.Height == current.Height)
                    {
                        currentDirection++;
                        break;
                    }
                    if (tree.Height < current.Height) break;
                    currentDirection++;
                }
                
                if (currentDirection == 0) return;
                currentScore *= currentDirection;
            }

            if (currentScore > maxScore)
            {
                maxScore = currentScore;
                px = x;
                py = y;
            }
        });

        Console.WriteLine($"Max scenic score is {maxScore} at ({px}, {py})");
    }

    Tree[,] BuildForest(string[] data)
    {
        var forest = new Tree[data.Length, data[0].Length];
        forest.Iterate((x, y, _) =>
        {
            forest[x, y] = new Tree(data[x][y]);
        });

        return forest;
    }

    void SetVisibility(Tree[,] forest)
    {
        for (int x = 0; x < forest.GetLength(0); x++)
        {
            int maxHeight = int.MinValue;
            for (int y = 0; y < forest.GetLength(1); y++)
            {
                var tree = forest[x, y];
                if (tree.Height > maxHeight)
                {
                    tree.Visible = true;
                    maxHeight = tree.Height;
                }
            }

            maxHeight = int.MinValue;
            for (int y = forest.GetLength(1) - 1; y >= 0; y--)
            {
                var tree = forest[x, y];
                if (tree.Height > maxHeight)
                {
                    tree.Visible = true;
                    maxHeight = forest[x, y].Height;
                }
            }
        }
        
        for (int y = 0; y < forest.GetLength(1); y++)
        {
            int maxHeight = int.MinValue;
            for (int x = 0; x < forest.GetLength(0); x++)
            {
                var tree = forest[x, y];
                if (tree.Height > maxHeight)
                {
                    tree.Visible = true;
                    maxHeight = forest[x, y].Height;
                }
            }

            maxHeight = int.MinValue;
            for (int x = forest.GetLength(0) - 1; x >= 0; x--)
            {
                var tree = forest[x, y];
                if (tree.Height > maxHeight)
                {
                    tree.Visible = true;
                    maxHeight = forest[x, y].Height;
                }
            }
        }
    }

    class Tree
    {
        public Tree(char height)
        {
            Height = height - '0';
        }

        public int Height { get; }
        public bool Visible { get; set; }
    }
}