using System.CommandLine;

namespace Aiursoft.DotDownload.PluginFramework;

public static class OptionsProvider
{
    public static readonly Option<string> Url =
        new(
            aliases: ["-u", "--url"],
            description: "The target url to download.")
        {
            IsRequired = true
        };

    public static readonly Option<string> SavePath =
        new(
            ["-f", "--file"],
            getDefaultValue: () => string.Empty,
            "The output file path to save the download result.");

    public static readonly Option<int> Threads =
        new(
            ["-t", "--threads"],
            getDefaultValue: () => 16,
            "Max threads allowed to connects to the download server.");

    public static readonly Option<int> BlockSize =
        new(
            ["-b", "--block-size"],
            getDefaultValue: () => 4 * 1024 * 1024,
            "The size of block. Default is 4MB. For example, for 100MB file, it will be split to 25 blocks to download in parallel.");

}
