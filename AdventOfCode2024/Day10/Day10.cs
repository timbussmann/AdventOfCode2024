namespace AdventOfCode2024;

public class Day10
{
    [TestCase("Day10.Test.txt", 36)]
    [TestCase("Day10.Input.txt", 496)]
    public void Part1(string filename, int expectedResult)
    {
        var map = ParseMap(filename);
        var trails = GetAllTrails(map);

        var trailScores = trails.Select(kvp => kvp.Value.Distinct().Count());
        Assert.That(trailScores.Sum(), Is.EqualTo(expectedResult));
    }

    [TestCase("Day10.Test.txt", 81)]
    [TestCase("Day10.Input.txt", 1120)]
    public void Part2(string filename, int expectedResult)
    {
        var map = ParseMap(filename);
        var trails = GetAllTrails(map);

        var trailRatings = trails.Select(kvp => kvp.Value.Count);
        Assert.That(trailRatings.Sum(), Is.EqualTo(expectedResult));
    }

    static Dictionary<(int, int), int> ParseMap(string filename)
    {
        var input = File.ReadAllLines(Input.GetFilePath(filename));
        
        Dictionary<(int, int), int> map = new();
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                map.Add((y, x), int.Parse(input[y][x].ToString()));
            }
        }

        return map;
    }

    Dictionary<(int trailHeadY, int trailHeadX), List<(int trailEndY, int trailEndX)>> GetAllTrails(Dictionary<(int, int), int> map)
    {
        var trailEnds = map.Where(pair => pair.Value == 9).Select(pair => pair.Key);

        Dictionary<(int, int), List<(int, int)>> trails = new();
        foreach (var trailEnd in trailEnds)
        {
            FindTrails(currentPosition: trailEnd, trailEnd: trailEnd, map, trails);
        }

        return trails;
    }

    void FindTrails((int y, int x) currentPosition, (int y, int x) trailEnd, Dictionary<(int, int), int> mapDic, Dictionary<(int, int), List<(int, int)>> trails)
    {
        var expectedNextHeight = mapDic[currentPosition] - 1;
        foreach ((int dy, int dx) direction in Directions)
        {
            var newPos = (currentPosition.y + direction.dy, currentPosition.x + direction.dx);
            if (mapDic.TryGetValue(newPos, out int height) && height == expectedNextHeight)
            {
                if (height == 0)
                {
                    var trailsEnds = trails.GetValueOrDefault(newPos, new List<(int, int)>());
                    trailsEnds.Add(trailEnd);
                    trails[newPos] = trailsEnds;
                }
                else
                {
                    FindTrails(newPos, trailEnd, mapDic, trails);
                }
            }
        }
    }

    static readonly (int, int)[] Directions = [(1, 0), (0, -1), (-1, 0), (0, 1)];
}