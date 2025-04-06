namespace AdventOfCode2024.Day16;

public class Day16
{
    [TestCase("Day16.Test1.txt", 7036)]
    public void Part1(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        Dictionary<Coordinate, char> grid = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                grid.Add(new Coordinate(y, x), lines[y][x]);
            }
        }
        
        GridHelper.RenderGrid(grid);
    }
}