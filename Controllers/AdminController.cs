using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.BusinessLogic.Interface;
using ValueBlue.Web.Domain;
using ValueBlue.Web.Extension;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthAttribute]
    public class AdminController : ControllerBase
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private readonly IFetchMovieDb _callserv;

        public AdminController(IFetchMovieDb callserv)
        {
            _callserv = callserv;
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(List<OmdbDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetAll()
        {
            var processId = $"{Guid.NewGuid()}";

            List<OmdbDto> result = new List<OmdbDto>();

            try
            {
                log.Info($"[{Request.Path.Value}] - Get all movies request:: processId:{processId}");

                var res = await _callserv.GetAllMovieFromDb(processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    if (res.ResponseObjects.Any())
                        foreach (var item in res.ResponseObjects)
                        {
                            var response = (OmdbDto)item;
                            result.Add(new OmdbDto
                            {
                                imdbID = response.imdbID,
                                ip_address = response.ip_address,
                                processing_time_ms = response.processing_time_ms,
                                search_token = response.search_token,
                                timestamp = response.timestamp,
                                _id = response._id
                            });
                        }
                    else
                        return NotFound(ConstMessage.NOT_FOUND);
                }
                else
                {
                    return NotFound((string)res.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Get all moviess internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }
            log.Info($"Get all movies response:: processId:{processId}");

            return Ok(result);
        }


        [HttpGet]
        [Route("GetByTitle/{title}")]
        [ProducesResponseType(typeof(OmdbDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var processId = $"{Guid.NewGuid()}";

            OmdbDto result = new OmdbDto();

            try
            {
                log.Info($"[{Request.Path.Value}] - Get movie by title request:: title:{title}/ processId:{processId}");
                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest(string.Format(ConstMessage.BAD_REQUEST, "title"));

                var res = await _callserv.GetMovieFromDb(title, processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    var response = (OmdbDto)res.ResponseObject;

                    if (response is null)
                        return NotFound(ConstMessage.NOT_FOUND);
                    else
                    {
                        result = new OmdbDto
                        {
                            imdbID = response.imdbID,
                            ip_address = response.ip_address,
                            processing_time_ms = response.processing_time_ms,
                            search_token = response.search_token,
                            timestamp = response.timestamp,
                            _id = response._id
                        };
                    }

                }
                else
                {
                    return NotFound((string)res.Message);
                }

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
        [Route("GetByDateRange/{startDate}/{endDate}")]
        [ProducesResponseType(typeof(List<OmdbDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetByDateRange(string startDate, string endDate)
        {
            var processId = $"{Guid.NewGuid()}";

            List<OmdbDto> result = new List<OmdbDto>();

            try
            {
                log.Info($"[{Request.Path.Value}] - Get movies by Date period request:: Date:{startDate}-{endDate}/ processId:{processId}");

                if (string.IsNullOrWhiteSpace(startDate) || string.IsNullOrWhiteSpace(endDate))
                    return BadRequest(string.Format(ConstMessage.BAD_REQUEST, "startDate/endDate"));

                var res = await _callserv.GetMoviesRangeFromDb(startDate, endDate, processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    if (res.ResponseObjects.Any())
                        foreach (var item in res.ResponseObjects)
                        {
                            var response = (OmdbDto)item;
                            result.Add(new OmdbDto
                            {
                                imdbID = response.imdbID,
                                ip_address = response.ip_address,
                                processing_time_ms = response.processing_time_ms,
                                search_token = response.search_token,
                                timestamp = response.timestamp,
                                _id = response._id
                            });
                        }
                    else
                        return NotFound(ConstMessage.NOT_FOUND);
                }
                else
                {
                    return NotFound((string)res.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Get movies by Date period internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }
            log.Info($"Get movies by Date period response:: Date:{startDate}-{endDate}/ processId:{processId}");

            return Ok(result);
        }


        [HttpGet]
        [Route("Report/GetByDate/{date}")]
        [ProducesResponseType(typeof(List<RequestStat>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetMostRequestByDate(string date)
        {
            var processId = $"{Guid.NewGuid()}";

            List<RequestStat> result = new List<RequestStat>();

            try
            {
                log.Info($"[{Request.Path.Value}] - Generate movies report by Date request:: Date:{date}/ processId:{processId}");

                if (string.IsNullOrWhiteSpace(date))
                    return BadRequest(string.Format(ConstMessage.BAD_REQUEST, "date"));

                var res = await _callserv.GenerateMovieReport(date, processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    if (res.usages.Any())
                        foreach (var item in res.usages)
                        {
                            var q = (UsageReport)item;
                            result.Add(new RequestStat { Timestamp = q.Day, Count = q.Count.Count });
                        }
                    else
                        return NotFound(ConstMessage.NOT_FOUND);
                }
                else
                {
                    return NotFound((string)res.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Generate movies report by Date internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }

            log.Info($"Generate movies report by Date response:: Date:{date}/ processId:{processId}");

            return Ok(result);
        }


        [HttpGet]
        [Route("Report/GetMostRequestByTitle/{title}")]
        [ProducesResponseType(typeof(List<StatByTitleRptDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetMostRequestByTitle(string title)
        {
            var processId = $"{Guid.NewGuid()}";

            List<StatByTitleRptDto> result = new List<StatByTitleRptDto>();

            try
            {
                log.Info($"[{Request.Path.Value}] - Generate movies report by Title request:: Title:{title}/ processId:{processId}");

                if (string.IsNullOrWhiteSpace(title))
                    return BadRequest(string.Format(ConstMessage.BAD_REQUEST, "title"));

                var res = await _callserv.GenerateMovieReportByTile(title, processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    if (res.TitleRpts.Any())
                        foreach (var item in res.TitleRpts)
                        {
                            var q = (RequestStatByTitle)item;
                            result.Add(new StatByTitleRptDto { Title = q.search_token, Count = q.Count });
                        }
                    else
                        return NotFound(ConstMessage.NOT_FOUND);
                }
                else
                {
                    return NotFound((string)res.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Generate movies report by title internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }

            log.Info($"Generate movies report by title response:: Date:{title}/ processId:{processId}");

            return Ok(result);
        }


        [HttpDelete]
        [Route("DeleteMovie/{title}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> DeleteMovie(string title)
        {
            var processId = $"{Guid.NewGuid()}";

            string result = string.Empty;

            try
            {
                log.Info($"[{Request.Path.Value}] - Delete movie request:: Title:{title} processId:{processId}");

                var res = await _callserv.DeleteMovie(title, processId);

                if (res is null)
                    return StatusCode(500, ConstMessage.INTERNAL_ERROR);

                if (res.Status)
                {
                    result = string.Format(ConstMessage.OPERATION_SUCCESS, title);
                }
                else
                {
                    return NotFound((string)res.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error($"Delete movie by title internal error:: exception:{ex}/processId: {processId}");
                return StatusCode(500, ConstMessage.INTERNAL_ERROR);
            }
            log.Info($"Delete movie by title response:: processId:{processId}");

            return Ok(result);
        }

    }
}
