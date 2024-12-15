namespace AdventOfCode2024;

public class Day6
{
    [TestCase("Day6.Test.txt", 41)]
    [TestCase("Day6.Input.txt", 5534)]
    public void Part1(string filename, int expected)
    {
        char[][] map = File.ReadAllLines(filename).Select(line => line.ToCharArray()).ToArray();
        
        var guardPosition = FindGuardPosition(map);
        var rowLength = map[0].Length;
        var direction = (yd: -1, xd: 0);
        while (guardPosition.x >= 0 && guardPosition.x < rowLength && guardPosition.y >= 0 && guardPosition.y < map.Length)
        {
            map[guardPosition.y][guardPosition.x] = 'x';
            
            var nextPosition = (y: guardPosition.y + direction.yd, x: guardPosition.x + direction.xd);
            if (nextPosition.y < 0 || nextPosition.y >= map.Length || nextPosition.x < 0 || nextPosition.x >= rowLength)
            {
                break;
            }
            
            if (map[nextPosition.y][nextPosition.x] == '#')
            {
                // rotate
                direction = direction switch
                {
                    (1, 0) => (0, -1),
                    (0, -1) => (-1, 0),
                    (-1, 0) => (0, 1),
                    (0, 1) => (1, 0)
                };
            }
            else
            {
                guardPosition = nextPosition;

            }
        }
        PrintMap(map);
        var result = map.Select(row => row.Count(c => c == 'x')).Sum();
        Assert.That(result, Is.EqualTo(expected));
    }

    static (int y, int x) FindGuardPosition(char[][] map)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[0].Length; x++)
            {
                if (map[y][x] == '^')
                {
                    return (y, x);
                }
            }
        }

        throw new ArgumentException();
    }

    static void PrintMap(char[][] map)
    {
        foreach (var row in map)
        {
            Console.WriteLine(string.Join("", row));
        }
    }
}