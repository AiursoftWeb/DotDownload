using Aiursoft.AiurProtocol;
using Aiursoft.AiurProtocol.Server;
using Aiursoft.Download.TrackerServer.Sdk;
using Aiursoft.Download.TrackerServer.Sdk.Models;
using Aiursoft.Download.TrackerServer.Sdk.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Aiursoft.Download.TrackerServer.Controllers;

[Route("api")]
public class ApiController : Controller
{
    private readonly ILogger<ApiController> logger;
    private readonly TrackerAccess trackerAccess;
    private readonly IConfiguration configuration;

    public ApiController(
        ILogger<ApiController> logger,
        TrackerAccess trackerAccess,
        IConfiguration configuration)
    {
        this.logger = logger;
        this.trackerAccess = trackerAccess;
        this.configuration = configuration;
    }

    [Route("info")]
    public IActionResult Info()
    {
        return this.Protocol(new ServerInfo
        {
            Code = Code.ResultShown,
            Message = "Basic information shown for this server.",
            Endpoint = this.configuration["Endpoint"],
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm]RegisterAddressModel model)
    {
        logger.LogInformation($"A server is requesting to register here. It's endpoint is {model.MyEndpoint}. Requesting his info..");

        var hisInfo = await trackerAccess.ServerInfoAsync(model.MyEndpoint);
        
        // TODO.
        return this.Protocol(Code.JobDone, "Successfully registered!");
    }
}
