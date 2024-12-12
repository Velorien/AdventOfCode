using System.Security.Cryptography;

namespace AdventOfCode.Aoc2015;

public class Day4 : IDay
{
    private readonly MD5 _md5 = MD5.Create();

    public void Run1(string[] data)
    {
        var inputLength = data[0].Length;
        var bytes = data[0].Concat("000000").Select(x => (byte)x).ToArray();
        var hash = new byte[16];
        int value = 0;

        while (true)
        {
            var numberLength = SetBytes(bytes, inputLength, value++);
            _md5.TryComputeHash(bytes[..(inputLength + numberLength)], hash, out _);

            if (IsSolution(hash)) break;
        }

        Console.WriteLine($"Number: {value}");


        bool IsSolution(byte[] data) => data[0] == 0 && data[1] == 0 && (data[2] & 0xF0) == 0;
    }

    public void Run2(string[] data)
    {
        var inputLength = data[0].Length;
        var bytes = data[0].Concat("0000000").Select(x => (byte)x).ToArray();
        var hash = new byte[16];
        int value = 0;

        while (true)
        {
            var numberLength = SetBytes(bytes, inputLength, value++);
            _md5.TryComputeHash(bytes[..(inputLength + numberLength)], hash, out _);

            if (IsSolution(hash)) break;
        }

        Console.WriteLine($"Number: {value}");

        bool IsSolution(byte[] data) => data[0] == 0 && data[1] == 0 && data[2] == 0;
    }

    private static int SetBytes(byte[] bytes, int inputLength, int number)
    {
        var length = number.GetNumberOfDigits();
        for (var i = 1; i <= length; i++)
        {
            bytes[inputLength + length - i] = (byte)('0' + number % 10);
            number /= 10;
        }

        return length;
    }
}