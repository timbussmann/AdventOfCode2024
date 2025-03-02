using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day13;

public partial class Day13
{
    [TestCase("Day13.Test.txt", 480)]
    [TestCase("Day13.Input.txt", 29187)]
    public void Part1(string filename, int requiredTokens)
    {
        var machineDescriptions = ParseMachineDescriptions(filename, 1);
        var totalRequiredTokens = machineDescriptions.Sum(CalculateRequiredTokens);
        Assert.That(totalRequiredTokens, Is.EqualTo(requiredTokens));
    }

    static List<MachineDescription> ParseMachineDescriptions(string filename, long prizeMultiplier)
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
                long.Parse(machineValues.Groups["PX"].Value) * prizeMultiplier,
                long.Parse(machineValues.Groups["PY"].Value) * prizeMultiplier))
            .ToList();
        return machineDescriptions;
    }

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
        for (int i = 1; i <= 100; i++)
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

    record MachineDescription(long ButtonA_XDelta, long ButtonA_YDelta, long ButtonB_XDelta, long ButtonB_YDelta, long PrizeX, long PrizeY);

    [GeneratedRegex("""
                              Button A: X\+(?<AX>\d+), Y\+(?<AY>\d+)
                              Button B: X\+(?<BX>\d+), Y\+(?<BY>\d+)
                              Prize: X=(?<PX>\d+), Y=(?<PY>\d+)
                              """)]
    private static partial Regex MachineDescriptionRegex();
}