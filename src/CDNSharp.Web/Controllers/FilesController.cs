using CDNSharp.Web.DataAccess;
using CDNSharp.Web.Models;
using CDNSharp.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace CDNSharp.Web.Controllers
{
    [Route("api/[controller]")]
    public class CDNFileController : ControllerBase
    {
        private readonly ILogger<CDNFileController> _logger;
        private readonly ICDNService _cdnService;
        //private readonly MyDataContext _myDataContext;

        public CDNFileController(ILogger<CDNFileController> logger, ICDNService cdnService
            //, MyDataContext myDataContext
            )
        {
            _logger = logger;
            _cdnService = cdnService;
            //_myDataContext = myDataContext;
        }

        /// <summary>
        /// Gets a list of files in the CDN.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Files
        ///     GET /Files?filter=id eq 'filename'
        ///
        /// </remarks>
        /// <returns>A list of CDNFile</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpGet]
        [EnableQuery]
        [SwaggerOperation("GetFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get()
        {
            try
            {
                //var cdnFiles = _myDataContext.Files;
                var CDNFileInfos = _cdnService.GetAllFiles(0, 100000);
                var CDNFileInfoStrings = from file in CDNFileInfos
                                         select (new CDNFile() { Filename = file.Filename, Id = file.Id, MimeType = file.MimeType });
                if (CDNFileInfoStrings.Count().Equals(0))
                {
                    return NoContent();
                }
                    
                return Ok(CDNFileInfoStrings);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
