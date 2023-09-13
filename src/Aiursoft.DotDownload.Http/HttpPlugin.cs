using Aiursoft.CommandFramework.Abstracts;
using Aiursoft.CommandFramework.Framework;
using Anduin.HappyRecorder.Calendar.Handlers.Download;

namespace Anduin.HappyRecorder.Calendar;

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