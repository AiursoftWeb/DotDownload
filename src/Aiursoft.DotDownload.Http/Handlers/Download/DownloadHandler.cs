using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.CommandFramework.Services;
using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.DependencyInjection;
using Aiursoft.DotDownload.Core.Services;
using Aiursoft.DotDownload.PluginFramework;

namespace Aiursoft.DotDownload.Http.Handlers.Download;

public class DownloadHandler : ExecutableCommandHandlerBuilder
{
    protected override string Name => "download";

    protected override string Description => "Download an HTTP Url.";

    protected override Option[] GetCommandOptions() =>
    [
        CommonOptionsProvider.VerboseOption,
        OptionsProvider.Url,
        OptionsProvider.SavePath,
        OptionsProvider.Threads,
        OptionsProvider.BlockSize
    ];

    protected override async Task Execute(ParseResult context)
    {
        var verbose = context.GetValue(CommonOptionsProvider.VerboseOption);
        var url = context.GetValue(OptionsProvider.Url)!;
        var savePath = context.GetValue(OptionsProvider.SavePath)!;
        var threads = context.GetValue(OptionsProvider.Threads);
        var blockSize = context.GetValue(OptionsProvider.BlockSize);

        var host = ServiceBuilder
            .CreateCommandHostBuilder<Startup>(verbose)
            .Build();

        var downloader = host.Services.GetRequiredService<Downloader>();
        await downloader.DownloadWithWatchAsync(url, savePath, blockSize, threads, showProgressBar: !verbose);
    }
}
