namespace AdventOfCode.Aoc2024;

public class Day9 : IDay
{
    public void Run1(string[] data)
    {
        var length = data[0].Sum(x => x - '0');
        var memory = new short[length];
        FillMemory(data[0], memory);

        var memoryIndex = data[0][0] - '0';

        for (int i = length - 1; i > memoryIndex; i--)
        {
            if (memory[i] == -1) continue;

            memory[memoryIndex++] = memory[i];
            memory[i] = -1;
            while (memory[memoryIndex] != -1)
            {
                memoryIndex++;
            }
        }

        long checksum = 0;
        for (int i = 0; i < memoryIndex; i++)
        {
            checksum += memory[i] * i;
        }

        Console.WriteLine($"Checksum: {checksum}");
    }

    public void Run2(string[] data)
    {
        var input = data[0];
        var length = input.Sum(x => x - '0');
        var memory = new short[length];
        FillMemory(input, memory);
        var freeMemoryMap = GetFreeMemory(input);

        bool isFile = true;
        int memoryIndex = length;
        for (int i = input.Length - 1; i >= 0; i--)
        {
            var size = (sbyte)(input[i] - '0');
            memoryIndex -= size;

            if (isFile)
            {
                MoveFile(memoryIndex, size);
            }

            isFile = !isFile;
        }

        long checksum = 0;
        for (int i = 0; i < length; i++)
        {
            if (memory[i] == -1) continue;
            checksum += memory[i] * i;
        }

        Console.WriteLine($"Checksum: {checksum}");

        void MoveFile(int position, sbyte size)
        {
            var (freeMemoryStart, freeSize) = GetFreeMemoryLocation(position, size);
            if (freeSize == 0) return;

            for (int i = 0; i < size; i++)
            {
                memory[freeMemoryStart + i] = memory[position + i];
                memory[position + i] = -1;
            }

            freeMemoryMap[freeSize].Remove(freeMemoryStart);

            if (freeSize != size)
            {
                var remaining = (sbyte)(freeSize - size);
                freeMemoryMap[remaining].Add(freeMemoryStart + size);
            }
        }

        (int freeMemoryStart, sbyte freeMemorySize) GetFreeMemoryLocation(int position, sbyte size) =>
            freeMemoryMap
                .Where(x => x.Key >= size && x.Value.Any(y => y < position))
                .Select(x => (start: x.Value.First(), size: x.Key))
                .OrderBy(x => x.start)
                .FirstOrDefault();
    }

    void FillMemory(string input, short[] memory)
    {
        short current = 0;
        int memoryIndex = 0;
        for (int i = 0; i < input.Length; i++)
        {
            bool isFile = i % 2 == 0;
            for (int j = 0; j < input[i] - '0'; j++)
            {
                memory[memoryIndex++] = isFile ? current : (short)-1;
            }

            if (isFile) current++;
        }
    }

    private Dictionary<sbyte, SortedSet<int>> GetFreeMemory(string input)
    {
        var indicesOfFreeMemory = new Dictionary<sbyte, SortedSet<int>>();

        int memoryIndex = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 != 0)
            {
                var free = (sbyte)(input[i] - '0');
                if (free == 0) continue;

                if (!indicesOfFreeMemory.ContainsKey(free))
                {
                    indicesOfFreeMemory[free] = [];
                }

                indicesOfFreeMemory[free].Add(memoryIndex);
            }

            memoryIndex += input[i] - '0';
        }

        return indicesOfFreeMemory;
    }
}