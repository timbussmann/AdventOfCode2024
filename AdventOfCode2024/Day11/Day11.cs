namespace AdventOfCode2024.Day11;

public class Day11
{
    [TestCase("125 17", 55312)]
    [TestCase("5178527 8525 22 376299 3 69312 0 275", 189547)]
    public void Part1(string stonesInput, long expected)
    {
        LinkedList<long> stones = new(stonesInput.Split(" ").Select(x1 => long.Parse(x1)));
        for (int i = 0; i < 25; i++)
        {
            var currentStone = stones.First;
            while (currentStone != null)
            {
                if (currentStone.Value == 0)
                {
                    currentStone.Value = 1;
                }
                else if (currentStone.Value.ToString().Length % 2 == 0)
                {
                    var stoneString = currentStone.Value.ToString();
                    var length = stoneString.Length / 2;
                    var left = stoneString.Substring(0, length);
                    var right = stoneString.Substring(length, length);
                    stones.AddBefore(currentStone, new LinkedListNode<long>(long.Parse(left)));
                    currentStone.Value = long.Parse(right);
                } else
                {
                    currentStone.Value *= 2024;
                }
                
                currentStone = currentStone.Next;
            }
        }
        Assert.That(stones.Count, Is.EqualTo(expected));
    }
}