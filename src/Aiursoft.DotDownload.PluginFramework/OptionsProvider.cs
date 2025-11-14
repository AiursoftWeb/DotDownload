using System.CommandLine;

namespace Aiursoft.DotDownload.PluginFramework;

public static class OptionsProvider
{
    public static readonly Option<string> Url =
        new(
            name: "--url",
            aliases: ["-u"])
        {
            Description = "The target url to download.",
            Required = true
        };

    public static readonly Option<string> SavePath =
        new(
            name: "--file",
            aliases: ["-f"])
        {
            DefaultValueFactory = _ => string.Empty,
            Description = "The output file path to save the download result."
        };

    public static readonly Option<int> Threads =
        new(
            name: "--threads",
            aliases: ["-t"])
        {
            DefaultValueFactory = _ => 16,
            Description = "Max threads allowed to connects to the download server."
        };

    public static readonly Option<int> BlockSize =
        new(
            name: "--block-size",
            aliases: ["-b"])
        {
            DefaultValueFactory = _ => 4 * 1024 * 1024,
            Description = "The size of block. Default is 4MB. For example, for 100MB file, it will be split to 25 blocks to download in parallel."
        };
}
