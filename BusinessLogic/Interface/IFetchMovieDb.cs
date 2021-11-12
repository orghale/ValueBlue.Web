using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.BusinessLogic.Interface
{
    public interface IFetchMovieDb
    {
        Task<CommonResObj> GetAllMovieFromDb(string requestId);
        Task<CommonResObj> GetMovieFromDb(string title, string requestId);
        Task<CommonResObj> GetMoviesRangeFromDb(string date1, string date2, string requestId);
        Task<CommonResObj> GenerateMovieReport(string date, string requestId);
        Task<CommonResObj> DeleteMovie(string id, string requestId);
    
    }
}
