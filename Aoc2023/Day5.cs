using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;
internal class Day5 : IDay
{
    public void Run1(string[] data)
    {
        var seeds = GetSeeds(data);
        var maps = GetMaps(data);

        maps.Aggregate((p, n) => { 
            p.SetNext(n);
            return n;
        });

        var firstMap = maps.First();
        Console.WriteLine($"Min location number is : {seeds.Select(firstMap.GetChainedValue).Min()}");
    }

    public void Run2(string[] data)
    {
        var seeds = GetSeedRanges(data);
        var maps = GetMaps(data);

        var map = maps.Aggregate((p, n) =>
        {
            p.Merge(n);
            return p;
        });

        var min = map.Ranges
            .Select(x => x.Source)
            .Where(x => seeds.Any((s) => s.start <= x && x < s.start + s.length))
            .Concat(seeds.Select(x => x.start))
            .Select(map.GetChainedValue)
            .Min();

        Console.WriteLine($"Min location number is : {min}");
    }

    IEnumerable<ulong> GetSeeds(string[] data) => data[0][7..].Split(' ').Select(ulong.Parse);

    IEnumerable<(ulong start, ulong length)> GetSeedRanges(string[] data) =>
        data[0][7..].Split(' ').Select(ulong.Parse).Chunk(2).Select(x => (x[0], x[1]));

    IReadOnlyCollection<Map> GetMaps(string[] data)
    {
        var maps = new List<Map>();
        var ranges = new List<MapRange>();

        for (int i = 2; i < data.Length; i++)
        {
            if (data[i].Contains('-'))
            {
                ranges.Clear();
            }
            else if (data[i].Length > 0)
            {
                var split = data[i].Split(' ').Select(ulong.Parse).ToList();
                ranges.Add(new(split[0], split[1], split[2]));
            }

            if (data[i].Length == 0 || i == data.Length - 1)
            {
                var map = new Map(ranges.ToList());
                maps.Add(map);
            }
        }

        return maps;
    }

    class Map
    {
        Map? _next;

        public Map(List<MapRange> ranges) => Ranges = ranges;

        public List<MapRange> Ranges { get; }

        public void SetNext(Map map) => _next = map;

        public ulong GetValue(ulong key)
        {
            var range = Ranges.FirstOrDefault(x => x.Source <= key && key < x.Source + x.Length);
            if (range == default) return key;

            return range.Destination - range.Source + key;
        }

        public ulong GetChainedValue(ulong key) => _next?.GetChainedValue(GetValue(key)) ?? GetValue(key);

        public void Merge(Map other)
        {
            var set = new SortedSet<ulong>();

            foreach (var range in Ranges)
            {
                set.Add(range.Source);
                set.Add(range.Source + range.Length);

                var otherSources = other.Ranges
                    .SelectMany(x => GetBorderValues(range, x))
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value);

                foreach (var source in otherSources) set.Add(source);
            }

            var mapRanges = new List<MapRange>();
            var storedNext = _next;
            SetNext(other);

            set.Aggregate((l, r) => {
                var length = r - l; // l is exclusive upper bound
                var destination = GetChainedValue(l);

                mapRanges.Add(new MapRange(destination, l, length));

                return r;
            });

            _next = storedNext;

            MapRange currentRange = mapRanges.First();
            Ranges.Clear();

            foreach (var range in mapRanges.Skip(1))
            {
                var newRange = currentRange.TryMerge(range);

                if (newRange is not null)
                {
                    currentRange = newRange;
                }
                else
                {
                    Ranges.Add(currentRange);
                    currentRange = range;
                }
            }

            if (currentRange != null) Ranges.Add(currentRange);


            static IEnumerable<ulong?> GetBorderValues(MapRange current, MapRange other)
            {
                yield return other.Source;
                yield return other.Source + other.Length;
                yield return current.DestinationToSource(other.Source);
                yield return current.DestinationToSource(other.Source + other.Length);
            }
        }
    }

    record MapRange(ulong Destination, ulong Source, ulong Length)
    {
        public ulong? DestinationToSource(ulong destination)
        {
            if (destination < Destination || destination > Destination + Length)
                return null;

            return Source + destination - Destination;
        }

        public MapRange? TryMerge(MapRange other)
        {
            if (Source + Length == other.Source &&
                Destination + Length == other.Destination)
            {
                return new MapRange(Destination, Source, Length + other.Length);
            }

            return null;
        }
    }
}
