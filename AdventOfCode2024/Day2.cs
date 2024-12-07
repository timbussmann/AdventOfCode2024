namespace AdventOfCode2024;

public class Day2
{
    [TestCase("Day2.Test.txt", 2)]
    [TestCase("Day2.Input.txt", 224)]
    public void Part1(string filename, int expectedResult)
    {
        var reportLines = File.ReadAllLines(filename);
        var reports = ParseReports(reportLines);

        int validReports = reports.Count(IsValidReport);

        Assert.That(validReports, Is.EqualTo(expectedResult));
    }

    [TestCase("Day2.Test.txt", 4)]
    [TestCase("Day2.Input.txt", 293)]
    public void Part2(string filename, int expectedResult)
    {
        var reportLines = File.ReadAllLines(filename);
        var reports = ParseReports(reportLines);
        var dampenedReportGroups = reports.Select(report => report.Select((_, i) =>
        {
            // Using LINQ:
            // return report.Take(i).Concat(report.Skip(i + 1).ToArray());
                
            // Using Collection spread and range operators:
            return (int[])[..report[..i], ..report[(i + 1)..]];
        }).ToArray());

        var validReports = dampenedReportGroups.Sum(reportGroup => reportGroup.Any(IsValidReport) ? 1 : 0);

        Assert.That(validReports, Is.EqualTo(expectedResult));
    }

    static bool IsValidReport(int[] report)
    {
        var diff = report.Zip(report.Skip(1), (x, y) => x - y).ToArray();
        return diff.All(d => d is >= 1 and <= 3) || diff.All(d => d is >= -3 and <= -1);
    }

    static int[][] ParseReports(string[] reportLines)
    {
        var reports = reportLines.Select(x => x.Split(" ").Select(int.Parse).ToArray()).ToArray();
        return reports;
    }
}