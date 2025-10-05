namespace AdventOfCode2024.Day16;

public class Day16
{
    static readonly Coordinate North = new Coordinate(-1, 0);
    static readonly Coordinate South = new Coordinate(1, 0);
    static readonly Coordinate West = new Coordinate(0, -1);
    static readonly Coordinate East = new Coordinate(0, 1);
    static readonly Coordinate[] Directions = { North, East, South, West };

    [TestCase("Day16.Test1.txt", 7036)]
    [TestCase("Day16.Test2.txt", 11048)]
    [TestCase("Day16.Input.txt", 78428)]
    public void Part1_V2(string filename, int expectedResult)
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
                var newCoord = cheapest.Key.coord + direction;
                if (visited.ContainsKey((newCoord, direction)))
                {
                    continue;
                }
                
                var stepCost = direction == cheapest.Key.direction ? 1 : 1001;
                var totalCost = cheapest.Value + stepCost;
                if (grid.TryGetValue(newCoord, out var value) && (value == '.' || value == 'E'))
                {
                    if (openPaths.TryGetValue((newCoord, direction), out var existingOpenPath))
                    {
                        if (totalCost < existingOpenPath)
                        {
                            openPaths[(newCoord, direction)] = totalCost;
                        }
                    }
                    else
                    {
                        openPaths[(newCoord, direction)] = totalCost;
                    }
                }
            }
            visited.Add(cheapest.Key, cheapest.Value);
            openPaths.Remove(cheapest.Key);
        }
        
        var v1 = visited.Select(kvp => kvp.Key.coord).ToHashSet().ToDictionary(e => e, _ => "x");
        GridHelper.RenderGrid(v1);
        Assert.Fail("no path found");
    }
    
    public static Coordinate OppositeDirection(Coordinate direction) =>
        direction switch
        {
            var d when d == North => South,
            var d when d == South => North,
            var d when d == East => West,
            var d when d == West => East,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction")
        };
    
    [TestCase("Day16.Test1.txt", 7036)]
    [TestCase("Day16.Test2.txt", 11048)]
    [TestCase("Day16.Input.txt", 11048)]
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
        

        //var openPaths = new List<((Coordinate coordinate, Coordinate facingDirection), long cost, List<Coordinate> visited)>();
        var openPaths = new Dictionary<(Coordinate coordinate, Coordinate facingDirection), (long cost, List<Coordinate> visited)>();
        var start = grid.Single(kvp => kvp.Value == 'S').Key;
        openPaths.Add((start, East), (0, new List<Coordinate>()));
        var completedPaths = new List<((Coordinate coordinate, Coordinate facingDirection), long cost, List<Coordinate> visited)>();
        //TODO bring visited up here and make it include facing direction
        while (openPaths.Any())
        {
            var lowestOpenPath = openPaths.OrderBy(kvp => kvp.Value.cost).First();
            if (grid.GetValueOrDefault(lowestOpenPath.Key.coordinate, '#') == 'E')
            {
                completedPaths.Add((lowestOpenPath.Key, lowestOpenPath.Value.cost, lowestOpenPath.Value.visited));
                openPaths.Remove(lowestOpenPath.Key);
                continue;
            }

            // try continue in same direction
            var aheadCoord = new Coordinate(lowestOpenPath.Key.coordinate.Y + lowestOpenPath.Key.facingDirection.Y, lowestOpenPath.Key.coordinate.X + lowestOpenPath.Key.facingDirection.X);
            if (!lowestOpenPath.Value.visited.Contains(aheadCoord))
            {
                char ahead = grid.GetValueOrDefault(aheadCoord, '#');
                if (ahead != '#')
                {
                    var visited2 = new List<Coordinate>(lowestOpenPath.Value.visited) { aheadCoord };
                    var cost = lowestOpenPath.Value.cost + 1;
                    if (openPaths.TryGetValue((aheadCoord, lowestOpenPath.Key.facingDirection), out var existingPath))
                    {
                        if (existingPath.cost > cost)
                        {
                            openPaths[(aheadCoord, lowestOpenPath.Key.facingDirection)] = (cost, visited2);
                        }

                    }
                    else
                    {
                        openPaths.Add((aheadCoord, lowestOpenPath.Key.facingDirection), (cost, visited2));
                    }
                }        
            }
                
            foreach (Coordinate direction in Directions.Except([lowestOpenPath.Key.facingDirection]))
            {
                var rotatedPosition = new Coordinate(lowestOpenPath.Key.coordinate.Y + direction.Y, lowestOpenPath.Key.coordinate.X + direction.X);
                var neighborPos = grid.GetValueOrDefault(rotatedPosition, '#');
                if (neighborPos != '#' && !lowestOpenPath.Value.visited.Contains(rotatedPosition))
                {
                    // not correct for the 180° turn but that is prevented since we're not moving back anyway.
                    var cost = lowestOpenPath.Value.cost + 1001;
                    var visited2 = new List<Coordinate>(lowestOpenPath.Value.visited) { rotatedPosition };

                    if (openPaths.TryGetValue((rotatedPosition, direction), out var existingPath))
                    {
                        if (existingPath.cost > cost)
                        {
                            openPaths[(rotatedPosition, direction)] = (cost, visited2);
                        }

                    }
                    else
                    {
                        openPaths.Add((rotatedPosition, direction), (cost, visited2));
                    }

                }
            }

            var remove = openPaths.Remove(lowestOpenPath.Key);
        }

        Console.WriteLine(completedPaths.Count);
        var lowestScore = completedPaths.Select(p => p.cost).Min();
        Assert.That(lowestScore, Is.EqualTo(expectedResult));
    }
}