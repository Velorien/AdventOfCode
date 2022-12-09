namespace AdventOfCode.Aoc2022;

public class Day2 : IDay
{
    public void Run1(string[] data)
    {
        var score = data
            .Select(x => GetScore(FromChar(x[0]), FromChar(x[2])))
            .Sum();

        Console.WriteLine($"Score is {score}");
    }

    public void Run2(string[] data)
    {
        var score = data
            .Select(x => GetScore(FromChar(x[0]), GetRPS(FromChar(x[0]), ResultFromChar(x[2]))))
            .Sum();

        Console.WriteLine($"Score is {score}");   
    }


    RPS FromChar(char input) => input switch
    {
        'A' or 'X' => RPS.Rock,
        'B' or 'Y' => RPS.Paper,
        'C' or 'Z' => RPS.Scissors
    };

    Result ResultFromChar(char input) => input switch
    {
        'X' => Result.Lose,
        'Y' => Result.Draw,
        'Z' => Result.Win
    };

    enum Result
    {
        Lose,
        Draw,
        Win
    }

    enum RPS
    {
        Rock,
        Paper,
        Scissors
    }

    int GetScore(RPS enemy, RPS you) =>
        (you, enemy) switch
        {
            (_, _) when you == enemy => 3,
            (RPS.Paper, RPS.Rock) => 6,
            (RPS.Rock, RPS.Scissors) => 6,
            (RPS.Scissors, RPS.Paper) => 6,
            _ => 0
        } + (int)you + 1;

    RPS GetRPS(RPS enemy, Result result) => result switch
    {
        Result.Draw => enemy,
        Result.Win => (RPS)(((int)enemy + 1) % 3),
        Result.Lose => (RPS)(((int)enemy + 2) % 3)
    };
}