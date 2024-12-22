namespace AdventOfCode2024;

public class Day7
{
    [TestCase("Day7.Test.txt", 3749)]
    [TestCase("Day7.Input.txt", 5702958180383)]
    public void Part1(string filename, long expectedResult)
    {
        var lines = File.ReadAllLines(filename);
        var equations = lines.Select(line =>
        {
            var lineParts = line.Split(":");
            var operationElements = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (equationResult: long.Parse(lineParts[0]),
                equationElements: operationElements.Select(long.Parse).ToArray());
        });

        var totalResult = 
            (from equation in equations 
                let allResults = ComputeAllOperationsV1(equation.equationElements[0], equation.equationElements[1..], equation.equationResult) 
                where allResults.Any(r => r == equation.equationResult) 
                select equation.equationResult).Sum();

        Assert.That(totalResult, Is.EqualTo(expectedResult));
    }

    [TestCase("Day7.Test.txt", 11387)]
    [TestCase("Day7.Input.txt", 92612386119138)]
    public void Part2(string filename, long expectedResult)
    {
        var lines = File.ReadAllLines(filename);
        var equations = lines.Select(line =>
        {
            var lineParts = line.Split(":");
            var operationElements = lineParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return (equationResult: long.Parse(lineParts[0]),
                equationElements: operationElements.Select(long.Parse).ToArray());
        });

        var totalResult = 
            (from equation in equations 
                let allResults = ComputeAllOperationsV2(equation.equationElements[0], equation.equationElements[1..], equation.equationResult) 
                where allResults.Any(r => r == equation.equationResult) 
                select equation.equationResult).Sum();

        Assert.That(totalResult, Is.EqualTo(expectedResult));
    }

    private static long[] ComputeAllOperationsV1(long input, long[] nextElements, long expectedResult)
    {
        if (nextElements.Length == 0)
        {
            return [input];
        }

        List<long> results = [];
        var additionResult = input + nextElements[0];
        if (additionResult <= expectedResult)
        {
            results.AddRange(ComputeAllOperationsV1(additionResult, nextElements[1..], expectedResult));
        }
        var multiplicationResult = input * nextElements[0];
        if (multiplicationResult <= expectedResult)
        {
            results.AddRange(ComputeAllOperationsV1(multiplicationResult, nextElements[1..], expectedResult));
        }

        return results.ToArray();
    }
    
    private static long[] ComputeAllOperationsV2(long input, long[] nextElements, long expectedResult)
    {
        if (nextElements.Length == 0)
        {
            return [input];
        }

        List<long> results = [];
        var additionResult = input + nextElements[0];
        if (additionResult <= expectedResult)
        {
            results.AddRange(ComputeAllOperationsV2(additionResult, nextElements[1..], expectedResult));
        }
        var multiplicationResult = input * nextElements[0];
        if (multiplicationResult <= expectedResult)
        {
            results.AddRange(ComputeAllOperationsV2(multiplicationResult, nextElements[1..], expectedResult));
        }

        var combinationResult = long.Parse(string.Concat(input, nextElements[0]));
        if (combinationResult <= expectedResult)
        {
            results.AddRange(ComputeAllOperationsV2(combinationResult, nextElements[1..], expectedResult));
        }

        return results.ToArray();
    }
}