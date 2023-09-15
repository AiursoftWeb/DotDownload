using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.CommandFramework.Services;
using Aiursoft.DotDownload.P2P;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace Aiursoft.DotDownload.P2p.Handlers.Download;

public class P2pHandler : CommandHandler
{
    private readonly Option<string> _url =
        new(
            aliases: new[] { "-u", "--url" },
            description: "The target url to download.")
        {
            IsRequired = true
        };

    private readonly Option<string> _savePath =
        new(
            new[] { "-f", "--file" },
            getDefaultValue: () => string.Empty,
            "The output file path to save the download result.");

    private readonly Option<string> _tracker =
        new(
            new[] { "-t", "--tracker" },
            "Tracker server to use.")
        {
            IsRequired = true
        };

    public override string Name => "download-p2p";

    public override string Description => "Download an HTTP Url with p2p, requires a tracker server.";

    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(
            Execute,
            CommonOptionsProvider.VerboseOption,
            _url,
            _savePath,
            _tracker);
    }

    public override Option[] GetCommandOptions() => new Option[]
    {
        _url,
        _savePath,
        _tracker,
    };

    private async Task Execute(bool verbose, string url, string savePath, string tracker)
    {
        var host = ServiceBuilder
        .BuildHost<Startup>(verbose)
        .Build();

        var downloader = host.Services.GetRequiredService<P2pDownloader>();
        await downloader.DownloadAsync(url, tracker, savePath, showProgressBar: !verbose);
    }
}