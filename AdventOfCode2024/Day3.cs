using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public partial class Day3
{
    [TestCase("Day3.Test.txt", 161)]
    [TestCase("Day3.Input.txt", 162813399)]
    public void Part1(string filename, int expectedResult)
    {
        var input = File.ReadAllText(filename);
        
        var matches = InputParserRegex().Matches(input);
        var result = matches
            .Select(match => int.Parse(match.Groups["X"].Value) * int.Parse(match.Groups["Y"].Value))
            .Sum();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [GeneratedRegex(@"mul\((?<X>\d{1,3}),(?<Y>\d{1,3})\)")]
    private static partial Regex InputParserRegex();
}