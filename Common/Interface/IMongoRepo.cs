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
        Task<CommonResObj> GetSingle(string id, string processId = null);
        Task<CommonResObj> GetAll(string processId = null);
        Task<CommonResObj> GetList(string id1, string Id2, string processId = null);
        Task<CommonResObj> GenerateReport(string date, string processId = null);
        Task<CommonResObj> GenerateReportByTitle(string title, string processId = null);
        Task<CommonResObj> Delete(string id, string processId = null);
        Task<CommonResObj> Update(OmdbEntity request, string processId = null);

        Task<bool> IsExist(string id, string processId = null);
    }

}
