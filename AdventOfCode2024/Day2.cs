namespace AdventOfCode2024;

public class Day2
{
    [TestCase("Day2.Test.txt", 2)]
    [TestCase("Day2.Input.txt", 224)]
    public void Part1(string filename, int expectedResult)
    {
        var reportLines = File.ReadAllLines(filename);
        var reports = reportLines.Select(x => x.Split(" ").Select(int.Parse).ToArray()).ToArray();

        int validReports = 0;
        foreach (var report in reports)
        {
            var diff = report.Zip(report.Skip(1), (x, y) => x - y).ToArray();
            if (diff.All(d => d is >= 1 and <= 3) || diff.All(d => d is >= -3 and <= -1))
            {
                validReports++;
            }
        }
        
        Assert.That(validReports, Is.EqualTo(expectedResult));
    }

    [TestCase("Day2.Test.txt", 4)]
    [TestCase("Day2.Input.txt", 293)]
    public void Part2(string filename, int expectedResult)
    {
        var reportLines = File.ReadAllLines(filename);
        var reports = reportLines.Select(x => x.Split(" ").Select(int.Parse).ToArray()).ToArray();
        var dampenedReports = reports.Select(report =>
        {
            return report.Select((level, i) => report.Take(i).Concat(report.Skip(i + 1).ToArray())).ToArray();
        });

        int validReports = 0;
        foreach (var reportGroup in dampenedReports)
        {
            validReports += reportGroup.Any(report =>
            {
                var diff = report.Zip(report.Skip(1), (x, y) => x - y).ToArray();
                return diff.All(d => d is >= 1 and <= 3) || diff.All(d => d is >= -3 and <= -1);
            })
                ? 1
                : 0;
        }
        
        Assert.That(validReports, Is.EqualTo(expectedResult));
    }
}