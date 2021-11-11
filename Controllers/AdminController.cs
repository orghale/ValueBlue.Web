using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.Extension;

namespace ValueBlue.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthAttribute]
    public class AdminController : ControllerBase
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public AdminController()
        {

        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> GetRate(string id)
        {
            var processId = $"{Guid.NewGuid()}";
            try
            {
                //if (_config.debug)
                    log.Info($"Request:: Id:{id}/ processId:{processId}");

              
            }
            catch (Exception ex)
            {
                log.Error($"{ex}{Environment.NewLine}/processId: {processId}");
                return StatusCode(500);
            }
            log.Info($"Response:: Id:{id}/ processId:{processId}");

            return Ok(id);
        }

    }
}
