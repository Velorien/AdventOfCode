using System.Diagnostics;
using System.Reflection;
using AdventOfCode;

Run(2024, 9, 2);

void Run(int year, int day, int task)
{
    var dayType = Assembly.GetExecutingAssembly()
        .GetTypes().FirstOrDefault(t => t.IsClass && t.IsAssignableTo(typeof(IDay))
            & t.Namespace == $"AdventOfCode.Aoc{year}" && t.Name == $"Day{day}");

    if (dayType is null)
    {
        Console.WriteLine("Type to run not found");
        return;
    }

    var dayObject = Activator.CreateInstance(dayType) as IDay;

    var sw = Stopwatch.StartNew();
    var data = File.ReadAllLines($"Aoc{year}/data/day{day}.txt");
    if (task == 1) dayObject!.Run1(data);
    else dayObject!.Run2(data);
    sw.Stop();
    Console.WriteLine($"Took {sw.ElapsedMilliseconds} milliseconds");
}