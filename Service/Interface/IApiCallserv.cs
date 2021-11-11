using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.Service.Interface
{
    public interface IApiCallserv
    {
        Task<ApiCallServObj> CallServGetAsync(EndpointParams e, string requestId = null);
    }
}
