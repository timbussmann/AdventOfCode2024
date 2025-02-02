namespace AdventOfCode2024;

public class Day9
{
    [TestCase("Day9.Test.txt", 1928)]
    [TestCase("Day9.Input.txt", 6385338159127)]
    public void Part1(string filename, long expectedResult)
    {
        var input = File.ReadAllText(Input.GetFilePath(filename));
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

        var freeBlocks = new Stack<int>(diskContent
            .Select((value, index) => (value: value, index: index))
            .Where(tuple => tuple.value == -1)
            .Select(tuple => tuple.index)
            .Reverse());
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

        long result = diskContent.TakeWhile(x => x != -1).Select((value, index) => (long)value * index).Sum();
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [TestCase("Day9.Test.txt", 2858)]
    [TestCase("Day9.Input.txt", 6415163624282)]
    public void Part2(string filename, long expectedResult)
    {
        var diskSegments = ParseDiskSegments(Input.GetFilePath(filename));

        var currentBlock = diskSegments.Last!;
        while (currentBlock != null)
        {
            if (currentBlock.Value.IdNumber != -1)
            {
                currentBlock = RearrangeBlock(currentBlock);
            }

            currentBlock = currentBlock.Previous;
        }

        var diskLayout = diskSegments.SelectMany(range => Enumerable.Repeat(range.IdNumber, range.Length)).ToList();
        // TestContext.Out.WriteLine(string.Concat(diskLayout.Select(x => x.ToString())).Replace("-1", "."));
        long result = diskLayout.Select((value, index) => (long)value * index).Where(x => x > 0).Sum();
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    static LinkedList<Range> ParseDiskSegments(string filename)
    {
        var input = File.ReadAllText(filename);
        
        if (input.Length % 2 != 0)
        {
            // add filler for chunk to work if last segment is missing
            input += "0";
        }
        
        var chunked = input.Chunk(2);
        var diskContent = new LinkedList<Range>();
        var blockID = 0;
        foreach (var chunk in chunked)
        {
            diskContent.AddLast(new Range(int.Parse(chunk[0].ToString()), blockID));
            blockID++;
            diskContent.AddLast(new Range(int.Parse(chunk[1].ToString()), -1));
        }

        return diskContent;
    }

    static LinkedListNode<Range> RearrangeBlock(LinkedListNode<Range> currentBlock)
    {
        var diskContent = currentBlock.List;
        var potentialBlock = diskContent.First;
        while (potentialBlock != null && potentialBlock != currentBlock)
        {
            if (potentialBlock.Value.IdNumber == -1
                && potentialBlock.Value.Length >= currentBlock.Value.Length)
            {
                // split existing free block into used and left-over blocks:
                diskContent.AddBefore(potentialBlock, new Range(currentBlock.Value.Length, currentBlock.Value.IdNumber));
                diskContent.AddBefore(potentialBlock, new Range(potentialBlock.Value.Length - currentBlock.Value.Length, -1));
                diskContent.Remove(potentialBlock);
                
                // free original space of the moved block:
                // Note: This approach doesn't combine multiple free segments and would make them unavailable for larger blocks
                // due to the strict ordering, this isn't an issue as the freed-up spaces are only at the end and we never move blocks backwards
                currentBlock.Value = currentBlock.Value with { IdNumber = -1 };
                break;
            }

            potentialBlock = potentialBlock.Next;
        }

        return currentBlock;
    }

    record Range(int Length, int IdNumber);
}