using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.CommandFramework.Services;
using System.CommandLine;
using Aiursoft.DotDownload.PluginFramework.Handlers.Download;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.Http.Handlers.Download;

public class DownloadHandler : CommandHandler
{
    private readonly Option<string> _url =
        new(
            aliases: new[] { "-u", "--url" },
            description: "The target url to connect to download.")
        {
            IsRequired = true
        };

    private readonly Option<string> _savePath =
        new(
            new[] { "-f", "--file" },
            getDefaultValue: () => string.Empty,
            "The output file path to save the download result.");

    private readonly Option<int> _threads =
        new(
            new[] { "-t", "--threads" },
            getDefaultValue: () => 16,
            "How many threads to connect to download.");

    private readonly Option<int> _blockSize =
        new(
            new[] { "-b", "--block-size" },
            getDefaultValue: () => 4 * 1024 * 1024,
            "The size of block. Default is 4MB. For example, for 100MB file, it will be split to 25 blocks to download in parallel.");


    public override string Name => "download";

    public override string Description => "Download an HTTP Url.";

    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(
            Execute,
            CommonOptionsProvider.VerboseOption,
            CommonOptionsProvider.DryRunOption,
            _url,
            _savePath,
            _threads,
            _blockSize);
    }

    public override Option[] GetCommandOptions() => new Option[]
    {
        _url,
        _savePath,
        _threads,
        _blockSize
    };

    private async Task Execute(bool verbose, bool dryRun, string url, string savePath, int threads, int blockSize)
    {
        var host = ServiceBuilder
            .BuildHost<Startup>(verbose)
            .Build();

        await host.StartAsync();
        
        var downloader = host.Services.GetRequiredService<Downloader>();
        await downloader.DownloadAsync(url, savePath, blockSize, threads);
    }
}