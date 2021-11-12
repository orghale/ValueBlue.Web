using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.Service.Interface
{
    public interface IApiCoreService
    {
        Task<ApiModel> CallGetAsync(string requestUri, string requestId = null);
        Task<ApiModel> DownloadImageAsync(Uri uri, string requestId = null);
    }
}
