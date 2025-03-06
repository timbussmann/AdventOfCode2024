using System.Text;

using Coordinate = (int y, int x);

namespace AdventOfCode2024.Day15;

public class Day15
{
    [TestCase("Day15.Test.txt", 10092)]
    [TestCase("Day15.Input.txt", 1413675)]
    public void Part1(string filename, long expectedResult)
    {
        var input = File.ReadAllLines(Input.GetFilePath(filename));
        
        var grid = BuildGrid(input);

        foreach (var instruction in string.Concat(input.SkipWhile(l => l != string.Empty).Skip(1)))
        {
            var robotPosition = grid.First(kvp => kvp.Value == '@').Key;
            var direction = instruction switch
            {
                '<' => (0, -1),
                '^' => (-1, 0),
                '>' => (0, 1),
                'v' => (1, 0),
            };
            
            TryRecursiveShift(grid, robotPosition, direction);
        }
        
        RenderGrid(grid);

        var boxCoordinateSum = grid
            .Where(kvp => kvp.Value == 'O')
            .Select(kvp => kvp.Key.y * 100 + kvp.Key.x)
            .Sum();
        Assert.That(boxCoordinateSum, Is.EqualTo(expectedResult));
    }

    private static Dictionary<(int y, int x), char> BuildGrid(string[] input)
    {
        var gridLines = input.TakeWhile(l => l != string.Empty).ToArray();
        var grid = new Dictionary<(int y, int x), char>();
        for (int y = 0; y < gridLines.Length; y++)
        {
            for (int x = 0; x < gridLines[0].Length; x++)
            {
                grid.Add((y, x), gridLines[y][x]);                
            }
        }

        return grid;
    }

    private bool TryRecursiveShift(Dictionary<Coordinate, char> grid, Coordinate currentPosition, Coordinate direction)
    {
        char currentElement = grid.GetValueOrDefault(currentPosition, '#');
        switch (currentElement)
        {
            case '#':
                return false;
            case '.':
                return true;
        }

        var nextPosition = (currentPosition.y + direction.y, currentPosition.x + direction.x);
        if (TryRecursiveShift(grid, nextPosition, direction))
        {
            grid[nextPosition] = currentElement;
            grid[currentPosition] = '.';
            return true;
        }

        return false;
    }

    private void RenderGrid<T>(Dictionary<Coordinate, T?> grid)
    {
        var yMax = grid.Keys.Max(k => k.y);
        var xMax = grid.Keys.Max(k => k.x);
        StringBuilder sb = new();
        for (var y = 0; y <= yMax; y++)
        {
            for (var x = 0; x <= xMax; x++)
            {
                sb.Append(grid.GetValueOrDefault((y, x), default)?.ToString() ?? " ");
            }

            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
}