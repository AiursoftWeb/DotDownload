using Aiursoft.AiurProtocol;
using Aiursoft.Download.TrackerServer.Sdk.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Aiursoft.Download.TrackerServer.Sdk
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
