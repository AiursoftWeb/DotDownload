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

    protected override async Task Execute(InvocationContext context)
    {
        var verbose = context.ParseResult.GetValueForOption(CommonOptionsProvider.VerboseOption);
        var url = context.ParseResult.GetValueForOption(OptionsProvider.Url)!;
        var savePath = context.ParseResult.GetValueForOption(OptionsProvider.SavePath)!;
        var threads = context.ParseResult.GetValueForOption(OptionsProvider.Threads);
        var blockSize = context.ParseResult.GetValueForOption(OptionsProvider.BlockSize);
        
        var host = ServiceBuilder
            .CreateCommandHostBuilder<Startup>(verbose)
            .Build();

        var downloader = host.Services.GetRequiredService<Downloader>();
        await downloader.DownloadWithWatchAsync(url, savePath, blockSize, threads, showProgressBar: !verbose);
    }
}