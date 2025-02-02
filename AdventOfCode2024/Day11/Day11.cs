using System.Collections.Concurrent;

namespace AdventOfCode2024.Day11;

public class Day11
{
    [TestCase("125 17", 55312)]
    [TestCase("5178527 8525 22 376299 3 69312 0 275", 189547)]
    public void Part1(string stonesInput, long expected)
    {
        var cache = new ConcurrentDictionary<(long value, int blinks), long>();
        long total = stonesInput
            .Split(" ")
            .Sum(stone => RecursiveCounting(long.Parse(stone), 25, cache));

        Assert.That(total, Is.EqualTo(expected));
    }
    
    [TestCase("5178527 8525 22 376299 3 69312 0 275", 224577979481346)]
    public void Part2(string stonesInput, long expected)
    {
        var cache = new ConcurrentDictionary<(long value, int blinks), long>();
        long total = stonesInput
            .Split(" ")
            .Sum(stone => RecursiveCounting(long.Parse(stone), 75, cache));

        Assert.That(total, Is.EqualTo(expected));
    }

    static long RecursiveCounting(long stone, int blinks, ConcurrentDictionary<(long value, int blinks), long> cache) =>
        cache.GetOrAdd((stone, blinks), static (tuple, cache) =>
        {
            if (tuple.blinks == 0)
            {
                return 1;
            }

            if (tuple.value == 0)
            {
                return RecursiveCounting(1, tuple.blinks - 1, cache);
            }

            if (tuple.value.ToString().Length % 2 == 0)
            {
                var stoneString = tuple.value.ToString();
                var length = stoneString.Length / 2;
                var left = long.Parse(stoneString.Substring(0, length));
                var right = long.Parse(stoneString.Substring(length, length));
                return RecursiveCounting(left, tuple.blinks - 1, cache) + RecursiveCounting(right, tuple.blinks - 1, cache);
            }

            return RecursiveCounting(tuple.value * 2024, tuple.blinks - 1, cache);
        }, cache);
}