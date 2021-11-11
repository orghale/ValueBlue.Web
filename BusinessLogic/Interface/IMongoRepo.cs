using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.BusinessLogic.Interface
{
    public interface IMongoRepo
    {
        Task<CommonResObj> Add(OmdbEntity request, string processId = null);
        Task<CommonResObj> Get(string id, string processId = null);
        Task<CommonResObj> Delete(string id, string processId = null);
        Task<CommonResObj> Update(OmdbEntity request, string processId = null);
    
    }

}
