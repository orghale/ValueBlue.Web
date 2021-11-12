using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.BusinessLogic.Interface
{
    public interface ISearchMovie
    {
        Task<ApiResponseEntity> GetMovieFromOmdbApi(string title, string ip, string requestId);
    }
}
