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
        [Route("GetByTitle/{title}")]
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

                log.Info($"[{Request.Path.Value}] - Get movie by title request:: title:{title}/ processId:{processId}");
               
                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest(ConstMessage.BAD_REQUEST);
               
                string ip = Request.Host.Host;

                var response = await _callserv.GetMovieFromOmdbApi(title, ip, processId);

                if (response is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (response.entity.Response != "True")
                    return NotFound(response.entity.Response);

                result = response.entity;
            }
            catch (Exception ex)
            {
                log.Error($"Get movie by title internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }
            log.Info($"Get movie by title response:: title:{title}/ processId:{processId}");

            return Ok(result);
        }


        [HttpGet]
        [Route("GetMoviePoster/{title}")]
        [ProducesResponseType(typeof(ImageResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetMoviePoster(string title)
        {
            var processId = $"{Guid.NewGuid()}";

            var result = string.Empty;

            try
            {
                log.Info($"[{Request.Path.Value}] - Get movie poster image request:: Title:{title} processId:{processId}");

                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest(ConstMessage.BAD_REQUEST);

                string ip = Request.Host.Host;

                var res = await _callserv.GetMoviePoster(title, ip, processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    result = (string)res.Message;
                }
                else
                {
                    return NotFound((string)res.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Get movie poster image by title internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }
            log.Info($"Get movie poster image title response:: processId:{processId}");

            return Ok(new ImageResponse { Status = true, Image = result });
        }

    }
}
