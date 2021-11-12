using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.BusinessLogic.Interface;
using ValueBlue.Web.Models;
using ValueBlue.Web.Service.Interface;

namespace ValueBlue.Web.BusinessLogic
{
    public class SearchMovie : ISearchMovie
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IApiCallserv _callserv;
        private readonly IMongoRepo _mongo;
        public SearchMovie(IApiCallserv callserv, IMongoRepo mongo)
        {
            _callserv = callserv;
            _mongo = mongo;
        }

        public async Task<ApiResponseEntity> GetMovieFromOmdbApi(string title, string ip, string requestId)
        {
            ApiResponseEntity result;
            try
            {
                var param = new List<Param>
                {
                    new Param { key = "t", value = title}
                };

                EndpointParams searchCriteria = new EndpointParams(param);

                var res = await _callserv.CallServGetAsync(searchCriteria, requestId);

                if (res.Status)
                {
                    result = (ApiResponseEntity)res.apiResponse;
                    //result = new ApiResponseEntity();
                    if (result.Response is "True")
                    {
                        var resObj = new OmdbEntity
                        {
                            imdbID = result.imdbID,
                            ip_address = ip,
                            processing_time_ms = res.responseInterval,
                            search_token = title
                        };

                        ApiCallServObj imgResult = await _callserv.CallServGetPosterImageAsync(result.Poster, requestId);

                        resObj.Doc = imgResult.Status ? (string)imgResult.apiResponse : null;

                        await _mongo.Add(resObj, requestId);
                    }
                }
                else
                {
                    result = new ApiResponseEntity { Response = (string)res.Message } ;
                }
            }
            catch (Exception e)
            {
                Log.Error($"[SearchMovie:GetMovie]Get movie error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }
    }

}
