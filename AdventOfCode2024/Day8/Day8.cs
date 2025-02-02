namespace AdventOfCode2024;

public class Day8
{
    [TestCase("Day8.Test.txt", 14)]
    [TestCase("Day8.Input.txt", 409)]
    public void Part1(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var antiNodes = CalculateAntiNodes(lines, Part1AntiNodes);
        
        Assert.That(antiNodes.Count, Is.EqualTo(expectedResult));
        
        Coordinate[] Part1AntiNodes(Coordinate sibling, Coordinate node, string[] lines)
        {
            var distance = new Coordinate(sibling.Y - node.Y, sibling.X - node.X);
            var antiNode = new Coordinate(sibling.Y + distance.Y, sibling.X + distance.X);
            return IsWithinMap(antiNode, lines) ? [antiNode] : [];
        }
    }

    [TestCase("Day8.Test.txt", 34)]
    [TestCase("Day8.Input.txt", 1308)]
    public void Part2(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var antiNodes = CalculateAntiNodes(lines, Part2AntiNodes);

        Assert.That(antiNodes.Count, Is.EqualTo(expectedResult));
        
        List<Coordinate> Part2AntiNodes(Coordinate sibling, Coordinate node, string[] lines)
        {
            var antiNodeLocations = new List<Coordinate>();
            var distance = new Coordinate(sibling.Y - node.Y, sibling.X - node.X);
            var antiNodeCoords = new Coordinate(sibling.Y, sibling.X);
            do
            {
                antiNodeLocations.Add(antiNodeCoords);
                antiNodeCoords = new Coordinate(antiNodeCoords.Y + distance.Y, antiNodeCoords.X + distance.X);
            } while (IsWithinMap(antiNodeCoords, lines));

            return antiNodeLocations;
        }
    }

    private static IEnumerable<Coordinate> CalculateAntiNodes(string[] lines,
        Func<Coordinate, Coordinate, string[], IEnumerable<Coordinate>> antiNodesFunction)
    {
        var nodes = ParseNodes(lines);
        var nodesByFrequency = nodes.ToLookup(n => n.Value, n => n.Key);

        var antiNodes = nodes.SelectMany(node =>
        {
            var siblingNodes = nodesByFrequency[node.Value]
                .Where(n => n != node.Key);
            return siblingNodes.SelectMany(sibling => antiNodesFunction(sibling, node.Key, lines));
        });
        
        return antiNodes.ToHashSet();
    }

    private static bool IsWithinMap(Coordinate coordinate, string[] map) =>
        coordinate.Y < map.Length
        && coordinate.Y >= 0
        && coordinate.X < map[0].Length
        && coordinate.X >= 0;

    private static Dictionary<Coordinate, char> ParseNodes(string[] lines)
    {
        Dictionary<Coordinate, char> nodes = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                var value = lines[y][x];
                if (char.IsAsciiLetterOrDigit(value))
                {
                    nodes.Add(new Coordinate(y, x), value);
                }
            }
        }

        return nodes;
    }

    readonly record struct Coordinate(int Y, int X);
}