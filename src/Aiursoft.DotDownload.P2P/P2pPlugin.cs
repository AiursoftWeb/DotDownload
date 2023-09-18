using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Framework;
using Aiursoft.DotDownload.P2p.Handlers.Download;

namespace Aiursoft.DotDownload.P2p;

public class P2pPlugin : IPlugin
{
    public CommandHandler[] Install()
    {
        return new CommandHandler[]
        {
            new P2pHandler(),
            new ServeHandler()
        };
    }
}