namespace Aiursoft.DotDownload.PluginFramework.Handlers.Download;

internal static class PathExtensions
{
    internal static string GetFilePathToSave(string userInput, string url)
    {
        var fileToWrite = userInput;
        if (string.IsNullOrWhiteSpace(fileToWrite))
        {
            fileToWrite = "." + Path.DirectorySeparatorChar + Path.GetFileName(url);
        }
        fileToWrite = GetAbsolutePath(Directory.GetCurrentDirectory(), fileToWrite);
        return fileToWrite;
    }

    private static string GetAbsolutePath(string currentPath, string referencePath)
    {
        if (Path.IsPathRooted(referencePath)) return referencePath;
        referencePath = referencePath.Replace('\\', Path.DirectorySeparatorChar);
        return Path.GetFullPath(Path.Combine(currentPath, referencePath));
    }
}