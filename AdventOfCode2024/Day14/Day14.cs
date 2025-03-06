using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day14;

using Pos = (int y, int x);

public class Day14
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
            if (robot.Position.x == widthMiddleIndex || robot.Position.y == heightMiddleIndex) continue;

            var quadrantX = robot.Position.x < widthMiddleIndex ? 0 : 1;
            var quadrantY = robot.Position.y < heightMiddleIndex ? 0 : 1;
            quadrants[quadrantX][quadrantY]++;
        }

        var safetyFactor = quadrants.SelectMany(q => q).Aggregate(1L, (i, l) => i * l);
        Assert.That(safetyFactor, Is.EqualTo(expectedResult));
    }

    private static List<Robot> ParseRobots(string filename)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var regex = new Regex(@"-?\d+");
        var robots = new List<Robot>();
        foreach (var line in lines)
        {
            var matches = regex.Matches(line);
            robots.Add(new Robot
            {
                Position = (int.Parse(matches[1].Value), int.Parse(matches[0].Value)),
                Velocity = (int.Parse(matches[3].Value), int.Parse(matches[2].Value))
            });
        }

        return robots;
    }

    private class Robot
    {
        public (int y, int x) Position { get; set; }
        public (int yd, int xd) Velocity { get; init; }
        
        public void Move(int gridWidth, int gridHeight)
        {
            Pos newPos = (Position.y + Velocity.yd, Position.x + Velocity.xd);
            Position = (WrapAround(newPos.y, gridHeight), WrapAround(newPos.x, gridWidth));

            int WrapAround(int value, int max) => value < 0 ? max + value : value % max;
        }
    }
}