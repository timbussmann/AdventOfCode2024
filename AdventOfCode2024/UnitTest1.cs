namespace AdventOfCode2024;

public class Day1
{
    [Test]
    public void Part1()
    {
        var allLines = File.ReadAllLines("Day1.Input.txt");
        var (leftList, rightList) = GetRows(allLines);
        var diff = leftList
            .Order()
            .Zip(rightList.Order())
            .Select(pair => Math.Max(pair.First, pair.Second) - Math.Min(pair.First, pair.Second))
            .Sum();
        
        Assert.That(diff, Is.EqualTo(1603498));
    }

    [Test]
    public void Part2()
    {
        var allLines = File.ReadAllLines("Day1.Input.txt");
        var (leftList, rightList) = GetRows(allLines);
        var lookup = rightList.ToLookup(x => x);
        var similarityScore = leftList.Select(x => x * lookup[x].Count()).Sum();
        
        Assert.That(similarityScore, Is.EqualTo(25574739));
    }

    static (int[] leftList, int[] rightList) GetRows(string[] allLines)
    {
        var pairs = allLines.Select(line =>
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();
        
        var leftList = pairs.Select(p => p.Item1).ToArray();
        var rightList = pairs.Select(p => p.Item2).ToArray();
        return (leftList, rightList);
    }
}