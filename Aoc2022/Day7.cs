namespace AdventOfCode.Aoc2022;

public class Day7 : IDay
{
    public void Run1(string[] data)
    {
        var flatList = GetDirectories(data);
        Console.WriteLine($"sum of sub-100k directories is {flatList.Where(x => x.Size <= 100000).Sum(x =>x.Size)}");
    }

    public void Run2(string[] data)
    {
        var flatList = GetDirectories(data);
        var usedSize = flatList.First().Size;
        var freeSize = 70_000_000 - usedSize;
        var requiredSize = 30_000_000 - freeSize;
        Console.WriteLine($"Candidate for deletion size is {flatList.OrderBy(x => x.Size).First(x => x.Size - requiredSize > 0).Size}");
    }

    private List<Directory> GetDirectories(string[] data)
    {
        Directory current = new() { Name = "/"};
        List<Directory> flatList = new() { current };

        bool listing = false;
        foreach (var item in data)
        {
            if (item.StartsWith("$")) listing = false;

            if (item == "$ ls")
            {
                listing = true;
                continue;
            }

            if (item == "$ cd ..") current = current.Parent;
            else if (item == "$ cd /")
                current = flatList.First();
            else if (item.StartsWith("$ cd"))
                current = current.Directories.First(x => x.Name == item.Substring(5));

            if (listing)
            {
                if (item.StartsWith("dir "))
                {
                    var dir = new Directory { Name = item.Substring(4), Parent = current };
                    current.Directories.Add(dir);
                    flatList.Add(dir);
                }
                else
                {
                    var fileData = item.Split(" ");
                    current.Files.Add((int.Parse(fileData[0]), fileData[1]));
                }
            }
        }

        return flatList;
    }

    class Directory
    {
        public Directory? Parent { get; init; }
        public required string Name { get; init; }
        public long Size => Files.Sum(x => x.Size) + Directories.Sum(x => x.Size);
        public List<Directory> Directories { get; } = new();
        public List<(int Size, string Name)> Files { get; } = new();
    }
}