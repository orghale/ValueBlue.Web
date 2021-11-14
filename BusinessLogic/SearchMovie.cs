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

        public async Task<ImageSearchResponse> GetMovieFromOmdbApi(string title, string ip, string requestId)
        {
            ImageSearchResponse result;
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
                    var resObj = new OmdbEntity();

                    var response = (ApiResponseEntity)res.apiResponse;

                    if (response.Response is "True")
                    {
                        //if (!await _mongo.IsExist(response.Title, requestId))
                        //{
                            resObj.imdbID = response.imdbID;
                            resObj.ip_address = ip;
                            resObj.processing_time_ms = res.responseInterval;
                            resObj.search_token = title.ToUpper().Trim();

                            ApiCallServObj imgResult = await _callserv.CallServGetPosterImageAsync(response.Poster, requestId);

                            resObj.Doc = imgResult.Status ? (string)imgResult.apiResponse : null;

                            await _mongo.Add(resObj, requestId);
                        //}

                        result = new ImageSearchResponse
                        {
                            Status = res.Status,
                            entity = response,
                            Image = resObj.Doc is null ? null : (string)resObj.Doc
                        };

                        return result;

                    }
                }
                result = new ImageSearchResponse
                {
                    Status = res.Status,
                    entity = new ApiResponseEntity
                    {
                        Response = (string)res.Message
                    }
                };
            }
            catch (Exception e)
            {
                Log.Error($"[SearchMovie:GetMovieFromOmdbApi]Get movie error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

        public async Task<CommonResObj> GetMoviePoster(string title, string ip, string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.GetSingle(title, requestId);
                if (!result.Status || result.ResponseObject is null)
                {
                    var res = await GetMovieFromOmdbApi(title, ip, requestId);

                    result.Message = res.Status ? (string)res.Image : null;
                    result.Status = res.Status;
                }
                else
                    result.Message = (string)((OmdbEntity)result.ResponseObject).Doc;
            }
            catch (Exception e)
            {
                Log.Error($"[SearchMovie:GetMoviePoster] Get movie error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

    }

}
