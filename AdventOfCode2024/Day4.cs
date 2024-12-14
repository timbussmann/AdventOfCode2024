using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day4
{
    [TestCase("Day4.Test.txt", 18)]
    [TestCase("Day4.Input.txt", 2536)]
    public void Part1(string filename, int expectedResult)
    {
        char[][] characters = File.ReadAllLines(filename).Select(line => line.ToCharArray()).ToArray();

        var horizontalLines = characters.Select(lineCharacters => string.Concat(lineCharacters));
        var verticalLines = Enumerable.Range(0, characters[0].Length).Select(columnIndex => string.Concat(characters.Select(line => line[columnIndex])));
        var positiveDiagonalLines = GetPositiveDiagnonalLines(characters);
        var negativeDiagnonalLines = GetNegativeDiagnonalLines(characters);

        string[] allLines = [..horizontalLines, ..verticalLines, .. positiveDiagonalLines, ..negativeDiagnonalLines];
        var result = allLines.Select(line => Regex.Matches(line, "XMAS").Count + Regex.Matches(line, "SAMX").Count).Sum();

        //too high
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    static List<string> GetNegativeDiagnonalLines(char[][] characters)
    {
        (int StartY, int StartX)[] startPos = Enumerable.Range(0, characters.Length).Select(rowIndex => (rowIndex, characters[0].Length - 1))
            .Concat(Enumerable.Range(0, characters[0].Length - 1).Select(columnIndex => (0, columnIndex))).ToArray();
        List<string> negativeDiagnonalLines = new();

        foreach (var pos in startPos)
        {
            var lineChars = new List<char>();
            var currentX = pos.StartX;
            var currentY = pos.StartY;
            do
            {
                lineChars.Add(characters[currentY][currentX]);
                currentX--;
                currentY++;
            } while (currentX >= 0 && currentY < characters.Length);
            negativeDiagnonalLines.Add(string.Concat(lineChars));
        }

        return negativeDiagnonalLines;
    }

    static List<string> GetPositiveDiagnonalLines(char[][] characters)
    {
        (int StartY, int StartX)[] startPos = Enumerable.Range(0, characters.Length).Select(rowIndex => (rowIndex, 0))
            .Concat(Enumerable.Range(1, characters[0].Length - 1).Select(columnIndex => (0, columnIndex))).ToArray();
        List<string> positiveDiagonalLines = new();

        foreach (var pos in startPos)
        {
            var lineChars = new List<char>();
            var currentX = pos.StartX;
            var currentY = pos.StartY;
            do
            {
                lineChars.Add(characters[currentY][currentX]);
                currentX++;
                currentY++;
            } while (currentX < characters[0].Length && currentY < characters.Length);
            positiveDiagonalLines.Add(string.Concat(lineChars));
        }

        return positiveDiagonalLines;
    }
}