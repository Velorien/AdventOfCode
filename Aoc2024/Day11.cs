namespace AdventOfCode.Aoc2024;

public class Day11 : IDay
{
    private readonly Dictionary<ulong, (ulong, ulong?)> _cache = [];

    public void Run1(string[] data)
    {
        var input = data[0].Split(' ').Select(ulong.Parse).ToArray();
        var counts = input.ToDictionary(k => k, _ => (ulong)1);

        foreach (var stone in input)
        {
            GenerateStones(stone, 0);
        }

        ulong sum = 0;
        foreach (var count in counts.Values) sum += count;

        Console.WriteLine($"There are {sum} stones");

        void GenerateStones(ulong stone, int iteration)
        {
            if (iteration == 25) return;

            var (becomes, spawns) = Blink(stone);
            counts[stone]--;

            if (!counts.TryAdd(becomes, 1)) counts[becomes]++;
            GenerateStones(becomes, iteration + 1);

            if (spawns.HasValue)
            {
                if (!counts.TryAdd(spawns.Value, 1)) counts[spawns.Value]++;
                GenerateStones(spawns.Value, iteration + 1);
            }
        }
    }

    public void Run2(string[] data)
    {
        var stones = data[0].Split(' ').Select(ulong.Parse).ToDictionary(x => x, _ => (ulong)1);
        for (var i = 0; i < 75; i++)
        {
            var dict = new Dictionary<ulong, ulong>();
            foreach (var (stone, count) in stones)
            {
                var (becomes, spawns) = Blink(stone);
                SetOrAdd(dict, becomes, count);
                if (spawns.HasValue) SetOrAdd(dict, spawns.Value, count);
            }

            stones = dict;
        }

        Console.WriteLine($"There are {stones.Values.Sum(x => (decimal)x)} stones");

        void SetOrAdd(Dictionary<ulong, ulong> dict, ulong stone, ulong value)
        {
            if (!dict.TryAdd(stone, value)) dict[stone] += value;
        }
    }

    public void Run22(string[] data)
    {
        var input = data[0].Split(' ').Select(ulong.Parse).ToArray();
        ulong stoneCount = 0;

        var validStones = new HashSet<ulong>();
        foreach (var stone in input)
        {
            FindAllPossibleStones(stone);
        }

        var generatedIn25Blinks =
            validStones.ToDictionary(k => k, _ => validStones.ToDictionary(k => k, _ => (ulong)0));

        var generatedAt25 = validStones.ToDictionary(k => k, _ => (ulong)0);
        var generatedAt50 = validStones.ToDictionary(k => k, _ => (ulong)0);
        var generatedAt75 = validStones.ToDictionary(k => k, _ => (ulong)0);

        foreach (var stone in validStones)
        {
            Blink25(stone, stone, 1);
        }

        foreach (var stone in input)
        {
            foreach (var (s, c) in generatedIn25Blinks[stone])
            {
                generatedAt25[s] += c;
            }
        }

        foreach (var (stone, multiplier) in generatedAt25)
        {
            foreach (var (s, c) in generatedIn25Blinks[stone])
            {
                generatedAt50[s] += c * multiplier;
            }
        }

        foreach (var (stone, multiplier) in generatedAt50)
        {
            foreach (var (s, c) in generatedIn25Blinks[stone])
            {
                generatedAt75[s] += c * multiplier;
            }
        }

        foreach (var c in generatedAt75.Values)
        {
            stoneCount += c;
        }

        Console.WriteLine($"There are {stoneCount} stones");

        void FindAllPossibleStones(ulong value)
        {
            if (!validStones.Add(value)) return;

            var (becomes, spawns) = Blink(value);

            if (spawns.HasValue)
            {
                FindAllPossibleStones(spawns.Value);
            }

            FindAllPossibleStones(becomes);
        }

        void Blink25(ulong stone, ulong ancestor, int iteration)
        {
            var (becomes, spawns) = _cache[stone];
            if (iteration == 25)
            {
                var dict = generatedIn25Blinks[ancestor];
                dict[becomes]++;
                if (spawns.HasValue) dict[spawns.Value]++;
            }
            else
            {
                Blink25(becomes, ancestor, iteration + 1);
                if (spawns.HasValue) Blink25(spawns.Value, ancestor, iteration + 1);
            }
        }
    }

    (ulong becomes, ulong? spawns) Blink(ulong value)
    {
        if (_cache.TryGetValue(value, out var r)) return r;

        ulong becomes;
        ulong? spawns = null;

        var nod = value.GetNumberOfDigits();
        if (value == 0) becomes = 1;
        else if (nod % 2 == 0)
        {
            var div = (ulong)Math.Pow(10, nod / 2);
            becomes = value / div;
            spawns = value % div;
        }
        else becomes = value * 2024;

        _cache[value] = (becomes, spawns);
        return (becomes, spawns);
    }
}