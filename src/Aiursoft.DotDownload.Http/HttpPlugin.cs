using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Framework;
using Aiursoft.DotDownload.Http.Handlers.Download;

namespace Aiursoft.DotDownload.Http;

public class HttpPlugin : IPlugin
{
    public CommandHandler[] Install()
    {
        return new CommandHandler[]
        {
            new DownloadHandler()
        };
    }
}