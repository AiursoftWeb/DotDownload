using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.DotDownload.TrackerServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

    private readonly Option<int> _serverPort =
        new(
            new[] { "-p", "--port" },
            () => 8100,
            "Tracker server port to host.")
        {
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
            _tracker,
            _serverPort);
    }

    public override Option[] GetCommandOptions() => new Option[]
    {
        _url,
        _savePath,
        _tracker,
        _serverPort
    };

    private async Task Execute(bool verbose, string url, string savePath, string tracker, int port)
    {
        var host = WebTools.Extends.App<Startup>(Array.Empty<string>(), port);
        await host.StartAsync();

        var scope = host.Services.CreateScope();
        var downloader = scope.ServiceProvider.GetRequiredService<P2pDownloader>();
        await downloader.DownloadAsync(url, tracker, savePath, showProgressBar: !verbose);

        await host.WaitForShutdownAsync();
    }
}