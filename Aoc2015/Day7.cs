using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Aoc2015;

public class Day7 : IDay
{
    public void Run1(string[] data)
    {
        var gates = ParseInput(data);

        Console.WriteLine($"Value of a is: {gates["a"].GetValue(gates)}");
    }

    public void Run2(string[] data)
    {
        var gates = ParseInput(data);

        var a = gates["a"].GetValue(gates);
        foreach (var gate in gates.Values) gate.Reset();

        gates["b"] = new Gate(a.ToString(), "", "", true);

        Console.WriteLine($"Value of a is: {gates["a"].GetValue(gates)}");
    }

    private IDictionary<string, Gate> ParseInput(string[] data)
    {
        var dictionary = new Dictionary<string, Gate>();
        foreach (var line in data)
        {
            var match = Regex.Match(line,
                @"(?<left>\w+)?(?! \w+ ->) ?(?<op>NOT|AND|OR|LSHIFT|RSHIFT)? ?(?<right>\w+)? -> (?<target>\w+)");
            var target = match.Groups["target"].Value;

            dictionary[target] = new Gate(Left(), Right(), Op(), int.TryParse(Left(), out _));

            string Right() => match.Groups["right"].Value;
            string Left() => match.Groups["left"].Value;
            string Op() => match.Groups["op"].Value;
        }

        return dictionary;
    }

    record Gate(string Left, string Right, string Op, bool LeftIsNumber)
    {
        private ushort? _value;

        public ushort GetValue(IDictionary<string, Gate> gates) => _value ??= Op switch
        {
            "" when LeftIsNumber => _value ??= ushort.Parse(Left),
            "" => gates[Left].GetValue(gates),
            "NOT" => (ushort)~gates[Right].GetValue(gates),
            "AND" when LeftIsNumber => (ushort)((_value ??= ushort.Parse(Left)) & gates[Right].GetValue(gates)),
            "AND" => (ushort)(gates[Left].GetValue(gates) & gates[Right].GetValue(gates)),
            "OR" when LeftIsNumber => (ushort)((_value ??= ushort.Parse(Left)) | gates[Right].GetValue(gates)),
            "OR" => (ushort)(gates[Left].GetValue(gates) | gates[Right].GetValue(gates)),
            "LSHIFT" => (ushort)(gates[Left].GetValue(gates) << (_value ??= ushort.Parse(Right))),
            "RSHIFT" => (ushort)(gates[Left].GetValue(gates) >> (_value ??= ushort.Parse(Right))),
            _ => throw new UnreachableException()
        };

        public void Reset() => _value = null;
    }
}