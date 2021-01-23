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
using MyCDN.Web.DataAccess;
using MyCDN.Web.Services;

namespace MyCDN.Web.Controllers
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
            
            return File(file.OpenRead(), "text/css");
        }

        [HttpGet]
        [Route("Post")]
        public async Task<IActionResult> PostAsync()
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            var file = fileMock.Object;
            FormFile formFile = new FormFile(ms, 0, ms.Length, fileName, fileName);
            var blah = await _cdnService.UploadAsync(formFile);

            return NoContent();
        }
    }
}
