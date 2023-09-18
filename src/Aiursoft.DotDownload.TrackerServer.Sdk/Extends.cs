using Aiursoft.AiurProtocol;
using Aiursoft.DotDownload.TrackerServer.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.DotDownload.TrackerServer.Sdk
{
    public static class Extends
    {
        public static IServiceCollection AddTrackerAccess(this IServiceCollection services)
        {
            services.AddAiurProtocolClient();
            services.AddScoped<TrackerAccess>();
            return services;
        }
    }
}
