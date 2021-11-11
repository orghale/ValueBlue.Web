using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ValueBlue.Web.Models;
using ValueBlue.Web.Service.Interface;

namespace ValueBlue.Web.Service
{
    public class ApiCoreService : IApiCoreService
    {
        public ApiCoreService()
        {
        }

        public async Task<ApiModel> CallGetAsync(string requestUri)
        {
            ApiModel apiResponse = new ApiModel();            
            try
            {

                apiResponse.RequestObject = requestUri;
                var stopWatch = new Stopwatch();

                using (HttpClient client = new HttpClient())
                {
                    stopWatch.Start();
                    using (var Response = await client.GetAsync(requestUri))
                    {
                        stopWatch.Stop();
                        apiResponse.ResponseTime = stopWatch.ElapsedMilliseconds;
                        if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            apiResponse.ResponseObject = await Response.Content.ReadAsStringAsync();
                            apiResponse.Status = true;
                        }
                        else
                        {
                            apiResponse.Message = Response.StatusCode.ToString();
                        }
                    }
                }
            }
            catch(HttpRequestException c)
            {
                apiResponse.Message = $"{c.Message}";
            }
            catch(Exception e)
            {
                apiResponse.Message = $"{e.Message}";
            }

            return apiResponse;
        }

    }
}
