using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day14;

public partial class Day14
{
    [TestCase("Day14.Test.txt", 11, 7, 12)]
    [TestCase("Day14.Input.txt", 101, 103, 222062148)]
    public void Part1(string filename, int width, int height, long expectedResult)
    {
        var robots = ParseRobots(filename);

        for (var i = 0; i < 100; i++)
            foreach (var robot in robots) 
                robot.Move(width, height);

        long[][] quadrants = [[0, 0], [0, 0]];
        var widthMiddleIndex = (width - 1) / 2;
        var heightMiddleIndex = (height - 1) / 2;
        foreach (var robot in robots)
        {
            if (robot.Position.x == widthMiddleIndex || robot.Position.y == heightMiddleIndex) 
                continue;

            var quadrantX = robot.Position.x < widthMiddleIndex ? 0 : 1;
            var quadrantY = robot.Position.y < heightMiddleIndex ? 0 : 1;
            quadrants[quadrantX][quadrantY]++;
        }

        var safetyFactor = quadrants.SelectMany(q => q).Aggregate(1L, (i, l) => i * l);
        Assert.That(safetyFactor, Is.EqualTo(expectedResult));
    }

    [Explicit]
    [TestCase("Day14.Input.txt", 101, 103, 6000, 8000)]
    public void Part2(string filename, int width, int height, int renderStartRange, int renderEndRange)
    {
        var robots = ParseRobots("Day14.Input.txt");
        var outputFile = File.CreateText("Day14.Output.txt");

        var grid = new char[height * width];
        for (var i = 0; i < renderEndRange; i++)
        {
            Array.Fill(grid, ' ');
            foreach (var robot in robots)
            {
                robot.Move(width, height);
                grid[robot.Position.y * width + robot.Position.x] = 'X';
            }

            if (i >= renderStartRange && i <= renderEndRange)
            {
                outputFile.WriteLine($"---------------------- {i + 1} seconds -----------------------");
                foreach (var line in grid.Chunk(width))
                {
                    outputFile.WriteLine(line);
                }
            }
            
        }
        
        outputFile.Close();
        
        // 7520 seconds is the right answer - found by manually inspecting the output ;)
    }

    private static List<Robot> ParseRobots(string filename) =>
        File.ReadAllLines(Input.GetFilePath(filename))
            .Select(line => Regex().Matches(line))
            .Select(matches => new Robot
            {
                Position = (int.Parse(matches[1].Value), int.Parse(matches[0].Value)), 
                Velocity = (int.Parse(matches[3].Value), int.Parse(matches[2].Value))
            }).ToList();

    private class Robot
    {
        public (int y, int x) Position { get; set; }
        public (int yd, int xd) Velocity { get; init; }
        
        public void Move(int gridWidth, int gridHeight)
        {
            // always add the grid width/height to handle negative values
            Position = ((Position.y + Velocity.yd + gridHeight) % gridHeight, (Position.x + Velocity.xd + gridWidth) % gridWidth);
        }
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex Regex();
}