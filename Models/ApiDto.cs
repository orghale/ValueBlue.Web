using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Models
{
    public class ApiCallServObj
    {
        public ApiResponseEntity apiResponse { get; set; }
        public bool Status { get; set; }
        public object Message { get; set; }

    }
}
