using CDNSharp.Web.Models;
using CDNSharp.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Net;
using System.Threading;

namespace CDNSharp.Web.Controllers
{
    [Route("api/[controller]")]
    public class CDNFileInfoStringController : ControllerBase
    {
        private readonly ILogger<CDNFileInfoStringController> _logger;
        private readonly ICDNService _cdnService;

        public CDNFileInfoStringController(ILogger<CDNFileInfoStringController> logger, ICDNService cdnService)
        {
            _logger = logger;
            _cdnService = cdnService;
        }


        [HttpGet]
        [EnableQuery]
        [SwaggerOperation("GetFiles")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult Get()
        {
            var CDNFileInfos = _cdnService.GetAllFiles(0, 100000);
            var CDNFileInfoStrings = from file in CDNFileInfos
                                     select (new CDNFileInfoString() {Filename =  file.Filename, Id = file.Id, MimeType = file.MimeType});
            return Ok(CDNFileInfoStrings);
        }
    }
}
