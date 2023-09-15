﻿using Aiursoft.AiurProtocol;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Aiursoft.Scanner.Abstractions;

namespace Aiursoft.Download.TrackerServer.Sdk.Services
{
    public class TrackerAccess : IScopedDependency
    {
        private readonly AiurProtocolClient aiurProtocol;

        public TrackerAccess(AiurProtocolClient aiurProtocol)
        {
            this.aiurProtocol = aiurProtocol;
        }

        public async Task<ServerInfo> ServerInfoAsync(string endpoint)
        {
            var url = new AiurApiEndpoint(host: endpoint, "api", "info", param: new { });
            var result = await aiurProtocol.Get<ServerInfo>(url);
            return result;
        }
    }
}