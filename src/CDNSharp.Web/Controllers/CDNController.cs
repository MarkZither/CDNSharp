using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using CDNSharp.Web.DataAccess;
using CDNSharp.Web.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CDNSharp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CDNController : ControllerBase
    {
        private readonly ILogger<CDNController> _logger;
        private readonly ICDNService _cdnService;

        public CDNController(ILogger<CDNController> logger, ICDNService cdnService)
        {
            _logger = logger;
            _cdnService = cdnService;
        }

        [HttpGet]
        [SwaggerOperation("GetPackages")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> Get()
        {
            var files = await Task.Run(() => _cdnService.GetAllFiles(0, 100)).ConfigureAwait(true);
            return Ok(files);
        }
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("GetPackagesByName")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetByName([FromRouteAttribute]string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NoContent();
            }

            var file = await _cdnService.DownloadAsync(id);
            
            return File(file.OpenRead(), file.MimeType);
        }

        [HttpGet]
        [Route("stream/{filename}")]
        [SwaggerOperation("DownloadPackage")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<FileStreamResult> Download(string filename)
        {
            var file = await _cdnService.DownloadAsync(filename);
            return new FileStreamResult(file.OpenRead(), file.MimeType);
        }

        [HttpPost]
        [Route("{fileName}/{version}")]
        [SwaggerOperation("UploadPackage")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PostAsync(IFormFile file, string fileName, string version)
        {
            var fileInfo = await _cdnService.UploadAsync(file, fileName, version);

            return Created($"{Request.Scheme}://{Request.Host}{Request.PathBase}/Download/{fileInfo.Id}", fileInfo.Id);
        }
    }
}
