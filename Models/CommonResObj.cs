using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Models
{
    public class CommonResObj
    {
        public bool Status { get; set; }
        public object ResponseObject { get; set; }
        public IEnumerable<OmdbDto> ResponseObjects { get; set; }
        public IEnumerable<UsageReport> usages { get; set; }
        public IEnumerable<RequestStatByTitle> TitleRpts { get; set; }
        public string Message { get; set; }
    }

}
