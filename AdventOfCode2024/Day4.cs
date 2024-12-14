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
        var positiveDiagonalLines = GetDiagnonalLines(characters, 0, x => ++x);
        var negativeDiagnonalLines = GetDiagnonalLines(characters, characters[0].Length - 1, x => --x);

        string[] allLines = [..horizontalLines, ..verticalLines, .. positiveDiagonalLines, ..negativeDiagnonalLines];
        var result = allLines.Select(line => Regex.Matches(line, "XMAS").Count + Regex.Matches(line, "SAMX").Count).Sum();

        //too high
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    static IEnumerable<string> GetDiagnonalLines(char[][] characters, int verticalLineIndex, Func<int, int> diagnonalDirection)
    {
        (int StartY, int StartX)[] startPos = Enumerable.Range(0, characters.Length).Select(rowIndex => (rowIndex, verticalLineIndex))
            .Concat(Enumerable.Range(0, characters[0].Length).Select(columnIndex => (0, columnIndex)))
            .Distinct() // filter out the one duplicate because it's simpler to read than adjusting the horizontal start positions by 1
            .ToArray();

        foreach (var pos in startPos)
        {
            var lineChars = new List<char>();
            var currentX = pos.StartX;
            var currentY = pos.StartY;
            do
            {
                lineChars.Add(characters[currentY][currentX]);
                currentX = diagnonalDirection(currentX);
                currentY++;
            } while (currentX >= 0 && currentX < characters[0].Length && currentY < characters.Length);

            yield return string.Concat(lineChars);
        }
    }
}