using Aiursoft.AiurProtocol;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Microsoft.Extensions.Logging;

namespace Aiursoft.Download.TrackerServer.Sdk.Services
{
    public class TrackerAccess
    {
        private readonly ILogger<TrackerAccess> logger;
        private readonly AiurProtocolClient aiurProtocol;

        public TrackerAccess(
            ILogger<TrackerAccess> logger,  
            AiurProtocolClient aiurProtocol)
        {
            this.logger = logger;
            this.aiurProtocol = aiurProtocol;
        }

        public async Task<AiurResponse> ServerInfoAsync(string endpoint)
        {
            var url = new AiurApiEndpoint(host: endpoint, "api", "info", param: new { });
            var result = await aiurProtocol.Get<AiurResponse>(url);
            return result;
        }

        public async Task<AiurResponse> RegisterOnRemoteAsync(string remoteServer, string localEndpoint)
        {
            var url = new AiurApiEndpoint(host: remoteServer, "api", "register", param: new { });
            var payload = new AiurApiPayload(param: new RegisterAddressModel 
            {
                MyEndpoint = localEndpoint
            });
            logger.LogInformation($"Registering on remote: {remoteServer}...");
            return await aiurProtocol.Post<AiurResponse>(url, payload);
        }
    }
}
