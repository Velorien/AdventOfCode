using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2022;

public class Day5 : IDay
{
    public void Run1(string[] data)
    {
        var (stacks, index) = GetStacks(data);

        var regex = new Regex(@"move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)");

        for (int i = index; i < data.Length; i++)
        {
            var matches = regex.Match(data[i]);
            int count = int.Parse(matches.Groups["count"].Value);
            int from = int.Parse(matches.Groups["from"].Value) - 1;
            int to = int.Parse(matches.Groups["to"].Value) - 1;

            for (int j = 0; j < count; j++)
            {
                var toMove = stacks[from].Pop();
                stacks[to].Push(toMove);
            }
        }
        
        Console.WriteLine($"The top containers are {new string(stacks.Select(x => x.Peek()).ToArray())}");
    }

    public void Run2(string[] data)
    {
        var (stacks, index) = GetStacks(data);

        var regex = new Regex(@"move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)");

        for (int i = index; i < data.Length; i++)
        {
            var matches = regex.Match(data[i]);
            int count = int.Parse(matches.Groups["count"].Value);
            int from = int.Parse(matches.Groups["from"].Value) - 1;
            int to = int.Parse(matches.Groups["to"].Value) - 1;

            var tempStack = new Stack<char>();
            for (int j = 0; j < count; j++)
            {
                var toMove = stacks[from].Pop();
                tempStack.Push(toMove);
            }

            while (tempStack.TryPop(out var current))
            {
                stacks[to].Push(current);
            }
        }
        
        Console.WriteLine($"The top containers are {new string(stacks.Select(x => x.Peek()).ToArray())}");
    }

    (List<Stack<char>> stacks, int index) GetStacks(string[] data)
    {
        List<Stack<char>> stacks = new List<Stack<char>>();

        int i = 0;
        for (; i < data.Length; i++)
            if (data[i] == string.Empty)
                break;
        var startIndex = i + 1;
        i--;

        var stacksCount = int.Parse(data[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
        for (int j = 0; j < stacksCount; j++)
            stacks.Add(new Stack<char>());

        i--;

        for (; i >= 0; i--)
        {
            var chunks = data[i].Chunk(4);
            int index = 0;
            foreach (var chunk in chunks)
            {
                if (chunk[1] != ' ')
                {
                    stacks[index].Push(chunk[1]);
                }

                index++;
            }
        }

        return (stacks, startIndex);
    }
}