using Aiursoft.AiurProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiursoft.Download.TrackerServer.Sdk.Models
{
    public class RegisterAddressModel
    {
        [Url]
        [Required]
        public required string MyEndpoint { get; init; }
    }
}
