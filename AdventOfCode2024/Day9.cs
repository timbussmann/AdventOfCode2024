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
    
    [TestCase("Day9.Test.txt", 2858)]
    [TestCase("Day9.Input.txt", 6415163624282)]
    public void Part2(string filename, long expectedResult)
    {
        var input = File.ReadAllText(filename);
        if (input.Length % 2 != 0)
        {
            input += "0";
        }
        var chunked = input.Chunk(2);
        LinkedList<Range> diskContent = new LinkedList<Range>();
        int blockID = 0;
        int index = 0;
        foreach (var chunk in chunked)
        {
            var fileLenth = int.Parse(chunk[0].ToString());
            diskContent.AddLast(new Range(index, fileLenth, blockID));
            blockID++;
            index += fileLenth;
            var freeBlockLength = int.Parse(chunk[1].ToString());
            diskContent.AddLast(new Range(index, freeBlockLength, -1));
            index += freeBlockLength;
        }

        var currentNode = diskContent.Last;
        while (currentNode != null)
        {
            if (currentNode.Value.value == -1)
            {
                //free space, skip
                currentNode = currentNode.Previous;
            }
            else
            {
                LinkedListNode<Range>? potentialBlock = diskContent.First;
                bool moved = false;
                while (potentialBlock != null)
                {
                    if (potentialBlock.Value.value == -1 && potentialBlock.Value.length >= currentNode.Value.length && potentialBlock.Value.startIndex < currentNode.Value.startIndex)
                    {
                        // free space
                        diskContent.AddBefore(potentialBlock, new Range(potentialBlock.Value.startIndex, currentNode.Value.length, currentNode.Value.value));
                        diskContent.AddBefore(potentialBlock, new Range(potentialBlock.Value.startIndex + currentNode.Value.length, potentialBlock.Value.length - currentNode.Value.length, -1));
                        diskContent.Remove(potentialBlock);
                        //TODO: probably have to combine separate -1 sections again
                        diskContent.AddAfter(currentNode, currentNode.Value with { value = -1 });
                        var next = currentNode.Previous;
                        diskContent.Remove(currentNode);
                        currentNode = next;
                        moved = true;
                        break;
                    }

                    potentialBlock = potentialBlock.Next;
                }

                if (!moved)
                {
                    currentNode = currentNode.Previous;
                }

            }
        }

        var flattenedContent = diskContent.SelectMany(range => Enumerable.Repeat(range.value, range.length)).ToList();
        TestContext.Out.WriteLine(string.Concat(flattenedContent.Select(x => x.ToString())).Replace("-1", "."));
        long result = flattenedContent.Select((value, index) => (long)value * index).Where(x => x > 0).Sum();
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    record Range(int startIndex, int length, int value);
}