using System.Linq.Expressions;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2022;

public class Day11 : IDay
{
    public void Run1(string[] data)
    {
        var monkeys = ParseMonkeys<int>(data);
        Console.WriteLine($"Monkey business level is {CalculateMonkeyBusiness(monkeys, 20)}");
    }

    public void Run2(string[] data)
    {
        var monkeys = ParseMonkeys<Item>(data);
        var divisors = monkeys.Select(x => x.Divisor).ToList();
        foreach (var item in monkeys.SelectMany(x => x.Items))
        {
            item.InitializeModulo(divisors);
        }
        
        Console.WriteLine($"Monkey business level is {CalculateMonkeyBusiness(monkeys, 10_000)}");
    }

    ulong CalculateMonkeyBusiness<TItem>(List<Monkey<TItem>> monkeys, int rounds)
        where TItem : IParsable<TItem>, IDivisionOperators<TItem, int, TItem>, IModulusOperators<TItem, int, int>
    {
        for (int i = 0; i < rounds; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.ExecuteTurn();
            }
        }

        return monkeys
            .Select(x => x.ItemsInspected)
            .OrderDescending()
            .Take(2)
            .Aggregate((p, n) => p * n);
    }

    List<Monkey<TItem>> ParseMonkeys<TItem>(string[] data)
        where TItem : IParsable<TItem>, IDivisionOperators<TItem, int, TItem>, IModulusOperators<TItem, int, int>
    {
        var monkeys = new List<Monkey<TItem>>();
        var current = new Monkey<TItem> { AllMonkeys = monkeys };
        var numberRegex = new Regex(@"\d+");

        foreach (var line in data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                monkeys.Add(current);
                current = new Monkey<TItem> { AllMonkeys = monkeys };
            }
            else if (line.StartsWith("  Starting items"))
            {
                var items = line.Substring(18).Split(", ").Select(x => TItem.Parse(x, null));
                foreach (var item in items) current.Items.Enqueue(item);
            }
            else if (line.StartsWith("  Operation"))
            {
                var operation = line.Substring(19);
                current.Operation = BuildOperation<TItem>(operation.Split(' '));
            }
            else if (line.StartsWith("  Test"))
            {
                current.Divisor = int.Parse(numberRegex.Match(line).Value);
            }
            else if (line.StartsWith("    If true"))
            {
                current.TrueTarget = int.Parse(numberRegex.Match(line).Value);
            }
            else if (line.StartsWith("    If false"))
            {
                current.FalseTarget = int.Parse(numberRegex.Match(line).Value);
            }
        }

        monkeys.Add(current);
        return monkeys;
    }

    class Monkey<TItem>
        where TItem : IParsable<TItem>, IDivisionOperators<TItem, int, TItem>, IModulusOperators<TItem, int, int>
    {
        public List<Monkey<TItem>> AllMonkeys { get; init; }
        public Queue<TItem> Items { get; } = new();
        public Func<TItem, TItem> Operation { get; set; }
        public int Divisor { get; set; }
        public int TrueTarget { get; set; }
        public int FalseTarget { get; set; }
        public ulong ItemsInspected { get; private set; }

        public void ExecuteTurn()
        {
            while (Items.TryDequeue(out var current))
            {
                ItemsInspected++;
                current = Operation(current);
                current /= 3;

                if (current % Divisor == 0)
                    AllMonkeys[TrueTarget].Items.Enqueue(current);
                else
                    AllMonkeys[FalseTarget].Items.Enqueue(current);
            }
        }
    }

    class Item : IParsable<Item>
        , IAdditionOperators<Item, int, Item>, IDivisionOperators<Item, int, Item>, IModulusOperators<Item, int, int>,
        IMultiplyOperators<Item, int, Item>,
        IMultiplyOperators<Item, Item, Item>
    {
        private int _value;
        private readonly Dictionary<int, int> _moduloTable = new();

        public static Item Parse(string s, IFormatProvider? provider)
        {
            var value = int.Parse(s);
            return new Item { _value = value };
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out Item result) =>
            throw new NotImplementedException();

        public static Item operator +(Item left, int right)
        {
            var mt = left._moduloTable;
            foreach (var k in mt.Keys)
            {
                mt[k] = (mt[k] + right % k) % k;
            }
            
            return left;
        }

        public static Item operator /(Item left, int right) => left;

        public static int operator %(Item left, int right) =>
            left._moduloTable[right] == 0 ? 0 : 1;

        public static Item operator *(Item left, int right)
        {
            var mt = left._moduloTable;
            foreach (var k in mt.Keys)
            {
                mt[k] = mt[k] * right % k % k;
            }
            
            return left;
        }

        public static Item operator *(Item left, Item _)
        {
            var mt = left._moduloTable;
            foreach (var k in mt.Keys)
            {
                mt[k] = mt[k] * mt[k] % k;
            }

            return left;
        }

        public void InitializeModulo(List<int> divisors)
        {
            foreach (var divisor in divisors)
                _moduloTable[divisor] = _value % divisor;
        }
    }

    Func<TItem, TItem> BuildOperation<TItem>(string[] expr) where TItem : IParsable<TItem>
    {
        var parameter = Expression.Parameter(typeof(TItem), "old");
        var left = parameter;
        Expression right = expr[2] == "old"
            ? parameter
            : Expression.Constant(int.Parse(expr[2]));
        var op = expr[1] switch
        {
            "*" => Expression.Multiply(left, right),
            "+" => Expression.Add(left, right)
        };

        return Expression.Lambda<Func<TItem, TItem>>(op, parameter).Compile();
    }
}