namespace AdventOfCode2024.Day16;

public class Day16
{
    static readonly Coordinate North = new(-1, 0);
    static readonly Coordinate South = new(1, 0);
    static readonly Coordinate West = new(0, -1);
    static readonly Coordinate East = new(0, 1);
    static readonly Coordinate[] Directions = { North, East, South, West };

    [TestCase("Day16.Test1.txt", 7036)]
    [TestCase("Day16.Test2.txt", 11048)]
    [TestCase("Day16.Input.txt", 78428)]
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

        var openPaths = new Dictionary<(Coordinate coord, Coordinate direction), long>();
        var visited = new Dictionary<(Coordinate coord, Coordinate direction), long>();
        
        var start = grid.Single(kvp => kvp.Value == 'S').Key;
        openPaths.Add((start, East), 0);

        while (openPaths.Count > 0)
        {
            var cheapest = openPaths.OrderBy(t => t.Value).First();
            
            if (grid[cheapest.Key.coord] == 'E')
            {
                Assert.That(cheapest.Value, Is.EqualTo(expectedResult));
                return;
            }
            
            foreach (var direction in Directions.Except([OppositeDirection(cheapest.Key.direction)]))
            {
                var neighbor = cheapest.Key.coord + direction;
                
                if (visited.ContainsKey((neighbor, direction)))
                {
                    continue;
                }
                
                var stepCost = direction == cheapest.Key.direction ? 1 : 1001;
                var totalCost = cheapest.Value + stepCost;
                
                if (grid.TryGetValue(neighbor, out var value) && (value == '.' || value == 'E'))
                {
                    if (openPaths.TryGetValue((neighbor, direction), out var existingOpenPath))
                    {
                        if (totalCost < existingOpenPath)
                        {
                            openPaths[(neighbor, direction)] = totalCost;
                        }
                    }
                    else
                    {
                        openPaths[(neighbor, direction)] = totalCost;
                    }
                }
            }
            
            visited.Add(cheapest.Key, cheapest.Value);
            openPaths.Remove(cheapest.Key);
        }
        
        Assert.Fail("no path found");
    }

    static Coordinate OppositeDirection(Coordinate direction) =>
        direction switch
        {
            var d when d == North => South,
            var d when d == South => North,
            var d when d == East => West,
            var d when d == West => East,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };
}