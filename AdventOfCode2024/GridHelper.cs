using System.Text;

namespace AdventOfCode2024;

public record struct Coordinate(int Y, int X);


public static class GridHelper
{
    public static void RenderGrid<T>(Dictionary<Coordinate, T?> grid)
    {
        var yMax = grid.Keys.Max(k => k.Y);
        var xMax = grid.Keys.Max(k => k.X);
        StringBuilder sb = new();
        for (var y = 0; y <= yMax; y++)
        {
            for (var x = 0; x <= xMax; x++)
            {
                sb.Append(grid.GetValueOrDefault(new Coordinate(y, x), default)?.ToString() ?? " ");
            }

            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }
}