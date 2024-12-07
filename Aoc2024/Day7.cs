using Operation = System.Func<long, long, long>;

namespace AdventOfCode.Aoc2024;

public class Day7 : IDay
{
    private static readonly Operation Add = (x, y) => x + y;
    private static readonly Operation Mul = (x, y) => x * y;
    private static readonly Operation Con = (x, y) =>
    {
        var pow = Math.Floor(Math.Log10(y)) + 1;
        return x * (long)Math.Pow(10, pow) + y;
    };

    private static readonly Operation[] AddMul = [Add, Mul];
    private static readonly Operation[] AddMulCon = [Add, Mul, Con];

    public void Run1(string[] data)
    {
        var sum = ParseInput(data)
            .Select(x => (x.value, x.operands, operations: AddMul.Combinations(x.operands.Length - 1)))
            .Where(x => x.operations.Any(o => IsValid(x.value, x.operands, o)))
            .Sum(x => x.value);

        Console.WriteLine($"Sum: {sum}");
    }

    public void Run2(string[] data)
    {
        var sum = ParseInput(data)
            .Select(x => (x.value, x.operands, operations: AddMulCon.CombinationsFast(x.operands.Length - 1)))
            .Where(x => x.operations.Any(o => IsValid(x.value, x.operands, o)))
            .Sum(x => x.value);

        Console.WriteLine($"Sum: {sum}");
    }

    private IEnumerable<(long value, long[] operands)> ParseInput(string[] data) => data
        .Select(x => x.Split(':'))
        .Select(x => (long.Parse(x[0]),
            x[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()));

    private IEnumerable<IEnumerable<Operation>> GetOperationCombinations(int count)
    {
        var max = 1 << count;
        for (int i = 0; i <= max; i++)
        {
            yield return GetOperationsForValue(i, count);
        }
    }

    private IEnumerable<Operation> GetOperationsForValue(long value, int power)
    {
        for (int i = 0; i <= power; i++)
        {
            var position = 1 << i;
            yield return (value & position) == 0 ? Add : Mul;
        }
    }

    private bool IsValid(long value, long[] operands, IEnumerable<Operation> operations)
    {
        int index = 1;
        var current = operands[0];
        foreach (var operation in operations)
        {
            var next = operation(current, operands[index++]);
            if (current <= next)
            {
                current = next;
            }
            else return false;
        }

        return value == current;
    }
}