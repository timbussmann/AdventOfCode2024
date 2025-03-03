using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day14;

using Pos = (int y, int x);

public class Day14
{
    [TestCase("Day14.Test.txt", 11, 7, 12)]
    [TestCase("Day14.Input.txt", 101, 103, 222062148)]
    public void Part1(string filename, int width, int height, long expectedResult)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var regex = new Regex(@"-?\d+");
        List<Robot> robots = new List<Robot>();
        foreach (var line in lines)
        {
            var matches = regex.Matches(line);
            robots.Add(new Robot
            {
                Position = (int.Parse(matches[1].Value), int.Parse(matches[0].Value)),
                Velocity = (int.Parse(matches[3].Value), int.Parse(matches[2].Value))
            });
        }

        for (var i = 0; i < 100; i++)
            foreach (var robot in robots)
            {
                Pos newPos = (robot.Position.y + robot.Velocity.yd, robot.Position.x + robot.Velocity.xd);
                newPos = (WrapAround(newPos.y, height), WrapAround(newPos.x, width));
                robot.Position = newPos;
            }

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

        int WrapAround(int value, int max)
        {
            return value < 0 ? max + value : value % max;
        }
    }

    private class Robot
    {
        public (int y, int x) Position { get; set; }
        public (int yd, int xd) Velocity { get; set; }
    }
}