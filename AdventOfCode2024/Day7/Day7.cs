namespace AdventOfCode2024;

public class Day7
{
    [TestCase("Day7.Test.txt", 3749)]
    [TestCase("Day7.Input.txt", 5702958180383)]
    public void Part1(string filename, long expectedResult)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var equations = Parse(lines);

        Func<long, long, long>[] operations = [
            (x, y) => x + y,
            (x, y) => x * y
        ];
        
        var totalResult = 
            (from equation in equations 
                let allResults = ComputeAllOperations(equation.equationElements[0], equation.equationElements[1..], equation.equationResult, operations) 
                where allResults.Any(r => r == equation.equationResult) 
                select equation.equationResult).Sum();

        Assert.That(totalResult, Is.EqualTo(expectedResult));
    }

    [TestCase("Day7.Test.txt", 11387)]
    [TestCase("Day7.Input.txt", 92612386119138)]
    public void Part2(string filename, long expectedResult)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var equations = Parse(lines);

        Func<long, long, long>[] operations = [
            (x, y) => x + y,
            (x, y) => x * y,
            (x, y) => long.Parse(string.Concat(x, y))
        ];
        
        var totalResult = 
            (from equation in equations 
                let allResults = ComputeAllOperations(equation.equationElements[0], equation.equationElements[1..], equation.equationResult, operations) 
                where allResults.Any(r => r == equation.equationResult) 
                select equation.equationResult).Sum();

        Assert.That(totalResult, Is.EqualTo(expectedResult));
    }

    private static long[] ComputeAllOperations(long input, long[] nextElements, long expectedResult, Func<long, long, long>[] operations)
    {
        if (nextElements.Length == 0)
        {
            return [input];
        }
        
        return operations
            .Select(op => op(input, nextElements[0]))
            .Where(x => x <= expectedResult)
            .SelectMany(x => ComputeAllOperations(x, nextElements[1..], expectedResult, operations))
            .ToArray();
    }

    private static IEnumerable<(long equationResult, long[] equationElements)> Parse(string[] lines)
    {
        var equations = lines.Select(line =>
        {
            var lineParts = line.Split(":");
            var operationElements = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (equationResult: long.Parse(lineParts[0]),
                equationElements: operationElements.Select(long.Parse).ToArray());
        });
        return equations;
    }
}