namespace AdventOfCode2024;

public class Day8
{
    [TestCase("Day8.Test.txt", 14)]
    [TestCase("Day8.Input.txt", 409)]
    public void Part1(string fileName, int expectedResult)
    {
        var lines = File.ReadAllLines(fileName);
        List<Node> nodes = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                var value = lines[y][x];
                if (char.IsAsciiLetterOrDigit(value))
                {
                    nodes.Add(new Node(value, y, x));
                }
            }
        }

        var nodesByFrequency = nodes.ToLookup(n => n.Frequency);

        HashSet<(int y, int x)> antiNodeLocations = new ();
        foreach (var node in nodes)
        {
            var siblingNodes = nodesByFrequency[node.Frequency].Where(n => n != node);
            foreach (var sibling in siblingNodes)
            {
                var distance = (y: sibling.Y - node.Y, x: sibling.X - node.X);
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
    
    record Node(char Frequency, int Y, int X);
}