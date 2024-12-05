namespace AdventOfCode.Aoc2024;

public class Day5 : IDay
{
    public void Run1(string[] data)
    {
        var (order, inputs) = ParseInput(data);
        int sum = 0;
        
        foreach (var input in inputs)
        {
            if (InputIsInOrder(order, input))
            {
                sum += input[input.Length / 2];
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public void Run2(string[] data)
    {
        var (order, inputs) = ParseInput(data);
        var comparer = new PageComparer(order); 
        int sum = 0;
        
        foreach (var input in inputs)
        {
            if (!InputIsInOrder(order, input))
            {
                var sorted = input.OrderBy(x => x, comparer).ToArray();
                sum += sorted[sorted.Length / 2];
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }

    private (Dictionary<int, List<int>> order, List<int[]> inputs) ParseInput(string[] data)
    {
        var order = new Dictionary<int, List<int>>();
        var inputs = new List<int[]>();
        bool breakEncountered = false;
        
        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                breakEncountered = true;
                continue;
            }
            
            if (breakEncountered)
            {
                inputs.Add(line.Split(',').Select(int.Parse).ToArray());
            }
            else
            {
                var values = line.Split('|').Select(int.Parse).ToArray();
                if (!order.ContainsKey(values.First()))
                {
                    order.Add(values.First(), []);
                }
                
                order[values.First()].Add(values[1]);
            }
        }
        
        return (order, inputs);
    }

    private bool InputIsInOrder(Dictionary<int, List<int>> order, int[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (order.TryGetValue(input[i], out var orderForPage))
            {
                for (int j = i + 1; j < input.Length; j++)
                {
                    if (!orderForPage.Contains(input[j]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return i == input.Length - 1;
            }
        }
        
        return true;
    }

    private class PageComparer(Dictionary<int, List<int>> order) : IComparer<int>
    {
        public int Compare(int x, int y) => (order.GetValueOrDefault(x), order.GetValueOrDefault(y)) switch
        {
            (null, null) => 0,
            (null, _) => 1,
            (_, null) => -1,
            _ => order[x].Contains(y) ? -1 : 1             
        };
    }
}