using Aiursoft.AiurProtocol;

namespace Aiursoft.Download.TrackerServer.Sdk.Models;

public class ServerInfo : AiurResponse
{
    public string? Endpoint { get; set; }
    public List<KnownLink> KnownLinks { get; set; } = new List<KnownLink>();
}
