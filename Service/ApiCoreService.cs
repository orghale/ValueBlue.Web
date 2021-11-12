using NLog;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ValueBlue.Web.Models;
using ValueBlue.Web.Service.Interface;

namespace ValueBlue.Web.Service
{
    public class ApiCoreService : IApiCoreService
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public ApiCoreService()
        {
        }

        public async Task<ApiModel> CallGetAsync(string requestUri, string requestId = null)
        {
            ApiModel apiResponse = new ApiModel();
            var stopWatch = new Stopwatch();
            try
            {

                apiResponse.RequestObject = requestUri;

                using (HttpClient client = new HttpClient())
                {
                    stopWatch.Start();
                    using (var Response = await client.GetAsync(requestUri))
                    {
                        stopWatch.Stop();
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
            catch (HttpRequestException c)
            {
                Log.Fatal(c, $"[CallGetAsync] Exception: {c.Message}/ request ID: {requestId}");
                apiResponse.Message = $"{c.Message}";
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"[CallGetAsync] Exception: {e.Message}/ request ID: {requestId}");
                apiResponse.Message = $"{e.Message}";
            }

            apiResponse.ResponseTime = stopWatch.ElapsedMilliseconds;

            return apiResponse;
        }


        public async Task<ApiModel> DownloadImageAsync(Uri uri, string requestId = null)
        {
            ApiModel result = new ApiModel();
            var stopWatch = new Stopwatch();
            try
            {

                using (var httpClient = new HttpClient())
                {
                    stopWatch.Start();

                    byte[] imageBytes = await httpClient.GetByteArrayAsync(uri);

                    stopWatch.Stop();

                    result.Status = true;
                    result.ResponseObject = Convert.ToBase64String(imageBytes);
                }
            }
            catch (HttpRequestException c)
            {
                Log.Fatal(c, $"[DownloadImageAsync] Exception: {c.Message}/ request ID: {requestId}");
                result.Message = $"{c.Message}";
            }
            catch (Exception e)
            {
                Log.Fatal(e, $"[DownloadImageAsync] Exception: {e.Message}/ request ID: {requestId}");
                result.Message = $"{e.Message}";
            }

            result.ResponseTime = stopWatch.ElapsedMilliseconds;

            return result;
        }
    }
}
