namespace AdventOfCode2024;

public class Day10
{
    [TestCase("Day10.Test.txt", 36)]
    [TestCase("Day10.Input.txt", 496)]
    public void Part1(string filename, int expectedResult)
    {
        var input = File.ReadAllLines(filename);
        
        Dictionary<(int, int), int> map = new();
        for (var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                map.Add((y, x), int.Parse(input[y][x].ToString()));
            }
        }

        var trailEnds = map.Where(pair => pair.Value == 9).Select(pair => pair.Key);


        Dictionary<(int, int), HashSet<(int, int)>> trails = new();
        foreach (var trailEnd in trailEnds)
        {
            GetNumberOfTrails(trailEnd, trailEnd, map, trails);
        }

        Assert.That(trails.Select(kvp => kvp.Value.Count).Sum(), Is.EqualTo(expectedResult));
    }

    static readonly (int, int)[] Directions = [(1, 0), (0, -1), (-1, 0), (0, 1)];

    void GetNumberOfTrails((int y, int x) currentPosition, (int y, int x) trailEnd, Dictionary<(int, int), int> mapDic, Dictionary<(int, int), HashSet<(int, int)>> trails)
    {
        var expectedNextHeight = mapDic[currentPosition] - 1;
        foreach ((int dy, int dx) direction in Directions)
        {
            var newPos = (currentPosition.y + direction.dy, currentPosition.x + direction.dx);
            if (mapDic.TryGetValue(newPos, out int height) && height == expectedNextHeight)
            {
                if (height == 0)
                {
                    var trailsEnds = trails.GetValueOrDefault(newPos, new HashSet<(int, int)>());
                    trailsEnds.Add(trailEnd);
                    trails[newPos] = trailsEnds;
                }
                else
                {
                    GetNumberOfTrails(newPos, trailEnd, mapDic,trails);
                }
            }
        }
    }
}