using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.CommandFramework.Services;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace Aiursoft.DotDownload.Calendar.Handlers.Download;

public class DownloadHandler : CommandHandler
{
    public override string Name => "download";

    public override string Description => "Download an HTTP Url.";

    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(Execute, CommonOptionsProvider.VerboseOption);
    }

    private async Task Execute(bool verbose)
    {
        var services = ServiceBuilder
            .BuildServices<Startup>(verbose)
            .BuildServiceProvider();
    }
}
