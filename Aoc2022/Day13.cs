namespace AdventOfCode.Aoc2022;

public class Day13 : IDay
{
    public void Run1(string[] data)
    {
        int index = 1;
        int rightOrder = 0;
        foreach (var pair in data.Chunk(3))
        {
            var left = Packet.Parse(pair[0]);
            var right = Packet.Parse(pair[1]);
            if (left.CompareTo(right) == -1) rightOrder += index;
            index++;
        }

        Console.WriteLine($"Right order index sum is {rightOrder}");
    }

    public void Run2(string[] data)
    {
        Packet dp1 = Packet.Parse("[[2]]"), dp2 = Packet.Parse("[[6]]");

        var packets = new List<Packet> { dp1, dp2 };
        foreach (var item in data)
        {
            if (string.IsNullOrEmpty(item)) continue;
            packets.Add(Packet.Parse(item));
        }

        packets.Sort();
        var i1 = packets.IndexOf(dp1) + 1;
        var i2 = packets.IndexOf(dp2) + 1;
        Console.WriteLine($"Decoder key is {i1 * i2}");
    }

    class Packet : IParsable<Packet>, IComparable<Packet>
    {
        public bool IsArray => ListValue is not null;
        public bool IsValue => Value.HasValue;

        public int? Value { get; set; }
        public List<Packet>? ListValue { get; set; }

        public static Packet Parse(string s, IFormatProvider? provider = null) => Parse(s.AsSpan()[1..^1]);

        private static Packet Parse(ReadOnlySpan<char> s)
        {
            var p = new Packet { ListValue = new() };
            if (s.Length == 0) return p;
            int brackets = 0;
            int currentValue = -1;
            int bracketStart = -1;

            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '[':
                        if (bracketStart == -1) bracketStart = i;
                        brackets++;
                        break;
                    case ']':
                        brackets--;
                        if (bracketStart != -1 && brackets == 0)
                        {
                            p.ListValue.Add(Parse(s[(bracketStart + 1)..i]));
                            bracketStart = -1;
                        }

                        break;
                    case ',':
                        if (brackets > 0) break;
                        if (currentValue != -1) p.ListValue.Add(new Packet { Value = currentValue });
                        currentValue = -1;
                        break;
                    default:
                        if (brackets > 0) break;
                        if (currentValue == -1) currentValue = 0;
                        else currentValue *= 10;
                        currentValue += s[i] - '0';
                        break;
                }
            }

            if (currentValue != -1) p.ListValue.Add(new Packet { Value = currentValue });

            return p;
        }

        public override string ToString()
        {
            if (IsValue) return Value.ToString();
            else return $"[{string.Join(',', ListValue)}]";
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out Packet result) =>
            throw new NotImplementedException();

        public int CompareTo(Packet? other)
        {
            if (other is null) throw new ArgumentNullException();

            if (IsValue && other.IsValue) return Value!.Value.CompareTo(other.Value!.Value);
            if (IsArray && other.IsArray)
            {
                foreach (var (l, r) in ListValue.Zip(other.ListValue))
                {
                    var comparison = l.CompareTo(r);
                    if (comparison != 0) return comparison;
                }

                return ListValue.Count.CompareTo(other.ListValue.Count);
            }


            var left = IsValue ? new Packet { ListValue = new() { this } } : this;
            var right = other.IsValue ? new Packet { ListValue = new() { other } } : other;

            return left.CompareTo(right);
        }
    }
}