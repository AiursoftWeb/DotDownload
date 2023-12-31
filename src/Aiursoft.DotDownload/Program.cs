using Aiursoft.CommandFramework;
using Aiursoft.DotDownload.PluginFramework;
using Aiursoft.DotDownload.Http.Handlers.Download;

return await new SingleCommandApp<DownloadHandler>()
    .WithDefaultOption(OptionsProvider.Url)
    .RunAsync(args);
