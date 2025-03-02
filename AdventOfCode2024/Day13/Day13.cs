using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day13;

public partial class Day13
{
    [TestCase("Day13.Test.txt", 480)]
    [TestCase("Day13.Input.txt", 29187)]
    public void Part1(string filename, long requiredTokens)
    {
        var machineDescriptions = ParseMachineDescriptions(filename, 0);
        var totalRequiredTokens = machineDescriptions.Sum(CalculateRequiredTokens);
        Assert.That(totalRequiredTokens, Is.EqualTo(requiredTokens));
    }
    
    [TestCase("Day13.Input.txt", 99968222587852)]
    public void Part2(string filename, long requiredTokens)
    {
        var machineDescriptions = ParseMachineDescriptions(filename, 10000000000000);
        var totalRequiredTokens = machineDescriptions.Sum(CalculateRequiredTokens2);
        Assert.That(totalRequiredTokens, Is.EqualTo(requiredTokens));
    }

    /// <summary>
    /// Better solution working for both part 1 and 2 using Cramer's rule (https://en.wikipedia.org/wiki/Cramer%27s_rule).
    /// </summary>
    private static long CalculateRequiredTokens2(MachineDescription machineDescription)
    {
        
        long ButtonA_presses = ((machineDescription.PrizeX * machineDescription.ButtonB_YDelta) -
                               (machineDescription.ButtonB_XDelta * machineDescription.PrizeY)) /
                              ((machineDescription.ButtonA_XDelta * machineDescription.ButtonB_YDelta) -
                               (machineDescription.ButtonB_XDelta * machineDescription.ButtonA_YDelta));
        
        long ButtonB_presses = ((machineDescription.ButtonA_XDelta * machineDescription.PrizeY) -
                               (machineDescription.PrizeX * machineDescription.ButtonA_YDelta)) /
                              ((machineDescription.ButtonA_XDelta * machineDescription.ButtonB_YDelta) -
                               (machineDescription.ButtonB_XDelta * machineDescription.ButtonA_YDelta));

        long calculatedPrizeX = ButtonA_presses * machineDescription.ButtonA_XDelta + ButtonB_presses * machineDescription.ButtonB_XDelta;
        long calculatedPrizeY = ButtonA_presses * machineDescription.ButtonA_YDelta + ButtonB_presses * machineDescription.ButtonB_YDelta;
        if (ButtonA_presses >= 0 
            && ButtonB_presses >= 0 
            && calculatedPrizeX == machineDescription.PrizeX
            && calculatedPrizeY == machineDescription.PrizeY)
        {
            return ButtonA_presses * 3 + ButtonB_presses;
        }

        return 0;
    }

    /// <summary>
    /// Original approach for part 1 which doesn't scale for part 2
    /// </summary>
    private static long CalculateRequiredTokens(MachineDescription machineDescription)
    {
        var validXPresses = ButtonCombinations(machineDescription.PrizeX, machineDescription.ButtonA_XDelta, machineDescription.ButtonB_XDelta);
        var validYPresses = ButtonCombinations(machineDescription.PrizeY, machineDescription.ButtonA_YDelta, machineDescription.ButtonB_YDelta);
        var validCombinations = validXPresses.Intersect(validYPresses).ToList();
        return validCombinations.Count == 0
            ? 0
            : validCombinations.Min(combination => combination.buttonA * 3 + combination.buttonB);
    }

    private static List<(long buttonA, long buttonB)> ButtonCombinations(long prizeLocation, long buttonADelta, long buttonBDelta)
    {
        List<(long buttonA, long buttonB)> validXPresses = new();
        long iterations = prizeLocation / Math.Min(buttonADelta, buttonBDelta);
        for (int i = 1; i <= iterations; i++)
        {
            var left = prizeLocation - buttonADelta * i;
            if (left < 0)
            {
                break;
            }

            if (left % buttonBDelta == 0)
            {
                validXPresses.Add((i, left / buttonBDelta));
            }
        }

        return validXPresses;
    }

    static List<MachineDescription> ParseMachineDescriptions(string filename, long prizeOffset)
    {
        var lines = File.ReadAllLines(Input.GetFilePath(filename));
        var machineChunks = lines.Chunk(4);
        var regex = MachineDescriptionRegex();
        var machineDescriptions = machineChunks
            .Select(machineChunk => regex.Match(string.Join(Environment.NewLine, machineChunk)))
            .Select(machineValues => new MachineDescription(
                long.Parse(machineValues.Groups["AX"].Value),
                long.Parse(machineValues.Groups["AY"].Value),
                long.Parse(machineValues.Groups["BX"].Value),
                long.Parse(machineValues.Groups["BY"].Value),
                long.Parse(machineValues.Groups["PX"].Value) + prizeOffset,
                long.Parse(machineValues.Groups["PY"].Value) + prizeOffset))
            .ToList();
        return machineDescriptions;
    }

    record MachineDescription(long ButtonA_XDelta, long ButtonA_YDelta, long ButtonB_XDelta, long ButtonB_YDelta, long PrizeX, long PrizeY);

    [GeneratedRegex("""
                    Button A: X\+(?<AX>\d+), Y\+(?<AY>\d+)
                    Button B: X\+(?<BX>\d+), Y\+(?<BY>\d+)
                    Prize: X=(?<PX>\d+), Y=(?<PY>\d+)
                    """)]
    private static partial Regex MachineDescriptionRegex();
}