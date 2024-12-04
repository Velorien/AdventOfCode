namespace AdventOfCode.Aoc2024;

public class Day2 : IDay
{
    public void Run1(string[] data)
    {
        int safeCount = 0;
        foreach (var levels in ParseInput(data))
        {
            if (IsSafe(levels))
            {
                safeCount++;
            }
        }

        Console.WriteLine($"There are {safeCount} safe levels");
    }

    public void Run2(string[] data)
    {
        int safeCount = 0;
        foreach (var levels in ParseInput(data))
        {
            if (IsSafe(levels))
            {
                safeCount++;
            }
            else
            {
                for (int i = 0; i < levels.Length; i++)
                {
                    var buffer = new int[levels.Length - 1];
                    FillBuffer(buffer, levels, i);
                    if (IsSafe(buffer))
                    {
                        safeCount++;
                        break;
                    }
                }
            }
        }

        Console.WriteLine($"There are {safeCount} safe levels");
    }
    
    IEnumerable<int[]> ParseInput(string[] data)
    {
        foreach (var line in data)
        {
            yield return line.Split(' ').Select(int.Parse).ToArray();
        }
    }

    bool IsSafe(int[] levels)
    {
        int deltaSign = Math.Sign(levels[1] - levels[0]);
        if (deltaSign == 0)
        {
            return false;
        }
        
        for (int i = 0; i < levels.Length - 1; i++)
        {
            var delta = levels[i + 1] - levels[i];
            var absDelta = Math.Abs(delta);
            var currentSign = Math.Sign(delta);

            if (deltaSign != currentSign || absDelta > 3)
            {
                return false;
            }
        }

        return true;
    }

    void FillBuffer(int[] buffer, int[] source, int skipIndex)
    {
        int writeIndex = 0;
        for (int i = 0; i < source.Length; i++)
        {
            if (i == skipIndex) continue;
            buffer[writeIndex++] = source[i];
        }
    }
}