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

namespace CDNSharp.Web.Controllers
{
    [ApiController]
    [Route("api")]
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
        public async Task<IActionResult> Get()
        {
            var files = _cdnService.GetAllFiles(0, 100);
            return Ok(files);
        }
        [HttpGet]
        [Route("/{id}")]
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
        public async Task<FileStreamResult> Download(string filename)
        {
            var file = await _cdnService.DownloadAsync(filename);
            return new FileStreamResult(file.OpenRead(), file.MimeType);
        }

        [HttpPost]
        [Route("{fileName}/{version}")]
        public async Task<IActionResult> PostAsync(IFormFile file, string fileName, string version)
        {
            var fileInfo = await _cdnService.UploadAsync(file, fileName, version);

            return Created($"{Request.Scheme}://{Request.Host}{Request.PathBase}/Download/{fileInfo.Id}", fileInfo.Id);
        }
    }
}
