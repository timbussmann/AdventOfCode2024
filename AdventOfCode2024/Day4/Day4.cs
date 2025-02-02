using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day4
{
    [TestCase("Day4.Test.txt", 18)]
    [TestCase("Day4.Input.txt", 2536)]
    public void Part1(string filename, int expectedResult)
    {
        var characters = File.ReadAllLines(Input.GetFilePath(filename)).Select(line => line.ToCharArray()).ToArray();

        var horizontalLines = characters.Select(lineCharacters => string.Concat(lineCharacters));
        var verticalLines = Enumerable.Range(0, characters[0].Length).Select(columnIndex => string.Concat(characters.Select(line => line[columnIndex])));
        var positiveDiagonalLines = GetDiagnonalLines(characters, 0, x => ++x);
        var negativeDiagnonalLines = GetDiagnonalLines(characters, characters[0].Length - 1, x => --x);

        string[] allLines = [..horizontalLines, ..verticalLines, .. positiveDiagonalLines, ..negativeDiagnonalLines];
        var result = allLines.Select(line => Regex.Matches(line, "XMAS").Count + Regex.Matches(line, "SAMX").Count).Sum();

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("Day4.Test.txt", 9)]
    [TestCase("Day4.Input.txt", 1875)]
    public void Part2(string filename, int expectedResult)
    {
        var characters = File.ReadAllLines(Input.GetFilePath(filename)).Select(line => line.ToCharArray()).ToArray();
        string[] validXmasCombinations = ["MMASS", "SMASM", "MSAMS", "SSAMM"];

        var allCrosses = GetCrosses(characters).ToArray();
        var result = allCrosses.Count(block => validXmasCombinations.Contains(block));

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    IEnumerable<string> GetCrosses(char[][] characters)
    {
        for (var y = 0; y < characters.Length - 2; y++)
        {
            for (var x = 0; x < characters[0].Length - 2; x++)
            {
                char[] blockCharacters =
                [
                    characters[y][x], characters[y][x + 2],
                    characters[y + 1][x + 1],
                    characters[y + 2][x], characters[y + 2][x + 2]
                ];
                yield return string.Concat(blockCharacters);
            }
        }
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