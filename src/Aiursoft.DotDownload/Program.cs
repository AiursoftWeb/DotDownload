using Aiursoft.DotDownload.PluginFramework;
using Aiursoft.CommandFramework;
using Aiursoft.CommandFramework.Extensions;
using Aiursoft.DotDownload.Http.Handlers.Download;

var command = new DownloadHandler().BuildAsCommand();

return await new AiursoftCommandApp(command)
    .RunAsync(args.WithDefaultTo(OptionsProvider.Url));