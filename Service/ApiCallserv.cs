using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValueBlue.Web.Models;
using ValueBlue.Web.Service.Interface;

namespace ValueBlue.Web.Service
{
    public class ApiCallserv : IApiCallserv
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IApiCoreService _api;
        private readonly AppSettings _config;
        public ApiCallserv(IOptions<AppSettings> config, IApiCoreService api)
        {
            this._config = config.Value;
            _api = api;
        }


        public async Task<ApiCallServObj> CallServGetAsync(EndpointParams e, string requestId = null)
        {
            ApiCallServObj result = new ApiCallServObj();
            try
            {
                Log.Debug($"[CallServGetAsync]PASS A -- > Get Async call serv request ID: {requestId}");

                Log.Debug($"[CallServGetAsync]PASS B -- > Base Url: {_config.CommonEndpoint.BaseUrl}/ request ID: {requestId}");

                string ep = $"?apikey{_config.CommonEndpoint.Apikey}{sbBuilder(e.epParam)}";

                var apiResponse = await _api.CallGetAsync($"{_config.CommonEndpoint.BaseUrl}{ep}");

                result.responseInterval = apiResponse.ResponseTime;

                if (apiResponse.Status)
                {
                    result.apiResponse = JsonConvert.DeserializeObject<ApiResponseEntity>(apiResponse.ResponseObject.ToString());
                    result.Status = apiResponse.Status;

                    Log.Debug($"[CallServGetAsync]PASS D -- > Api service response body: {apiResponse.ResponseObject}/ request ID: {requestId}");
                }
                else
                {
                    result.Message = $"API service response code {apiResponse.Message}";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Exception encountered: {ex.Message}";
                Log.Fatal(ex, $"[CallServGetAsync] Get Async call exception: {ex}/ request ID: {requestId}");
            }

            return result;
        }

        private string sbBuilder(List<Param> param)
        {
            var sb = new StringBuilder();
            foreach (var p in param)
            {
                sb.Append($"&{p.key}={p.value}");
            }
            return sb.ToString();
        }

    }
}
