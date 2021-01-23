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
    [Route("")]
    public class CDNController : ControllerBase
    {
        private readonly ILogger<CDNController> _logger;
        private readonly ICDNService _cdnService;

        public CDNController(ILogger<CDNController> logger, ICDNService cdnService)
        {
            _logger = logger;
            _cdnService = cdnService;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Content("Something to go here soon");
        }
        [HttpGet]
        [Route("/{filename}")]
        public async Task<IActionResult> GetByName([FromRouteAttribute]string filename)
        {
            if(string.IsNullOrEmpty(filename))
            {
                return NoContent();
            }

            var file = await _cdnService.DownloadAsync(filename);
            
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
        [Route("Post")]
        public async Task<IActionResult> PostAsync(IFormFile file, string version)
        {
            var fileInfo = await _cdnService.UploadAsync(file, version);

            return Created($"http://localhost:6115/Download/{fileInfo.Id}", fileInfo);
        }
    }
}
