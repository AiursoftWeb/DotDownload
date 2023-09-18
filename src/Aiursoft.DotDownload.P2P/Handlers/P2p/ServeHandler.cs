using Aiursoft.CommandFramework.Framework;
using Aiursoft.CommandFramework.Models;
using Aiursoft.DotDownload.TrackerServer;
using Aiursoft.WebTools;
using System.CommandLine;

namespace Aiursoft.DotDownload.P2p.Handlers.Download;

public class ServeHandler : CommandHandler
{
    public override string Name => "serve-tracker";

    public override string Description => "Serve as a P2P tracker server";

    private readonly Option<int> _serverPort =
    new(
        new[] { "-p", "--port" },
        () => 8100,
        "Tracker server port to host.")
    {
    };

    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(Execute, CommonOptionsProvider.VerboseOption, _serverPort);
    }

    public override Option[] GetCommandOptions() => new Option[]
    {
            _serverPort
    };

    private async Task Execute(bool verbose, int port)
    {
        var host = Extends.App<Startup>(Array.Empty<string>(), port);
        await host.RunAsync();
    }
}