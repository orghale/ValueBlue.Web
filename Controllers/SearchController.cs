using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Threading.Tasks;
using ValueBlue.Web.BusinessLogic.Interface;
using ValueBlue.Web.Domain;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private readonly ISearchMovie _callserv;

        public SearchController(ISearchMovie callserv)
        {
            _callserv = callserv;
        }

        [HttpGet]
        [Route("{title}")]
        [ProducesResponseType(typeof(ApiResponseEntity), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var processId = $"{Guid.NewGuid()}";

            ApiResponseEntity result = new ApiResponseEntity();

            try
            {
                string ip = Request.Host.Host;

                log.Info($"[{Request.Path.Value}] - Get movie by title request:: title:{title}/ processId:{processId}");
                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest(ConstMessage.BAD_REQUEST);

                result = await _callserv.GetMovieFromOmdbApi(title, ip, processId);

                if (result is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (result.Response != "True")
                    return NotFound(result.Response);
            }
            catch (Exception ex)
            {
                log.Error($"Get movie by title internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }
            log.Info($"Get movie by title response:: title:{title}/ processId:{processId}");

            return Ok(result);
        }


    }
}
