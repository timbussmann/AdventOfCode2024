using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public partial class Day3
{
    [TestCase("Day3.Test.txt", 161)]
    [TestCase("Day3.Input.txt", 162813399)]
    public void Part1(string filename, int expectedResult)
    {
        var input = File.ReadAllText(Input.GetFilePath(filename));
        
        var instructions = MultiplyInstructionRegex().Matches(input);
        var result = instructions
            .Select(match => int.Parse(match.Groups["X"].Value) * int.Parse(match.Groups["Y"].Value))
            .Sum();

        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [TestCase("Day3.Test2.txt", 48)]
    [TestCase("Day3.Input.txt", 53783319)]
    public void Part2(string filename, long expectedResult)
    {
        var input = File.ReadAllText(Input.GetFilePath(filename));

        var activeInputBlockMatches = ActiveInstructionsBlocksRegex().Matches("do()" + input + "don't()");
        var instructionMatches = activeInputBlockMatches.SelectMany(match => MultiplyInstructionRegex().Matches(match.Value));
        var result = instructionMatches
            .Select(match => int.Parse(match.Groups["X"].Value) * int.Parse(match.Groups["Y"].Value))
            .Sum();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [GeneratedRegex(@"mul\((?<X>\d{1,3}),(?<Y>\d{1,3})\)")]
    private static partial Regex MultiplyInstructionRegex();
    
    // lazy matching between do() and don't() as well as RegexOptions.Singleline are crucial!
    [GeneratedRegex(@"do\(\).*?don't\(\)", RegexOptions.Singleline)]
    private static partial Regex ActiveInstructionsBlocksRegex();
}