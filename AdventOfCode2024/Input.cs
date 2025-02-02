namespace AdventOfCode2024;

public static class Input
{
    public static string GetFilePath(string fileName, [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
    {
        var folderName = Path.GetDirectoryName(sourceFilePath);
        return Path.Combine(folderName, fileName);
    }
}