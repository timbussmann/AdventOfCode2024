namespace AdventOfCode2024;

public class Day8
{
    [TestCase("Day8.Test.txt", 14)]
    [TestCase("Day8.Input.txt", 409)]
    public void Part1(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(filename);
        var nodes = ParseNodes(lines);
        var nodesByFrequency = nodes.ToLookup(n => n.Frequency, n => n.Coordinate);

        HashSet<(int y, int x)> antiNodeLocations = new ();
        foreach (var node in nodes)
        {
            var siblingNodes = nodesByFrequency[node.Frequency].Where(n => n != node.Coordinate);
            foreach (var sibling in siblingNodes)
            {
                var distance = (y: sibling.Y - node.Coordinate.Y, x: sibling.X - node.Coordinate.X);
                var antiNodeCoords = (y: sibling.Y + distance.y, x: sibling.X + distance.x);
                if (
                    antiNodeCoords.y < lines.Length
                    && antiNodeCoords.y >= 0
                    && antiNodeCoords.x < lines[0].Length 
                    && antiNodeCoords.x >= 0)
                {
                    antiNodeLocations.Add((antiNodeCoords.y, antiNodeCoords.x));
                }
            }
        }
        
        Assert.That(antiNodeLocations.Count, Is.EqualTo(expectedResult));
    }

    [TestCase("Day8.Test.txt", 34)]
    [TestCase("Day8.Input.txt", 1308)]
    public void Part2(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(filename);
        var nodes = ParseNodes(lines);
        var nodesByFrequency = nodes.ToLookup(n => n.Frequency, n => n.Coordinate);

        HashSet<Coordinate> antiNodeLocations = new ();
        foreach (var node in nodes)
        {
            antiNodeLocations.Add(node.Coordinate);
            var siblingNodes = nodesByFrequency[node.Frequency].Where(n => n != node.Coordinate);
            foreach (var sibling in siblingNodes)
            {
                var distance = new Coordinate(sibling.Y - node.Coordinate.Y, sibling.X - node.Coordinate.X);
                var antiNodeCoords = new Coordinate(sibling.Y, sibling.X);
                do
                {
                    antiNodeCoords = new Coordinate(antiNodeCoords.Y + distance.Y, antiNodeCoords.X + distance.X);
                } while (antiNodeCoords.Y < lines.Length
                         && antiNodeCoords.Y >= 0
                         && antiNodeCoords.X < lines[0].Length 
                         && antiNodeCoords.X >= 0
                         && SaveAntiNode(antiNodeCoords));
            }
        }
        
        Assert.That(antiNodeLocations.Count, Is.EqualTo(expectedResult));
        
        bool SaveAntiNode(Coordinate coordinate)
        {
            antiNodeLocations.Add(coordinate);
            return true;
        }
    }

    private static List<Node> ParseNodes(string[] lines)
    {
        List<Node> nodes = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                var value = lines[y][x];
                if (char.IsAsciiLetterOrDigit(value))
                {
                    nodes.Add(new Node(value, new Coordinate(y, x)));
                }
            }
        }

        return nodes;
    }

    record Node(char Frequency, Coordinate Coordinate);

    record Coordinate(int Y, int X);
}