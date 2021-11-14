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
    public class FetchMovieDb : IFetchMovieDb
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private readonly ISearchMovie _callserv;
        private readonly IMongoRepo _mongo;
        public FetchMovieDb(ISearchMovie callserv, IMongoRepo mongo)
        {
            _callserv = callserv;
            _mongo = mongo;
        }

        public async Task<CommonResObj> GetAllMovieFromDb(string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.GetAll(requestId);

            }
            catch (Exception e)
            {
                Log.Error($"[FetchMovieDb:GetAllMovieFromDb] Get all movie error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

        public async Task<CommonResObj> GetMovieFromDb(string title, string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.GetSingle(title, requestId);

            }
            catch (Exception e)
            {
                Log.Error($"[FetchMovieDb:GetMovieFromDb] Get movie error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

        public async Task<CommonResObj> GetMoviesRangeFromDb(string date1, string date2, string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.GetList(date1, date2, requestId);

            }
            catch (Exception e)
            {
                Log.Error($"[FetchMovieDb:GetMoviesRangeFromDb] Get movie range error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

        public async Task<CommonResObj> GenerateMovieReport(string date, string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.GenerateReport(date, requestId);
            }
            catch (Exception e)
            {
                Log.Error($"[FetchMovieDb:GenerateMovieReport] Generate report error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

        public async Task<CommonResObj> GenerateMovieReportByTile(string title, string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.GenerateReportByTitle(title, requestId);
            }
            catch (Exception e)
            {
                Log.Error($"[FetchMovieDb:GenerateMovieReportByTile] Generate report by title error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

        public async Task<CommonResObj> DeleteMovie(string id, string requestId)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                result = await _mongo.Delete(id, requestId);
            }
            catch (Exception e)
            {
                Log.Error($"[FetchMovieDb:DeleteMovie] Delete movie error:: exception:{e}/processId: {requestId}");
                result = null;
            }
            return result;
        }

    }

}
