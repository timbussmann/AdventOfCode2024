namespace AdventOfCode2024;

public class Day1
{
    [Test]
    public void Part1()
    {
        var allLines = File.ReadAllLines(Input.GetFilePath("Day1.Input.txt"));
        var (leftColumn, rightColumn) = GetColumns(allLines);
        var diff = Enumerable.Zip(leftColumn.Order(), rightColumn.Order())
            .Select(pair => Math.Abs(pair.First - pair.Second))
            .Sum();
        
        Assert.That(diff, Is.EqualTo(1603498));
    }

    [Test]
    public void Part2()
    {
        var allLines = File.ReadAllLines(Input.GetFilePath("Day1.Input.txt"));
        var (leftColumn, rightColumn) = GetColumns(allLines);
        
        var lookup = rightColumn.ToLookup(x => x);
        var similarityScore = leftColumn
            .Select(x => x * lookup[x].Count())
            .Sum();
        
        Assert.That(similarityScore, Is.EqualTo(25574739));
    }
    
    [Test]
    public void Part2_CountBy()
    {
        var allLines = File.ReadAllLines(Input.GetFilePath("Day1.Input.txt"));
        var (leftColum, rightColumn) = GetColumns(allLines);
        
        // new shiny CountBy method in .NET 9
        var lookup = rightColumn.CountBy(x => x).ToDictionary();
        // Dictionaries throw when the key is not present, Lookup's return an empty enumerable instead
        var similarityScore = leftColum.Select(x => x * lookup.GetValueOrDefault(x)).Sum();
        
        Assert.That(similarityScore, Is.EqualTo(25574739));
    }

    static (int[] leftColumn, int[] rightColumn) GetColumns(string[] allLines)
    {
        var pairs = allLines.Select(line =>
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToList();

        return (pairs.Select(p => p.Item1).ToArray(), pairs.Select(p => p.Item2).ToArray());
    }
}