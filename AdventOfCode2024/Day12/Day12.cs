using System.ComponentModel.DataAnnotations.Schema;

namespace AdventOfCode2024.Day12;

public class Day12
{
    [TestCase("Day12.Test.txt", 1930)]
    [TestCase("Day12.Input.txt", 1371306)]
    public void Part1(string filename, int expectedResult)
    {
        var grid = File.ReadAllLines(Input.GetFilePath(filename));
        Dictionary<(int y, int x), Area> map = new();
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                MapArea(y, x, map, grid, new Area { Type = grid[y][x] });
            }
        }
        
        var result = map.Values.Distinct().Sum(area => area.Plots.Count * area.CalculatePerimeter(map));
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [TestCase("Day12.Test.txt", 1206)]
    [TestCase("Day12.Part2Example1.txt", 236)]
    [TestCase("Day12.Part2Example2.txt", 368)]
    [TestCase("Day12.Input.txt", 805880)]
    public void Part2(string filename, int expectedResult)
    {
        var grid = File.ReadAllLines(Input.GetFilePath(filename));
        Dictionary<(int y, int x), Area> map = new();
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[0].Length; x++)
            {
                MapArea(y, x, map, grid, new Area { Type = grid[y][x] });
            }
        }
        
        var result = map.Values.Distinct()
            .Select(area =>
            {
                Console.WriteLine($"Area: {area.Type} - plots: {area.Plots.Count}, sides: {area.CalculateSides(map)}");
                return area;
            })
            .Sum(area => area.Plots.Count * area.CalculateSides(map));
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    void MapArea(int y, int x, Dictionary<(int y, int x), Area> map, string[] grid, Area currentArea)
    {
        if (y < 0 || y >= grid.Length || x < 0 || x >= grid[0].Length)
        {
            return;
        }

        if (!map.TryGetValue((y,x), out _) && grid[y][x] == currentArea.Type)
        {
            currentArea.Plots.Add((y, x));
            map.Add((y, x), currentArea);
        }
        else
        {
            return;
        }
        
        ForEachNeighbour((y, x), (n, _) =>
        {
            MapArea(n.y, n.x, map, grid, currentArea);
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
            List<(Direction direction, (int y, int x) plot)> x = new();
            foreach (var plot in Plots)
            {
                ForEachNeighbour(plot, (n, d) =>
                {
                    var neighbourArea = map.GetValueOrDefault(n);
                    if (neighbourArea != this)
                    {
                        x.Add((d, plot));
                    }
                });
                
            }

            var counter = 0;
            var directions = x.GroupBy(tuple => tuple.direction);
            foreach (var direction in directions)
            {
                if (direction.Key == Direction.Up || direction.Key == Direction.Down)
                {
                    var sidesByY = direction.GroupBy(p => p.plot.y);
                    foreach (var yLine in sidesByY)
                    {
                        var sides = yLine.OrderBy(p => p.plot.x).ToList();
                        counter++;
                        for (int i = 1; i < sides.Count; i++)
                        {
                            if (Math.Abs(sides[i].plot.x - sides[i-1].plot.x) > 1)
                            {
                                counter++;
                            }
                        }
                    }
                }
                else
                {
                    var sidesByX = direction.GroupBy(p => p.plot.x);
                    foreach (var xLine in sidesByX)
                    {
                        var sides = xLine.OrderBy(p => p.plot.y).ToList();
                        counter++;
                        for (int i = 1; i < sides.Count; i++)
                        {
                            if (Math.Abs(sides[i].plot.y - sides[i-1].plot.y) > 1)
                            {
                                counter++;
                            }
                        }
                    }
                }
                
            }
            return counter;
        }
    }
}