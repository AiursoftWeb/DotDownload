using Aiursoft.DotDownload.PluginFramework;
using Aiursoft.DotDownload.Http.Handlers.Download;

return await new DownloadHandler().RunAsync(args, defaultOption: OptionsProvider.Url);
