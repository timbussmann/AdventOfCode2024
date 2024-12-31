namespace AdventOfCode2024;

public class Day9
{
    [TestCase("Day9.Test.txt", 1928)]
    [TestCase("Day9.Input.txt", 6385338159127)]
    public void Part1(string filename, long expectedResult)
    {
        var input = File.ReadAllText(filename);
        if (input.Length % 2 != 0)
        {
            input += "0";
        }
        var chunked = input.Chunk(2);
        var diskContent = new List<int>();
        int blockID = 0;
        foreach (var chunk in chunked)
        {
            diskContent.AddRange(Enumerable.Repeat(blockID++, int.Parse(chunk[0].ToString())));
            diskContent.AddRange(Enumerable.Repeat(-1, int.Parse(chunk[1].ToString())));
        }

        var freeBlocks = new Stack<int>(diskContent.Select((value, index) => (value: value, index: index))
            .Where(tuple => tuple.value == -1).Select(tuple => tuple.index).Reverse());
        for (int i = diskContent.Count - 1; i >= 0; i--)
        {
            if (diskContent[i] != -1)
            {
                var nextFreeBlockIndex = freeBlocks.Pop();
                if (nextFreeBlockIndex >= i)
                {
                    break;
                }
                
                diskContent[nextFreeBlockIndex] = diskContent[i];
                diskContent[i] = -1;
            }
        }

        TestContext.Out.WriteLine(string.Concat(diskContent));
        
        long result = diskContent.TakeWhile(x => x != -1).Select((value, index) => (long)value * index).Sum();
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
}