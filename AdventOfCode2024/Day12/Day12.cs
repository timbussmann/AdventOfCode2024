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
        
        ForEachNeighbour((y, x), n =>
        {
            MapArea(n.y, n.x, map, grid, currentArea);
        });
    }

    static void ForEachNeighbour((int y, int x) current, Action<(int y, int x)> action)
    {
        action((current.y - 1, current.x));
        action((current.y + 1, current.x));
        action((current.y, current.x - 1));
        action((current.y, current.x + 1));
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
                ForEachNeighbour(plot, n =>
                {
                    if (map.GetValueOrDefault(n) != this)
                    {
                        perimeter++;
                    }
                });
                
            }

            return perimeter;
        }
    }
}