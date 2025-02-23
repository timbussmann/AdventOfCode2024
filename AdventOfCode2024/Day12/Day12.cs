namespace AdventOfCode2024.Day12;

public class Day12
{
    [TestCase("Day12.Test.txt", 1930)]
    [TestCase("Day12.Input.txt", 1371306)]
    public void Part1(string filename, int expectedResult)
    {
        Dictionary<(int y, int x), char> grid = GridMap(File.ReadAllLines(Input.GetFilePath(filename)), c => c);
        Dictionary<(int y, int x), Area> areasMap = new();
        foreach (var coords in grid)
        {
            MapArea(coords.Key, coords.Value, areasMap, grid, new Area { Type = coords.Value });
        }
        
        var result = areasMap.Values.Distinct().Sum(area => area.Plots.Count * area.CalculatePerimeter(areasMap));
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("Day12.Test.txt", 1206)]
    [TestCase("Day12.Part2Example1.txt", 236)]
    [TestCase("Day12.Part2Example2.txt", 368)]
    [TestCase("Day12.Input.txt", 805880)]
    public void Part2(string filename, int expectedResult)
    {
        Dictionary<(int y, int x), char> grid = GridMap(File.ReadAllLines(Input.GetFilePath(filename)), c => c);
        Dictionary<(int y, int x), Area> areasMap = new();
        foreach (var coords in grid)
        {
            MapArea(coords.Key, coords.Value, areasMap, grid, new Area { Type = coords.Value });
        }
        
        var result = areasMap.Values.Distinct().Sum(area => area.Plots.Count * area.CalculateSides(areasMap));
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    void MapArea((int y, int x) coords, char coordsType, Dictionary<(int y, int x), Area> map, Dictionary<(int y, int x), char> grid, Area currentArea)
    {
        if (!map.TryGetValue(coords, out _) && coordsType == currentArea.Type)
        {
            currentArea.Plots.Add(coords);
            map.Add(coords, currentArea);
        }
        else
        {
            return;
        }
        
        ForEachNeighbour(coords, (n, _) =>
        {
            if (grid.TryGetValue(n, out char c))
            {
                MapArea(n, c, map, grid, currentArea);
            }
        });
    }

    static void ForEachNeighbour((int y, int x) current, Action<(int y, int x), Direction> action)
    {
        action((current.y - 1, current.x), Direction.Up);
        action((current.y + 1, current.x), Direction.Down);
        action((current.y, current.x - 1), Direction.Left);
        action((current.y, current.x + 1), Direction.Right);
    }

    enum Direction
    {
        Up, Down, Left, Right
    }

    class Area
    {
        public char Type { get; set; }

        public List<(int y, int x)> Plots { get; } = new();

        public int CalculatePerimeter(Dictionary<(int y, int x), Area> map)
        {
            int perimeter = 0;
            foreach (var plot in Plots)
            {
                ForEachNeighbour(plot, (n, _) =>
                {
                    if (map.GetValueOrDefault(n) != this)
                    {
                        perimeter++;
                    }
                });
                
            }

            return perimeter;
        }
        
        public int CalculateSides(Dictionary<(int y, int x), Area> map)
        {
            List<(Direction direction, (int y, int x) plot)> connectedArea = new();
            foreach (var plot in Plots)
            {
                ForEachNeighbour(plot, (n, d) =>
                {
                    var neighbourArea = map.GetValueOrDefault(n);
                    if (neighbourArea != this)
                    {
                        connectedArea.Add((d, plot));
                    }
                });
            }

            var counter = 0;
            foreach (var direction in connectedArea.GroupBy(tuple => tuple.direction))
            {
                var horizontalNeighbors = direction
                    .GroupBy(p => direction.Key is Direction.Up or Direction.Down ? p.plot.y : p.plot.x)
                    .Select(group => group
                        .Select(tuple => direction.Key is Direction.Up or Direction.Down ? tuple.plot.x : tuple.plot.y)
                        .OrderBy(x => x)
                        .ToList());
                
                foreach (var neighborsOnSameLine in horizontalNeighbors)
                {
                    counter++;
                    for (int i = 1; i < neighborsOnSameLine.Count; i++)
                    {
                        if (Math.Abs(neighborsOnSameLine[i] - neighborsOnSameLine[i-1]) > 1)
                        {
                            // if neighbors are not connected they belong to a different side
                            counter++;
                        }
                    }
                }
            }
            return counter;
        }
    }

    static Dictionary<(int y, int x), T> GridMap<T>(string[] input, Func<char, T> mapper)
    {
        var dic = new Dictionary<(int y, int x), T>();
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[0].Length; x++)
            {
                dic.Add((y, x), mapper(input[y][x]));
            }
        }

        return dic;
    }
}