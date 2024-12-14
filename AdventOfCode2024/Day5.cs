namespace AdventOfCode2024;

public class Day5
{
    [TestCase("Day5.Test.txt",  143)]
    [TestCase("Day5.Input.txt",  5948)]
    public void Part1(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(filename);
        var rules = lines.TakeWhile(l => l != string.Empty).Select(line =>
        {
            var segments = line.Split('|');
            return (segments[0], segments[1]);
        }).ToLookup(r => r.Item1, r => r.Item2);
        var updates = lines.SkipWhile(l => l != string.Empty).Skip(1)
            .Select(line => line.Split(',')).ToArray();

        var orderedInputs = updates.Where(u => IsInOrder(u, rules));
        var result = orderedInputs.Sum(line => int.Parse(line[line.Length/2]));
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [TestCase("Day5.Test.txt",  123)]
    [TestCase("Day5.Input.txt",  3062)]
    public void Part2(string filename, int expectedResult)
    {
        var lines = File.ReadAllLines(filename);
        var rules = lines.TakeWhile(l => l != string.Empty).Select(line =>
        {
            var segments = line.Split('|');
            return (segments[0], segments[1]);
        }).ToList();
        var comparer = Comparer<string>.Create((s1, s2) => rules.Contains((s1, s2)) ? -1 : 1);
        var updates = lines.SkipWhile(l => l != string.Empty).Skip(1)
            .Select(line => line.Split(',')).ToArray();

        var reOrdered = updates.Where(u => !u.SequenceEqual(u.Order(comparer))).Select(u => u.Order(comparer).ToArray());
        var result = reOrdered.Sum(line => int.Parse(line[line.Length/2]));
        
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    static bool IsInOrder(string[] update, ILookup<string, string> rules)
    {
        for (int i = 0; i < update.Length; i++)
        {
            var pagesThatMustbeAfterThisPage = rules[update[i]];
            if (update[..i].Any(previousPage => pagesThatMustbeAfterThisPage.Contains(previousPage)))
            {
                return false;
            }
        }

        return true;
    }
}