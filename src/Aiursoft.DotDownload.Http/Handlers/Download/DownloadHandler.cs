using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using System.CommandLine;

namespace Anduin.HappyRecorder.Calendar.Handlers.Download;

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
        Console.WriteLine("Hello world!");
    }
}
