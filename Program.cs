using System.Reflection;
using AdventOfCode;

//Run(2021, 16, 2);
Run(2022, 9, 1);

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

    var data = File.ReadAllLines($"Aoc{year}/data/day{day}.txt");
    if (task == 1) dayObject!.Run1(data);
    else dayObject!.Run2(data);
}