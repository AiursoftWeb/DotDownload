using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Framework;
using Aiursoft.DotDownload.Calendar.Handlers.Download;

namespace Aiursoft.DotDownload.Calendar;

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