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
                '<' => new Coordinate(0, -1),
                '^' => new Coordinate(-1, 0),
                '>' => new Coordinate(0, 1),
                'v' => new Coordinate(1, 0),
            };
            
            TryRecursiveShift(grid, robotPosition, direction);
        }

        GridHelper.RenderGrid(grid);

        var boxCoordinateSum = grid
            .Where(kvp => kvp.Value == 'O')
            .Select(kvp => kvp.Key.Y * 100 + kvp.Key.X)
            .Sum();
        Assert.That(boxCoordinateSum, Is.EqualTo(expectedResult));
    }

    private static Dictionary<Coordinate, char> BuildGrid(string[] input)
    {
        var gridLines = input.TakeWhile(l => l != string.Empty).ToArray();
        var grid = new Dictionary<Coordinate, char>();
        for (int y = 0; y < gridLines.Length; y++)
        {
            for (int x = 0; x < gridLines[0].Length; x++)
            {
                grid.Add(new Coordinate(y, x), gridLines[y][x]);                
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

        var nextPosition = new Coordinate(currentPosition.Y + direction.Y, currentPosition.X + direction.X);
        if (TryRecursiveShift(grid, nextPosition, direction))
        {
            grid[nextPosition] = currentElement;
            grid[currentPosition] = '.';
            return true;
        }

        return false;
    }
}