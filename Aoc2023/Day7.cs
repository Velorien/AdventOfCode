using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Aoc2023;
internal class Day7 : IDay
{

    public void Run1(string[] data)
    {
        var deck = "23456789TJQKA".ToList();

        var deals = data.Select(x =>
        {
            var split = x.Split(' ');
            return (hand: new Hand(split[0], deck), bid: int.Parse(split[1]));
        }).OrderBy(x => x.hand)
          .ToList();

        int rank = 1;
        int score = 0;
        foreach (var deal in deals)
        {
            score += deal.bid * rank;
            rank++;
        }

        Console.WriteLine($"Score is {score}");
    }

    public void Run2(string[] data)
    {
        var deck = "J23456789TQKA".ToList();

        var deals = data.Select(x =>
        {
            var split = x.Split(' ');
            return (hand: new Hand(split[0], deck, true), bid: int.Parse(split[1]));
        }).OrderBy(x => x.hand)
          .ToList();

        int rank = 1;
        int score = 0;
        foreach (var deal in deals)
        {
            score += deal.bid * rank;
            rank++;
        }

        Console.WriteLine($"Score is {score}");
    }

    class Hand(string cards, List<char> deck, bool handleJoker = false) : IComparable<Hand>
    {
        int _power;

        public string Cards => cards;

        public int CompareTo(Hand? other)
        {
            if (other is null) return 1;

            if (GetPower() != other.GetPower())
            {
                return GetPower().CompareTo(other.GetPower());
            }

            return GetNumericalValue().CompareTo(other.GetNumericalValue());
        }

        long _value = -1;
        private long GetNumericalValue()
        {
            if (_value >= 0) return _value;
            _value = 0;

            for (int i = 0; i < cards.Length; i++)
            {
                _value *= deck.Count;
                _value += deck.IndexOf(cards[i]);
            }

            return _value;
        }

        private int GetPower()
        {
            if (_power > 0) return _power;

            if (!handleJoker || !cards.Contains('J'))
            {
                var counts = cards.GroupBy(x => x).Select(x => x.Count()).OrderDescending().ToList();

                return _power = counts switch
                {
                    [5] => 7,
                    [4, ..] => 6,
                    [3, 2] => 5,
                    [3, ..] => 4,
                    [2, 2, 1] => 3,
                    [2, ..] => 2,
                    _ => 1
                };
            }
            else
            {
                var counts = cards.GroupBy(x => x)
                    .OrderByDescending(x => x.Count())
                    .ToDictionary(k => k.Key, v => v.Count());

                counts.Remove('J');

                if (counts.Count is 0 or 1) _power = 7; // 5 of a kind
                if (counts.Count == 2)
                {
                    if (counts.Last().Value == 1)
                        _power = 6; // 4 of a kind

                    if (counts.First().Value == counts.Last().Value)
                        _power = 5; // full house
                }
                if (counts.Count == 3) _power = 4; // 3 of a kind
                if (counts.Count == 4) _power = 2; // pair

                return _power;
            }
        }
    }
}
