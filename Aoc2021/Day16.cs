using System.Globalization;

namespace AdventOfCode.Aoc2021;

public class Day16 : IDay
{
    public void Run1(string[] data)
    {
        var message = string.Join("", data[0].Select(x =>
            Convert.ToString(int.Parse(x.ToString(), NumberStyles.HexNumber), 2).PadLeft(4, '0')));

        var packets = new List<Packet>();
        int index = 0;
        _ = ParsePackets(message.AsSpan(), ref index, 1, packets);

        Console.WriteLine($"Sum of versions is {packets.Sum(x => x.Version)}");
    }

    public void Run2(string[] data)
    {
        var message = string.Join("", data[0].Select(x =>
            Convert.ToString(int.Parse(x.ToString(), NumberStyles.HexNumber), 2).PadLeft(4, '0')));

        var packets = new List<Packet>();
        int index = 0;
        _ = ParsePackets(message.AsSpan(), ref index, 1, packets);

        Console.WriteLine($"Packet value is {packets[0].Value}");
    }

    List<Packet> ParsePackets(ReadOnlySpan<char> data, ref int index, int maxPackets, List<Packet> allPackets)
    {
        var currentList = new List<Packet>();
        while (index < data.Length && currentList.Count < maxPackets)
        {
            if (data.Length - index < 4) break;

            var p = new Packet();
            currentList.Add(p);
            allPackets.Add(p);

            ParseHeader(data, ref index, p);

            if (p.Type == 4) // is literal value
            {
                ParseLiteralValue(data, ref index, p);
            }
            else if (data[index++] == '0')
            {
                int bitCount = ParseNumber(data.Slice(index, 15));
                index += 15;
                int subIndex = 0;
                p.Subpackets.AddRange(ParsePackets(data.Slice(index, bitCount), ref subIndex, int.MaxValue,
                    allPackets));
                index += bitCount;
            }
            else
            {
                int packetCount = ParseNumber(data.Slice(index, 11));
                index += 11;
                p.Subpackets.AddRange(ParsePackets(data, ref index, packetCount, allPackets));
            }
        }

        return currentList;
    }

    int ParseNumber(ReadOnlySpan<char> data)
    {
        int number = 0;
        number += data[0] - '0';
        for (int i = 1; i < data.Length; i++)
        {
            number <<= 1;
            number += data[i] - '0';
        }

        return number;
    }

    void ParseHeader(ReadOnlySpan<char> data, ref int index, Packet p)
    {
        p.Version = ParseNumber(data.Slice(index, 3));
        index += 3;

        p.Type = ParseNumber(data.Slice(index, 3));
        index += 3;
    }

    void ParseLiteralValue(ReadOnlySpan<char> data, ref int index, Packet p)
    {
        ReadOnlySpan<char> currentValue;
        do
        {
            currentValue = data.Slice(index, 5);
            p.Value <<= 1;
            p.Value += currentValue[1] - '0';
            for (int i = 2; i < 5; i++)
            {
                p.Value <<= 1;
                p.Value += currentValue[i] - '0';
            }

            index += 5;
        } while (currentValue[0] == '1');
    }

    class Packet
    {
        private long _value;
        public int Version { get; set; }
        public int Type { get; set; }

        public long Value
        {
            get => Type switch
            {
                0 => Subpackets.Sum(x => x.Value),
                1 => Subpackets.Select(x => x.Value).Aggregate((p, n) => p * n),
                2 => Subpackets.Min(x => x.Value),
                3 => Subpackets.Max(x => x.Value),
                4 => _value,
                5 => Subpackets[0].Value > Subpackets[1].Value ? 1 : 0,
                6 => Subpackets[0].Value < Subpackets[1].Value ? 1 : 0,
                7 => Subpackets[0].Value == Subpackets[1].Value ? 1 : 0,
                _ => 0
            };
            set => _value = value;
        }

        public List<Packet> Subpackets { get; } = new();
    }
}